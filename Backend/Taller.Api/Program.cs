// Program.cs 
using Taller.Api.Controllers;
using Taller.Api.Data.Repositories;
using Taller.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CARGA explícita de appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


Console.WriteLine(">>> CONFIG JWT KEY: " + builder.Configuration["Jwt:Key"]);
Console.WriteLine(">>> CONFIG JWT ISSUER: " + builder.Configuration["Jwt:Issuer"]);
Console.WriteLine(">>> CONFIG JWT AUD: " + builder.Configuration["Jwt:Audience"]);
Console.WriteLine("JWT KEY LEÍDA: " + builder.Configuration["Jwt:Key"]);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(">>> CONNECTION STRING LEÍDA: " + connectionString);


// Add services to the container.
builder.Services.AddControllers();

// Register Repositories
builder.Services.AddScoped<IAuthRepository>(sp => new AuthRepository(connectionString));
builder.Services.AddScoped<IVehiculoRepository>(sp => new VehiculoRepository(connectionString));
builder.Services.AddScoped<IClienteRepository>(sp => new ClienteRepository(connectionString));
builder.Services.AddScoped<ICitaRepository>(sp => new CitaRepository(connectionString));
builder.Services.AddScoped<IServicioRepository>(sp => new ServicioRepository(connectionString));
builder.Services.AddScoped<IMaterialRepository>(sp => new MaterialRepository(connectionString));
builder.Services.AddScoped<IInformeRepository>(sp => new InformeRepository(connectionString));

// Register Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VehiculoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<CitaService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<InformeService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

Console.WriteLine(">>> JWT KEY EN PIPELINE: " + builder.Configuration["Jwt:Key"]);
Console.WriteLine(">>> JWT ISSUER EN PIPELINE: " + builder.Configuration["Jwt:Issuer"]);
Console.WriteLine(">>> JWT AUDIENCE EN PIPELINE: " + builder.Configuration["Jwt:Audience"]);

// JWT AUTH CORRECTO
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });



var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();   
app.UseAuthorization();  

app.MapControllers();


app.Run();

