using Serilog;
using Serilog.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog globally
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Capture Debug, Information, Warning, Error
    .WriteTo.Debug() // Send logs to Visual Studio Output Window
    .WriteTo.Console() // Optional: Keep console logs
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // Optional: Keep file logs
    .CreateLogger();

// Replace default logging with Serilog
builder.Services.AddSerilog(); // Registers Serilog as the logging provider

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    // Flush logs on shutdown
    Log.CloseAndFlush();
}