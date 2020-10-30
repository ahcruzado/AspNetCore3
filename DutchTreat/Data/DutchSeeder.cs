using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IHostingEnvironment hosting;

        public DutchSeeder(DutchContext context, IHostingEnvironment hosting)
        {
            this.context = context;
            this.hosting = hosting;
        }

        public void Seed()
        {
            context.Database.EnsureCreated();
            if(!context.Products.Any())
            {
                //nedd to create sample data
                var filePath = Path.Combine(hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var productos = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                context.Products.AddRange(productos);

                var order = context.Orders.Where(o => o.Id == 1).FirstOrDefault();

                if (order != null)
                {
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product= productos.First(),
                            Quantity=5,
                            UnitPrice=productos.First().Price
                        }
                    };
                }
                context.SaveChanges();
            }
        }
    }
}
