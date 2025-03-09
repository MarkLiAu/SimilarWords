using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(options =>
    {
        options.Authority = Configuration["OidcJwt:Authority"]?? throw new NullReferenceException("OidcJwt:Authority");
        options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Configuration["OidcJwt:Authority"] ,
                ValidateAudience = false
            };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureSetup(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.MapFallbackToFile("index.html"); // Serve index.html for unmatched routes

app.MapControllers();

app.Run();
