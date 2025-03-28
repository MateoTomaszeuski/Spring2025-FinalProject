using Consilium.API;
using Consilium.API.DBServices;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration["DB_CONN"] ?? throw new Exception("No connection string was found.");
Console.WriteLine("Connection String: " + connString);
builder.Services.AddSingleton<IDbConnection>(provider =>
{
    return new NpgsqlConnection(connString);
});
// Add services to the container.

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
//change
app.MapGet("", () => "Welcome to the Consilium Api");

// feature flag accomplished!
// Need to make a change to test formatting, test 3
bool featureFlag = builder.Configuration["feature_flag"] == "true";

if (featureFlag) {
    app.MapGet("/secret", () => "Secrets are hidden within.");
}

app.MapGet("/account", (DBService service) => service.GetAllUsers());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();