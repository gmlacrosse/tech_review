using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StargateContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Conditional(e => e.Level == Serilog.Events.LogEventLevel.Error, wt => wt.SQLite(sqliteDbPath: Environment.CurrentDirectory + @"\starbase.db", tableName: "ErrorLog", storeTimestampInUtc: true))
    .WriteTo.Conditional(e => e.Level == Serilog.Events.LogEventLevel.Debug, wt => wt.SQLite(sqliteDbPath: Environment.CurrentDirectory + @"\starbase.db", tableName: "DebugLog", storeTimestampInUtc: true))
    .WriteTo.Conditional(e => e.Level == Serilog.Events.LogEventLevel.Information, wt => wt.SQLite(sqliteDbPath: Environment.CurrentDirectory + @"\starbase.db", tableName: "InformationLog", storeTimestampInUtc: true))
    .CreateLogger();

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:7087")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


