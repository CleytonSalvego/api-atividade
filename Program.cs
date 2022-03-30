using api;
using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Inicia a API
StartAPI(builder);

void StartAPI(WebApplicationBuilder builder)
{
    ConfigureAuthentication(builder);
    ConfigureMvc(builder);
    ConfigureServices(builder);

    var app = builder.Build();

    //Configura Autoriza��o e Autentica��o
    app.UseAuthentication(); // Quem voc� �?
    app.UseAuthorization();  // O que voc� pode fazer?
    //Configura para API considerar controller para rotas
    app.MapControllers();
    //Configura para adicionar o Swagger para documenta��o
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Documenta��o API ACTi");
        
    });
    //Inicia o programa
    app.Run();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    // Configura��o do Token
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    // Autentica��o e Autoriza��o
    builder.Services.
        AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).
        AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    //Adiciona funcionalidade da gest�o por Controller
    builder
        .Services
       
        .AddMemoryCache()   //Gest�o do cache
        .AddControllers()
         .AddJsonOptions(x =>
         {
             x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
             x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
         })
        .ConfigureApiBehaviorOptions(options => {
            options.SuppressModelStateInvalidFilter = true;
        });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DataBaseContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "API - Plataforma ACTi",
            Description = "Documenta��o da API - Plataforma ACTi",
            Version = "v1"
        });

    });
}


