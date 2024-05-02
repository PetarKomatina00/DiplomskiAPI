using Azure.Storage.Blobs;
using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using DiplomskiAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins(
            "https://localhost:7194", 
            "https://localhost:3000",
            "http://localhost:3000",
            "http://localhost:7194")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("X-Pagination");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Za jwt token
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization header using the bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the next input below. \r\n\r\n" +
        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAzureConnection"),
        sqlServerOptionsAction : sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton<APIResponse, APIResponse>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

//Govorimo da ce da se koristi Jwt token
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
//Ako API zove s nekog drugog URL-a
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("corspolicy", build =>
//    {
//        build.WithOrigins(
//            "http://localhost:3000",
//            "https://localhost:3000",
//            "https://127.0.0.1:3000",
//            "http://127.0.0.1:3000",
//            "http://192.168.1.36:3000",
//            "http://192.168.1.36",
//            "https://192.168.1.36:3000",
//            "https://192.168.1.36",
//            "https://localhost:7194",
//            "https://localhost:7194/api",
//            "https://localhost:7194/api/Lek",
//            "https://localhost:7194/api/Lek/BestSellers"
//            )
//            .AllowAnyMethod().AllowAnyHeader();
//    });
//});
var app = builder.Build();
app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
        c.RoutePrefix = String.Empty;
    });
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MyPolicy");
//app.UseCors(x =>
//{
//    //x.AllowAnyHeader()
//    //.AllowAnyMethod() 
//    //.WithOrigins("http://localhost:3000",
//    //"https://localhost:3000",
//    //"http://192.168.1.54:3000",
//    //"https://192.168.1.54:3000",
//    //"https://109.92.104.228",
//    //"http://109.92.104.228",
//    //"https://109.92.104.228",
//    //"https://192.168.1.48",
//    //x.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
//    //"https://192.168.1.48");
//});
//app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));

//app.UseCors("corspolicy");
app.UseAuthentication();
//Za cors

//app.Use((ctx, next) =>
//{
//    ctx.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:3000";
//    return next();
//});
app.UseAuthorization();

app.MapControllers();

app.Run();
