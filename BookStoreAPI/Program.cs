using BookStoreAPI.Data;
using BookStoreAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//try /fix command to fix the connection string error
builder.Services.AddDbContext<BookStoreDbContext>(options =>
{
    options.UseSqlite("Data data=bookstore.db"); 
});
builder.Services.AddScoped<BookStoreDataService>();

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
