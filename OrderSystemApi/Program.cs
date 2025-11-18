using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderSystemApi.Services;
using OrderSystemApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Allow only the Angular dev server origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory services (ok como Singleton enquanto for apenas para testes)
builder.Services.AddSingleton<IClienteService, ClienteServiceInMemory>();
builder.Services.AddSingleton<IProdutoService, ProdutoServiceInMemory>();
builder.Services.AddSingleton<IPedidoService, PedidoServiceInMemory>();
builder.Services.AddSingleton<IUsuarioService, UsuarioServiceInMemory>();


var app = builder.Build();

// Dev-only middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Em produção você pode querer habilitar apenas o Swagger protegido ou removê-lo.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS (antes de MapControllers)
app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
