using System.Runtime.CompilerServices;

namespace Projeto_Desenvolvimento_Back_end.Configurations
{
    public static class ApplicationModule
    {
        public static IServiceCollection ConfigureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServices();
            services.ConfigureRepositories();
            return services;
        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {

        }
    }
}
