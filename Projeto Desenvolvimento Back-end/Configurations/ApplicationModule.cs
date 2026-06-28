using Infrastructure.Context;
using Infrastructure.Repositories;
using Domain.Repositories;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Services;
using Core.Settings;


namespace Projeto_Desenvolvimento_Back_end.Configurations
{
    public static class ApplicationModule
    {
        public static IServiceCollection ConfigureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServices();
            services.ConfigureRepositories();
            services.ConfigureSettings(configuration);
            return services;
        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnidadesService, UnidadesService>();
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
            services.AddScoped<IMovimentoEstoqueRepository, MovimentoEstoqueRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IUnidadeProdutoRepository, UnidadeProdutoRepository>();
            services.AddScoped<IUnidadeRepository, UnidadeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }

        public static void ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("Connection"));
                }, ServiceLifetime.Scoped, ServiceLifetime.Scoped
            );
        }
    }
}
