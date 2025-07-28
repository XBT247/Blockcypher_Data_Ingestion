using BlockCypher.Application;
using BlockCypher.Infrastructure;
using BlockCypher.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Config from environment (Docker etc)
var config = builder.Configuration;

// Add layers via extension methods
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);
builder.Services.AddPersistence(config);

// Add Controllers, Swagger, CORS, HealthChecks
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlockCypher DataIngestion API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddHealthChecks()
    .AddSqlite(config.GetConnectionString("DefaultConnection") ?? "Data Source=blockcypher.db");

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseDeveloperExceptionPage();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlockCypherDbContext>();
    db.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlockCypher DataIngestion API v1"));

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { } // required for WebApplicationFactory<TProgram>
