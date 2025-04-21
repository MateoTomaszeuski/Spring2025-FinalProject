using Consilium.API;
using Consilium.API.DBServices;
using Consilium.API.Metrics;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.Runtime;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Collections.Generic;


var builder = WebApplication.CreateBuilder(args);
string UptimeFilePath = Path.Combine(builder.Environment.WebRootPath, "uptime.txt");

DateTime started = DateTime.UtcNow;
builder.Logging.AddConsole();

const string serviceName = "Consilium";
var Uri = builder.Configuration["OTEL_URL"] ?? "";
double previousAggregated = LoadAggregatedUptimeFromStore();  // e.g. from a file or database
var meter = new Meter(serviceName, "1.0.0");
var errorCountCounter = meter.CreateCounter<long>("error_count", description: "Total number of errors");
var errorMessageCounter = meter.CreateCounter<long>("error_messages", description: "Number of errors by message", unit: "1");

Console.WriteLine(Uri);
if (Uri != "") {


    //  Current uptime in seconds
    meter.CreateObservableGauge(
        "application_uptime_seconds",
        () => new[] { new Measurement<double>((DateTime.UtcNow - started).TotalSeconds) },
        description: "Current application uptime in seconds"
    );

    //  Aggregated uptime (across restarts)
    meter.CreateObservableGauge(
        "application_aggregated_uptime_seconds",
        () =>
        {
            var total = previousAggregated + (DateTime.UtcNow - started).TotalSeconds;
            return new[] { new Measurement<double>(total) };
        },
        description: "Aggregated application uptime in seconds"
    );



    builder.Services.AddSingleton<TodoMetrics>();
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService(serviceName: serviceName))
        .WithLogging(logging => logging
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(Uri);
                options.Protocol = OtlpExportProtocol.Grpc;
            }))
        .WithMetrics(metrics => metrics
            .AddMeter(serviceName)
            .AddMeter("Consilium.Todos")
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

// Add services to the container.
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IIDMiddlewareConfig, MiddlewareConfig>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IDBService, DBService>();
builder.Services.AddSingleton<IDBService, DBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<IdentityMiddleware>();
app.UseRouting();

app.Lifetime.ApplicationStopping.Register(() =>
{
    var aggregated = previousAggregated + (DateTime.UtcNow - started).TotalSeconds;
    SaveAggregatedUptimeToStore(aggregated);
});

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
    app.Logger.LogInformation("Checking health");

    return Results.Ok("healthy");
});

// feature flag accomplished!

// Need to make a change to test formatting, test 3
bool featureFlag = builder.Configuration["feature_flag"] == "true";

if (featureFlag) {
    app.MapGet("/secret", () =>
    {
        using var activity = activitySource.StartActivity("HomeActivity");
        activity?.SetTag("Secret", "Checking Secrets");
        app.Logger.LogInformation("Inside the secret feature flag");

        return "Secrets are hidden within.";
    });
}

app.Use(async (ctx, next) =>
{
    try {
        await next();
    } catch (Exception ex) {
        errorCountCounter.Add(1);
        errorMessageCounter.Add(
    1,
    new[]
    {
         new KeyValuePair<string, object?>("message", ex.GetType().Name)
    });
        throw;
    }
});
//app.UseHttpsRedirection();

var userGauge = meter.CreateObservableGauge(
    "concurrent_users",
    () =>
    {
        var count = MyConcurrentUserTracker.CurrentCount; // your in‑mem or distributed count
        return new[] { new Measurement<long>(count) };
    },
    description: "Number of currently active users"
);

//  Most popular pages: leverage the AspNetCoreInstrumentation counter by route,
//  or create your own with a label:
var pageHits = meter.CreateCounter<long>("page_requests_total", description: "Page hits by route");
app.Use(async (ctx, next) =>
{
    await next();
    var route = ctx.GetEndpoint()?.DisplayName ?? ctx.Request.Path;
    pageHits.Add(
    1,
    new[]
    {
        new KeyValuePair<string, object?>("page", route)
    }
);

});

app.Lifetime.ApplicationStopping.Register(() =>
{
    var aggregated = previousAggregated
                   + (DateTime.UtcNow - started).TotalSeconds;
    SaveAggregatedUptimeToStore(aggregated);
});

app.UseAuthorization();

app.MapControllers();

app.Run();

double LoadAggregatedUptimeFromStore() {
    if (!File.Exists(UptimeFilePath))
        return 0;

    var text = File.ReadAllText(UptimeFilePath);
    if (double.TryParse(text,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var value)) {
        return value;
    }
    return 0;
}

void SaveAggregatedUptimeToStore(double seconds) {
    var text = seconds.ToString(CultureInfo.InvariantCulture);
    File.WriteAllText(UptimeFilePath, text);
}

public static class MyConcurrentUserTracker {
    private static long _count;
    public static long CurrentCount => Interlocked.Read(ref _count);

    public static void Increment() => Interlocked.Increment(ref _count);
    public static void Decrement() => Interlocked.Decrement(ref _count);
}