using ArtHouse.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ArtContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ArtContext")));

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.DefaultIgnoreCondition
            = JsonIgnoreCondition.WhenWritingDefault;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//To prepare the database and seed data.  Can comment this out some of the time.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    ArtInitializer.Initialize(serviceProvider: services, DeleteDatabase: false,
        UseMigrations: true, SeedSampleData: true);
}

app.Run();
