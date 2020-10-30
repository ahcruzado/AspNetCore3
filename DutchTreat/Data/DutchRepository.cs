using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(Order model)
        {
            this.context.Orders.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            IEnumerable<Order> orders;

            if (includeItems)
            {
                orders= context.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();
            }
            else
            {
                orders = context.Orders.ToList();
            }

            return orders;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            logger.LogInformation("GetAllProducts was called");

            return context.Products
                    .OrderBy(p => p.Title)
                    .ToList();
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                .Where(o=> o.Id== id)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductByCategory(string category)
        {
            return context.Products
                            .Where(p => p.Category == category)
                            .ToList();
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }
    }
}
