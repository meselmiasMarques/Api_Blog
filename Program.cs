using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1",
        Description = "API ASP.NET Blog"
    });
});

var app = builder.Build();

ConfigureSwagger(app);
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();
//app.UseResponseCompression();
app.UseStaticFiles();
app.MapControllers();
app.Run();


void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");

    var smtpConfiguration = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtpConfiguration);
    Configuration.Smtp = smtpConfiguration;
}

void ConfigureAuthentication(WebApplicationBuilder builder) 
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

}

void ConfigureMvc(WebApplicationBuilder builder)
{
    //dados em cache
    builder.Services.AddMemoryCache();
    
    //adiciona compressao
    builder.Services.AddResponseCompression(x =>
    {
        x.Providers.Add<GzipCompressionProvider>();
    });

    //configura compressao
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });
    
    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options => 
            { options.SuppressModelStateInvalidFilter = true; }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //ignora cicos de cadeias
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
}

void ConfigureServices(WebApplicationBuilder buider)
{
    //injeção de Dependencia
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddTransient<TokenService>();
    buider.Services.AddTransient<EmailService>();

}

void ConfigureSwagger(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Api");
            c.RoutePrefix = string.Empty;
        });
    }
}