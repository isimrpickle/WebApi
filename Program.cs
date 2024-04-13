using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using testapi.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // για την υποστήριξη controllers
builder.Services.AddDbContext<MyDbContext>(options=>{                //εισαγωγή του database
options.UseSqlServer(builder.Configuration.GetConnectionString("constring")); //το constring αρχικοποιείται στο appsettings.json
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // για τη σωστή λειτουργία controllers
app.Run();
