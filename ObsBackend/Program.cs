using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

// Swagger ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger arayüzü sadece development ortamında çalışır
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // ← Burada Swagger UI aktif olmalı
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();