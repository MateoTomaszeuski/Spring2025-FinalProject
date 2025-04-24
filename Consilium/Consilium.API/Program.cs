using Consilium.API;
using Consilium.API.DBServices;
using Consilium.API.Metrics;
using EmailAuthenticator;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "Consilium";
var Uri = builder.Configuration["OTEL_URL"] ?? "";

string UptimeCsvPath = Path.Combine(
    builder.Environment.WebRootPath,
    "uptime",
    "uptime.csv"
);
if (!Directory.Exists(Path.GetDirectoryName(UptimeCsvPath)))
    Directory.CreateDirectory(Path.GetDirectoryName(UptimeCsvPath)!);

DateTime started = DateTime.UtcNow;
builder.Logging.AddConsole();

double previousAggregated = LoadAggregatedUptimeFromCsv(UptimeCsvPath);
var firstStart = LoadFirstDeploymentStartTime(UptimeCsvPath)
                   ?? DateTime.UtcNow;

var meter = new Meter(serviceName, "1.0.0");
var errorCountCounter = meter.CreateCounter<long>("error_count", description: "Total number of errors");
var errorMessageCounter = meter.CreateCounter<long>("error_messages", description: "Number of errors by message", unit: "1");

meter.CreateObservableGauge(
    "application_uptime_seconds",
    () => new[] { new Measurement<double>((DateTime.UtcNow - started).TotalSeconds) },
    description: "Current application uptime in seconds"
);
meter.CreateObservableGauge(
    "application_aggregated_uptime_seconds",
    () =>
    {
        var current = (DateTime.UtcNow - started).TotalSeconds;
        return new[] {
            new Measurement<double>(previousAggregated + current)
        };
    },
    description: "Aggregated application uptime in seconds"
);
meter.CreateObservableGauge(
    "application_uptime_percentage",
    () =>
    {
        var currentSecs = (DateTime.UtcNow - started).TotalSeconds;

        var totalUp = previousAggregated + currentSecs;

        var windowSecs = (DateTime.UtcNow - firstStart).TotalSeconds;

        var pct = windowSecs > 0
                  ? (totalUp / windowSecs) * 100
                  : 0;

        return new[] { new Measurement<double>(pct) };
    },
    description: "Percent of time healthy since first deployment"
);
var myConcurrentUserTracker = new MyConcurrentUserTracker();
meter.CreateObservableGauge(
    "concurrent_users",
    () =>
    {
        var count = myConcurrentUserTracker.CurrentCount;
        return new[] { new Measurement<long>(count) };
    }, description: "Number of currently active users"
);

var podName = Environment.GetEnvironmentVariable("HOSTNAME")
              ?? Guid.NewGuid().ToString();
if (Uri != "") {

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService(serviceName: serviceName, serviceInstanceId: podName))
        .WithLogging(logging => logging
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(Uri);
                options.Protocol = OtlpExportProtocol.Grpc;
            })
            .AddConsoleExporter())
        .WithMetrics(metrics => metrics
            .AddMeter(serviceName)
            .AddMeter("Consilium.Todos")
            .AddMeter("Consilium.Accounts")
            .AddMeter("Consilium.NewFeature")
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(Uri);
                options.Protocol = OtlpExportProtocol.Grpc;
            })
            .AddView("http_response_duration_seconds", new ExplicitBucketHistogramConfiguration {
                Boundaries = new double[] { 0.1, 0.5, 1, 2, 5, 10 }
            }))
        .WithTracing(tracing => tracing
            .AddSource(serviceName)
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(Uri);
                options.Protocol = OtlpExportProtocol.Grpc;
            })

        );
}
ActivitySource? activitySource = new ActivitySource(serviceName);

string connString = builder.Configuration["DB_CONN"] ?? throw new Exception("No connection string was found.");
builder.Services.AddSingleton<IDbConnection>(provider =>
{
    return new NpgsqlConnection(connString);
});

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IIDMiddlewareConfig, MiddlewareConfig>();
builder.Services.AddSingleton<TodoMetrics>();
builder.Services.AddSingleton<AccountMetrics>();
builder.Services.AddSingleton<NewFeatureMerics>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDBService, DBService>();

var feedbackUri = builder.Configuration["FEEDBACK_WEBHOOK"] ?? "";
if (!string.IsNullOrEmpty(feedbackUri)) {
    builder.Services.AddHttpClient("FeedbackWebhock", client => client.BaseAddress = new Uri(feedbackUri));
}

var app = builder.Build();

var pageHits = meter.CreateCounter<long>("page_requests_total", description: "Page hits by route");

app.Use(async (ctx, next) =>
{
    myConcurrentUserTracker.Increment();
    try {
        await next();
    } catch (Exception ex) {
        errorCountCounter.Add(1);
        errorMessageCounter.Add(
        1, new[] { new KeyValuePair<string, object?>("message", ex.GetType().Name) });
        throw;
    } finally {
        var route = ctx.GetEndpoint()?.DisplayName ?? ctx.Request.Path;
        pageHits.Add(
            1, new[] { new KeyValuePair<string, object?>("page", route) }
        );
        myConcurrentUserTracker.Decrement();
    }
});
bool featureFlag = builder.Configuration["feature_flag"] == "true";

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || featureFlag) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<IdentityMiddleware>();
app.UseRouting();

//change
app.MapGet("", () =>
{
    using var activity = activitySource.StartActivity("HomeActivity");
    activity?.SetTag("home", "home");
    app.Logger.LogInformation("Home page accessed");
    return "Welcome to the Consilium Api";
});

app.MapGet("/health", () =>
{
    using var activity = activitySource.StartActivity("HomeActivity");
    activity?.SetTag("Health", "Checking Health");

    return Results.Ok("healthy");
});

app.MapGet("/error", () =>
{
    throw new Exception("This is an error");
});

app.MapGet("/timecheck", () =>
{
    using var activity = activitySource.StartActivity("HomeActivity");
    activity?.SetTag("actvate 1 min delay", "wating 1 min");
    Task.Delay(60).Wait();
    return Results.Ok("done");
});

// Need to make a change to test formatting, test 3
if (featureFlag) {
    app.MapGet("/secret", () =>
    {
        using var activity = activitySource.StartActivity("HomeActivity");
        activity?.SetTag("Secret", "Checking Secrets");
        app.Logger.LogInformation("Inside the secret feature flag");

        return "Secrets are hidden within.";
    });
}

//app.UseHttpsRedirection();
app.Lifetime.ApplicationStopping.Register(() =>
{
    var uptime = (DateTime.UtcNow - started).TotalSeconds;
    // 1) append this pod’s row
    AppendUptimeRow(UptimeCsvPath, podName, DateTime.UtcNow, uptime);
    // 2) (optional) log the new total
    var totalSoFar = previousAggregated + uptime;
    app.Logger.LogInformation(
        "Shutting down pod {pod} after {uptime}s (aggregate {total}s)",
        podName,
        uptime,
        totalSoFar);
});

app.UseAuthorization();
app.MapControllers();
app.Run();

// Metrics Stuff
double LoadAggregatedUptimeFromCsv(string path) {
    if (!File.Exists(path)) return 0;
    var lines = File.ReadAllLines(path);
    double total = 0;
    foreach (var line in lines.Skip(1))         // skip header
    {
        var cols = line.Split(',');
        if (cols.Length < 3) continue;
        if (double.TryParse(cols[2],
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out var secs))
            total += secs;
    }
    return total;
}
void AppendUptimeRow(string path, string podName, DateTime shutdownUtc, double uptimeSeconds) {
    var isNew = !File.Exists(path);
    using var w = new StreamWriter(path, append: true);
    if (isNew)
        w.WriteLine("podName,shutdownTime,uptimeSeconds");
    // use ISO 8601 for the timestamp
    var ts = shutdownUtc.ToString("o", CultureInfo.InvariantCulture);
    w.WriteLine($"{podName},{ts},{uptimeSeconds.ToString(CultureInfo.InvariantCulture)}");
}
DateTime? LoadFirstDeploymentStartTime(string csvPath) {
    if (!File.Exists(csvPath))
        return null;

    // skip header, grab the first data row
    var firstLine = File.ReadAllLines(csvPath).Skip(1).FirstOrDefault();
    if (string.IsNullOrEmpty(firstLine))
        return null;

    var cols = firstLine.Split(',');
    if (cols.Length < 3)
        return null;

    if (!DateTime.TryParse(cols[1],
                           null,
                           DateTimeStyles.RoundtripKind,
                           out var shutdown))
        return null;

    if (!double.TryParse(cols[2],
                         NumberStyles.Float,
                         CultureInfo.InvariantCulture,
                         out var uptimeSecs))
        return shutdown;

    return shutdown - TimeSpan.FromSeconds(uptimeSecs);
}

public class MyConcurrentUserTracker {
    private static long _count;
    public long CurrentCount => Interlocked.Read(ref _count);
    public void Increment() => Interlocked.Increment(ref _count);
    public void Decrement() => Interlocked.Decrement(ref _count);
}