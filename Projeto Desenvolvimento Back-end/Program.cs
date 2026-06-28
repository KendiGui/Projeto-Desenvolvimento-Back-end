using Infrastructure.Context;
using Projeto_Desenvolvimento_Back_end.Configurations;
using Projeto_Desenvolvimento_Back_end.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.ConfigureDependecies(builder.Configuration);
builder.Services.ConfigureDataContext(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Aplica migrations e popula dados iniciais (seed).
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await DatabaseSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Falha ao aplicar migrations/seed na inicialização.");
    }
}

app.UseSwagger().UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Raízes do Nordeste v1");
    options.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<AuthenticationMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
