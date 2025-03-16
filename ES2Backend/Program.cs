using Microsoft.EntityFrameworkCore;
using ES2Backend.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Adicionar Controllers
builder.Services.AddControllers();

// ✅ Swagger (Documentação API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ES2Backend API", Version = "v1" });
});

// ✅ Configuração do DbContext com Npgsql Options e Logs Detalhados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(120); // ✅ Timeout configurado corretamente
        })
    .EnableSensitiveDataLogging() // Log de dados sensíveis (para debugging, atenção em produção!)
    .EnableDetailedErrors());    // Erros detalhados

// ✅ Injeção de Dependências
builder.Services.AddScoped<AuthService>(); // Serviço de Autenticação

// ✅ Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// ✅ Pipeline de execução

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
