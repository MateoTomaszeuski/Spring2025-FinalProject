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
// hi
var builder = WebApplication.CreateBuilder(args);

const string serviceName = "Consilium";
var Uri = builder.Configuration["OTEL_URL"] ?? "";

DateTime started = DateTime.UtcNow;
builder.Logging.AddConsole();

var meter = new Meter(serviceName, "1.0.0");
var errorCountCounter = meter.CreateCounter<long>("error_count", description: "Total number of errors");
var errorMessageCounter = meter.CreateCounter<long>("error_messages", description: "Number of errors by message", unit: "1");

meter.CreateObservableGauge(
    "application_uptime_seconds",
    () => new[] { new Measurement<double>((DateTime.UtcNow - started).TotalSeconds) },
    description: "Current application uptime in seconds"
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

app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();

public class MyConcurrentUserTracker {
    private static long _count;
    public long CurrentCount => Interlocked.Read(ref _count);
    public void Increment() => Interlocked.Increment(ref _count);
    public void Decrement() => Interlocked.Decrement(ref _count);
}