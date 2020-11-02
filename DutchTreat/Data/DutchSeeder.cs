using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext context, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            this.context = context;
            this.hosting = hosting;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            context.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("ahcruzado@hotmail.com");
            if (user== null)
            {
                user = new StoreUser()
                {
                    FirstName = "Alejandro",
                    LastName = "Cruzado",
                    Email = "ahcruzado@hotmail.com",
                    UserName = "ahcruzado@hotmail.com"
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (result!= IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

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
                    order.User = user;
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
