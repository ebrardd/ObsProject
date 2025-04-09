using Microsoft.EntityFrameworkCore;
using ObsBackend.Data;
using ObsBackend.Service;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);


// Swagger ekle
builder.Services.AddScoped<IAuthService, AuthService>();
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