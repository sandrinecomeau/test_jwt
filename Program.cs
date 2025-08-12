using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"].Trim());
var signingKey = new SymmetricSecurityKey(keyBytes);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal",
        policy => policy
            .WithOrigins(
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowLocal");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();


