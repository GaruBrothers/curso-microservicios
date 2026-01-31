using Microsoft.EntityFrameworkCore;
using ApiGlobal.Data;
using ApiGlobal.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ApiGlobal",
        Version = "v1"
    });
});

// 🔹 DbContext
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// 🔹 Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGlobal v1");
});

app.UseHttpsRedirection();

// 🔹 Endpoints
app.MapGet("/Adults", async (DataContext context) =>
    await context.Adults.ToListAsync()
)
.WithName("GetAdults")
.WithOpenApi();

app.MapGet("/Children", async (DataContext context) =>
    await context.Children.ToListAsync()
)
.WithName("GetChildren")
.WithOpenApi();

app.MapGet("/Adults/{id:int}", async (DataContext context, int id) =>
    await context.Adults.FindAsync(id)
)
.WithName("GetAdultById")
.WithOpenApi();

app.MapGet("/Children/{id:int}", async (DataContext context, int id) =>
    await context.Children.FindAsync(id)
)
.WithName("GetChildById")
.WithOpenApi();

app.MapPost("/Add/Adults", async (DataContext context, Adult item) =>
{
    context.Adults.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/Adults/{item.Id}", item);
})
.WithName("AddAdult")
.WithOpenApi();

app.MapPost("/Add/Children", async (DataContext context, Child item) =>
{
    context.Children.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/Children/{item.Id}", item);
})
.WithName("AddChild")
.WithOpenApi();

app.Run();
