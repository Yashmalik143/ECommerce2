using ECommerce;
using ECommerce.ExtectionMethod;
using ECommerce.GlobalException;
using Microsoft.EntityFrameworkCore.Metadata;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDatabase(builder.Configuration)
    .AddServices()
    .AddJWT(builder.Configuration)
    .AddNewtonJson()
    .Swagger();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new MySampleActionFilter());
})
    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o=>o.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo
{ 
    Title = "MyAPI",
    Version ="v1",
    Description = "Des1"
}));

//?----------------------- Serilog Configuraion  --------------------------------

#region Serilog Configuration

string con = builder.Configuration.GetConnectionString("DefaultConnection");
string table = "Logs";

var _logger = new LoggerConfiguration()
.MinimumLevel.Debug()
    .WriteTo.MSSqlServer(con, table).CreateLogger();
builder.Logging.AddSerilog(_logger);

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.MSSqlServer(con, table).CreateLogger();

#endregion Serilog Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json","My API v1"));
}

//? GLobal Exception handling----------------------------
app.UseMiddleware<GlobalException>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();