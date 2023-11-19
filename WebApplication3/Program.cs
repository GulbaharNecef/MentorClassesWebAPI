using Microsoft.EntityFrameworkCore;
using WebApplication3;
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
builder.Services.AddSwaggerGen();
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
