using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Application.AI.Agents;
using Northwind.Application.Data;
using Northwind.Domain.Orders;
using Northwind.Infrastructure.AI.Agents.Ollama;
using Northwind.Infrastructure.AI.Tools;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Orders;

namespace Northwind.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Northwind")));

            services.AddScoped<IApplicationDbContext>(
                provider =>
                    provider.GetRequiredService<NorthwindDbContext>());

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<OrderTools>();
            services.AddScoped<BookTools>();

            services.AddScoped<OllamaAgentFactory>();
            services.AddScoped<ICustomerSupportAgent, OllamaCustomerSupportAgent>();

            return services;
        }
    }
}
