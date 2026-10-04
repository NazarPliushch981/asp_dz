using Microsoft.AspNetCore.Mvc;
using asp_dz.Models;

namespace asp_dz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static List<Order> orders = new List<Order>
        {
            new Order { Id = 1, Number = "ORD-001", Total = 1500.50m, CreatedAt = DateTime.Now.AddDays(-3) },
            new Order { Id = 2, Number = "ORD-002", Total = 2300.00m, CreatedAt = DateTime.Now.AddDays(-2) },
            new Order { Id = 3, Number = "ORD-003", Total = 450.75m, CreatedAt = DateTime.Now.AddDays(-1) }
        };

        [HttpGet]
        public IActionResult GetAll()
        { 
            return Ok(orders);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string number, [FromQuery] decimal? minTotal)
        {
            if (string.IsNullOrEmpty(number))
            {
                return BadRequest("number обов'язковo");
            }

            var result = orders.Where(o => o.Number.Contains(number, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minTotal.HasValue)
            {
                result = result.Where(o => o.Total >= minTotal.Value).ToList();
            }

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Order order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("not found");
            }

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] OrderDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Number) || dto.Total <= 0)
            {
                return BadRequest("Заповніть поля ");
            }

            int newId = 1;
            if (orders.Count > 0)
            {
                newId = orders.Max(o => o.Id) + 1;
            }

            Order newOrder = new Order
            {
                Id = newId,
                Number = dto.Number,
                Total = dto.Total,
                CreatedAt = DateTime.Now
            };

            orders.Add(newOrder);

            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] OrderDto dto)
        {
            Order order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            if (dto == null || string.IsNullOrEmpty(dto.Number) || dto.Total <= 0)
            {
                return BadRequest("Некоректні дані");
            }

            order.Number = dto.Number;
            order.Total = dto.Total;

            return Ok(order);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Order order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            orders.Remove(order);

            return NoContent();
        }
    }
}