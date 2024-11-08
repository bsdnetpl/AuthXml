using AuthXml.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenerateTokenService, GenerateTokenService>(); // Dodanie serwisu GenerateTokenService
builder.Services.AddScoped<IRSAEncryptorService, RSAEncryptorService>(); // Dodanie serwisu RSAEncryptorService
builder.Services.AddScoped<IGenerateUnixTimestampService, GenerateUnixTimestampService>(); // Dodanie serwisu GenerateUnixTimestampService


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "docs"; // Swagger UI b�dzie dost�pny pod /docs
    });
    }
app.MapGet("/", context =>
{
    context.Response.Redirect("/docs");
    return Task.CompletedTask;
});
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
