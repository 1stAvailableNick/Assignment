using Assignment.Controllers;
using Assignment.Converters;
using Assignment.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICachedStorage, CachedStorage>();
builder.Services.AddTransient<IStorage, InMemoryStorage>();
builder.Services.AddKeyedSingleton<IConverter, XmlConverter>(DocumentsController.XmlCode);
builder.Services.AddKeyedSingleton<IConverter, MessagePackConverter>(DocumentsController.MessagePackCode);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
