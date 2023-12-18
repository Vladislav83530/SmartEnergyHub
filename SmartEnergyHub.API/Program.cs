using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.API.Settings;
using SmartEnergyHub.BLL.Auth;
using SmartEnergyHub.BLL.Auth.Interfaces;
using SmartEnergyHub.BLL.Customer;
using SmartEnergyHub.BLL.Customer.Abstract;
using SmartEnergyHub.BLL.Device_;
using SmartEnergyHub.BLL.Device_.Abstract;
using SmartEnergyHub.BLL.DeviceGenerator;
using SmartEnergyHub.BLL.DeviceGenerator.Abstract;
using SmartEnergyHub.BLL.Residence_;
using SmartEnergyHub.BLL.Residence_.Abstract;
using SmartEnergyHub.DAL.EF;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<DeviceGeneratorSettings>
        (builder.Configuration.GetSection("DeviceGeneratorSettings"));

builder.Services.AddControllers();

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add(new ExceptionFilter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ICustomerInfoProvider, CustomerInfoProvider>();
builder.Services.AddScoped<IResidenceProvider, ResidenceProvider>();
builder.Services.AddScoped<IDeviceGenerator, DeviceGenerator>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
