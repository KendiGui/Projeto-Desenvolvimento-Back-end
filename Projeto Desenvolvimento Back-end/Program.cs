using Projeto_Desenvolvimento_Back_end.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.ConfigureDependecies(builder.Configuration);
builder.Services.ConfigureDataContext(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Raízes do Nordeste v1");
    options.RoutePrefix = string.Empty;
});

app.UseMiddleware<AuthenticationMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
