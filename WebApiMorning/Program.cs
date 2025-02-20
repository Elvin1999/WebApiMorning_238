using Microsoft.EntityFrameworkCore;
using WebApiMorning.Data;
using WebApiMorning.Formatters;
using WebApiMorning.Middlewares;
using WebApiMorning.Repositories.Abstract;
using WebApiMorning.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
   // options.OutputFormatters.Insert(0, new VCardOutputFormatter()); 
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<StudentDbContext>(opt =>
{
    opt.UseSqlServer(conn);
});

var app = builder.Build();



app.UseMiddleware<GlobalErrorHandlerMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomAuthMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
