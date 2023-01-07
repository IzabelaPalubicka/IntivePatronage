using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Patronage.API.Validators;
using Patronage.API.Validators.Books;
using Patronage.Application.Models.Author;
using Patronage.Application.Models.Book;
using Patronage.Application.Repositories;
using Patronage.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Patronage.Database")));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IValidator<BaseAuthorDto>, AuthorValidator>();
builder.Services.AddScoped<IValidator<CreateAuthorDto>, AuthorValidator>();

builder.Services.AddScoped<IValidator<BaseBookDto>, BookValidator>();
builder.Services.AddScoped<IValidator<CreateBookDto>, CreateBookValidator>();
builder.Services.AddScoped<IValidator<UpdateBookDto>, UpdateBookDtoValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
