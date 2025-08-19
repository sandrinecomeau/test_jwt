using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
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

builder.Services.AddSingleton<IAuthService, AuthService>();

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
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,

            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }

        };

        //facultatif, je l'ai mis pour débugger 
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"[JWT FAIL] {ctx.Exception.GetType().Name}: {ctx.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine($"[JWT CHALLENGE] {ctx.Error} - {ctx.ErrorDescription}");
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("[JWT OK] token validé");
                return Task.CompletedTask;
            }
        };
    });

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminsOnly", policy =>
//         policy.RequireRole("admin"));
// });

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


