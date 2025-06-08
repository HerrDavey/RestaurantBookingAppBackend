using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Definicja polityki CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Zezwalamy na zapytania z adresów, na których dzia³a nasz Frontend
                          policy.WithOrigins("https://localhost:7035", "http://localhost:5144")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Dodanie EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run migrations automatycznie (na czas dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();
app.Run();