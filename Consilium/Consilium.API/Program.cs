using Consilium.API;
using Consilium.API.DBServices;
using EmailAuthenticator;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "Consilium";
var Uri = builder.Configuration["OTEL_URL"] ?? throw new Exception("No collector uri string was found.");
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
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
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
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(Uri);
            options.Protocol = OtlpExportProtocol.Grpc;
        })

    );

string connString = builder.Configuration["DB_CONN"] ?? throw new Exception("No connection string was found.");
Console.WriteLine("Connection String: " + connString);
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

//app.UseMiddleware<IdentityMiddleware>();
app.UseRouting();

//change
app.MapGet("", () => "Welcome to the Consilium Api");

// feature flag accomplished!
// Need to make a change to test formatting, test 3
bool featureFlag = builder.Configuration["feature_flag"] == "true";

if (featureFlag) {
    app.MapGet("/secret", () => "Secrets are hidden within.");
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();