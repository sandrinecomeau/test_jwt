using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var signingKey = JwtKeyHelper.BuildSigningKey(builder.Configuration);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.MapFallback(async context =>
{
    var ep = context.GetEndpoint()?.DisplayName ?? "(null)";
    var path = context.Request.Path.Value ?? "";
    var basePath = context.Request.PathBase.Value ?? "";
    await context.Response.WriteAsync($"FALLBACK hit\nPathBase='{basePath}'\nPath='{path}'\nMatched='{ep}'");
});
app.Run();


