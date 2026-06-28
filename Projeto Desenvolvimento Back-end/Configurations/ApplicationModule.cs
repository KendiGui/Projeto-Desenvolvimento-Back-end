using Infrastructure.Context;
using Infrastructure.Repositories;
using Domain.Repositories;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Services;
using Service.Gateways;
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
            services.AddScoped<IProdutosService, ProdutosService>();
            services.AddScoped<ICardapioService, CardapioService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IFidelidadeService, FidelidadeService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IPaymentGateway, MockPaymentGateway>();
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
            services.AddScoped<IFidelidadeContaRepository, FidelidadeContaRepository>();
            services.AddScoped<IFidelidadeHistoricoRepository, FidelidadeHistoricoRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
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
