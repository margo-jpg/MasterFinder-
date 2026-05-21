using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MasterFinder.Domain.Interfaces;
using MasterFinder.Infrastructure.EntityFramework;
using MasterFinder.Infrastructure.Repositories;

namespace MasterFinder.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Добавляем DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Регистрируем репозитории
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<IExecutorRepository, EfExecutorRepository>();
            services.AddScoped<IOrderRepository, EfOrderRepository>();

            return services;
        }
    }
}
