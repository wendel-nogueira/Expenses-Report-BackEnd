using ExpensesReport.Expenses.API.Filters;
using ExpensesReport.Expenses.Infrastructure;
using ExpensesReport.Expenses.Application;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddInfrastructure()
    .AddApplication();


builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAtribute>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ExpensesReport.Expenses",
        Version = "v1",
        Description = "API to manage expenses and their information.",
        Contact = new OpenApiContact
        {
            Name = "Wendel Nogueira",
            Email = "wendelnogueira@unifei.edu.br",
            Url = new Uri("https://yelluh.tech/"),
        }
    });

    options.CustomSchemaIds(type => type.Name);

    var xmlFile = "ExpensesReport.Expenses.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
