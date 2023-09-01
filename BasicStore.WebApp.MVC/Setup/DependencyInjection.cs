using BasicStore.Catalog.Application.Services;
using BasicStore.Catalog.Data.Repositories;
using BasicStore.Catalog.Domain;
using BasicStore.Catalog.Domain.Events;
using BasicStore.Core.Bus;
using BasicStore.Sales.Application.Commands;
using BasicStore.Sales.Data;
using BasicStore.Sales.Data.Repository;
using BasicStore.Sales.Domain;
using MediatR;
using StoreDDD.Catalog.Data;

namespace BasicStore.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Catalog
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductApplicationService, ProductApplicationService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<CatalogContext>();

            services.AddScoped<INotificationHandler<ProductBellowStockEvent>, ProductEventHandler>();

            // Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<VendasContext>();

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        }
    }
}
