using Microsoft.EntityFrameworkCore;
using Token_Based_Authentication_Authorization.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var service = builder.Services;
// Add services to the container.

service.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();

/* CONNECTION STRING */
var connectionString = configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();



/* INITIALIZING ENTITY FRAMEWORK CORE */
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure the HTTP request pipeline.k
// Configure the HTTP request pipeline.ks
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
