using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

//Logging
using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

logger.Information("Flights API started!");

builder.Logging.AddSerilog(logger);

//End of Logging

builder.Services.AddScoped<MongoDbContext>();

builder.Services.AddScoped<IFlightsRepository, FlightsMongoDbRepository>();
builder.Services.AddScoped<IAirlinesRepository, AirlinesMongoDbRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen();


// Teste para fazer commit e agora no devops
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

app.Run();
