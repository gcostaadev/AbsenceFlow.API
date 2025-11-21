using AbsenceFlow.API.ExceptionHandler;
using AbsenceFlow.API.Persistence;
using AbsenceFlow.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// CONFIGURAÇÃO DE SERVIÇOS
// ----------------------------

// Serviços internos
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();

// DbContext - SQL Server
var connectionString = builder.Configuration.GetConnectionString("AbsenceFlowCs");
builder.Services.AddDbContext<AbsenceFlowDbContext>(o =>
    o.UseSqlServer(connectionString));

// Exception Handler
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

// ----------------------------
// 🔥 CORS — Obrigatório para Blazor WebAssembly
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
            policy.WithOrigins("https://localhost:7286")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()
        );
});

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------
// CONFIGURAÇÃO DO PIPELINE
// ----------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de exceções
app.UseExceptionHandler();

// CORS precisa vir ANTES do MapControllers
app.UseCors("AllowBlazor");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
