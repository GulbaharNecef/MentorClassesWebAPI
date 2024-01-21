using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Principal;
using System.Text;
using WebApplication3;
using WebApplication3.Auth;
using WebApplication3.AutoMapper;
using WebApplication3.IRepositories;
using WebApplication3.IRepositories.ISchoolRepos;
using WebApplication3.IRepositories.IStudentRepos;
using WebApplication3.IUnitOfWorks;
using WebApplication3.Repositories;
using WebApplication3.Services.Abstraction;
using WebApplication3.Services.Implementation;
using WebApplication3.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//String connection = @"Server=localhost;Database=YourDatabase;TrustServerCertificate=True;Trusted_Connection=True;";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Restaurant Final API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

builder.Services.AddDbContext<NewDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddDbContext<NewDbContext>(option =>
//{
//    option.UseSqlServer(connection);
//});
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// identity
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<NewDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            //ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecretKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false
        };
    });



//logger ile loglama
/*builder.Services.AddLogging(i =>
{
    i.ClearProviders();
    i.SetMinimumLevel(LogLevel.Information);
    i.AddConsole();
    i.AddDebug();
    //i.AddProvider(MyCustomProvider());
}
) ;*/


Logger? log = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Debug)
    .WriteTo.File("Logs/myJsonLogs.json")
    .WriteTo.File("Logs/mylogs.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "MySeriLog",
        AutoCreateSqlTable = true
    },
null, null, LogEventLevel.Warning, null,
columnOptions: new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
            new SqlColumn(columnName:"User_Id", SqlDbType.NVarChar)
    }
},
 null, null
 )
.Enrich.FromLogContext()
.MinimumLevel.Information()
.CreateLogger();

Log.Logger = log;
builder.Host.UseSerilog(log);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
