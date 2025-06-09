using EventScheduler.Server.Mappings;
using EventScheduler.Server.Models;
using EventScheduler.Server.Repositories;
using EventScheduler.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using EventScheduler.Server.Utils;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddDbContext<SpektraDbContext>(options =>
{
    string connection_string = builder.Configuration.GetConnectionString("SpektraDB")!;
    options.UseSqlServer(connection_string);
});

var jwtsetting = builder.Configuration.GetRequiredSection("JwtSetting");
builder.Services.Configure<JwtSettings>(jwtsetting);
builder.Services.AddScoped<JwtTokenGen>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer=true,
            ValidateAudience=true,
            ValidateLifetime=true,
            ValidIssuer= jwtsetting["Issuer"],
            ValidAudience= jwtsetting["Audience"],

            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsetting["SecretKey"]))
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(MappingEvent), typeof(MappingTicket));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
