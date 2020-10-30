using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{

    [Route("/api/orders/{orderid}/items")]
    public class OrderItemsController:Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;

        public OrderItemsController(IDutchRepository repository,
                                ILogger<OrdersController> logger,
                                IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = repository.GetOrderById(orderId);
            if (order==null)
            {
                return NotFound();
            }
            else
            {
                var vm=mapper.Map<IEnumerable<OrderItem>,IEnumerable< OrderItemViewModel>>(order.Items);
                return Ok(vm);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = repository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (item == null)
                {
                    return NotFound(); 
                }
                else
                {
                    var vm = mapper.Map<OrderItem, OrderItemViewModel>(item);
                    return Ok(vm);
                }
            }
        }

    }
}
