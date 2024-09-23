using Lab_Cart_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Lab_Cart_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private static List<CartItem> items = new List<CartItem>()
        {
            new CartItem(){Id = 1, Product = "Vegan Steak", Price= 14.00, Quantity=1},
            new CartItem(){Id = 2, Product = "Kale", Price=3.00, Quantity=1},
            new CartItem(){Id = 3, Product = "Almond Milk", Price = 4.50, Quantity = 2},
            new CartItem(){Id = 4, Product = "Quinoa Salad", Price = 8.75, Quantity = 1},
            new CartItem(){Id = 5, Product = "Tofu Stir-fry", Price = 12.00, Quantity = 1},
            new CartItem(){Id = 6, Product = "Avocado Toast", Price = 6.50, Quantity = 1},
            new CartItem(){Id = 7, Product = "Chickpea Burger", Price = 10.00, Quantity = 2},
            new CartItem(){Id = 8, Product = "Hummus", Price = 5.00, Quantity = 3},
            new CartItem(){Id = 9, Product = "Spinach", Price = 2.50, Quantity = 1},
            new CartItem(){Id = 10, Product = "Lentil Soup", Price = 7.25, Quantity = 1},
            new CartItem(){Id = 11, Product = "Blueberries", Price = 4.00, Quantity = 1},
            new CartItem(){Id = 12, Product = "Granola Bar", Price = 1.50, Quantity = 5}
        };
        private static int nextId = 13;

        //1. & 2.
        /*curl -K 'GET' \
          'https://localhost:7238/api/CartItems/Get-All' \
          -H 'accept: */ //*'

        [HttpGet("Get-All")]
        public IActionResult GetAll()
        {
            return Ok(items);
        }
 
        [HttpGet("Sort")]
        public IActionResult GetByMaxPrice(double? Price = null, string? prefix = null, int? pageSize = null, int? page = null)
        {
            IEnumerable<CartItem> cartItems = items;
            //3.
            /*curl -K 'GET' \
            'https://localhost:7238/api/CartItems/Sort?Price=8' \
            -H 'accept: */ //*'
            if (Price != null)
            {
                cartItems = cartItems.Where(i => i.Price > Price);
            }
            //4.
            /*curl -K 'GET' \
            'https://localhost:7238/api/CartItems/Sort?prefix=st' \
            -H 'accept: */ //*'
            if (prefix != null)
            {
                cartItems = cartItems.Where(i => i.Product.Contains(prefix, StringComparison.OrdinalIgnoreCase));
            }
            //5.
            /*curl -K 'GET' \
            'https://localhost:7238/api/CartItems/Sort?pageSize=2' \
            -H 'accept: */ //*'
            if (pageSize != null)
            {
                cartItems = cartItems.Take(pageSize.Value);
            }
            //Extended
            if (page != null && pageSize != null)
            {
                cartItems = cartItems.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            //Extended
            /*curl -K 'GET' \
            'https://localhost:7238/api/CartItems/Sort?Price=8&prefix=a&pageSize=2' \
            -H 'accept: */ //*'
            List<CartItem> filteredItems = cartItems.ToList();

            if (filteredItems.Count > 0)
            {
                return Ok(cartItems);
            }
            else
            {
                return NotFound();
            }
        }

        //6. & 7
        /*curl -K 'GET' \
        'https://localhost:7238/api/CartItems/6' \
        -H 'accept: */ //*'
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CartItem i = items.FirstOrDefault(c => c.Id == id);
            //not found
            if (i == null)
            {
                //8. 
                /*curl -K 'GET' \
                'https://localhost:7238/api/CartItems/13' \
                -H 'accept: */ //*'
                return NotFound();
            }

            //match
            return Ok(i);
        }

        //9. & 10.
        /*curl -X 'POST' \
          'https://localhost:7238/api/CartItems' \
          -H 'accept: */ /* *' \
          -H 'Content-Type: application/json' \
          -d '{
          "id": 0,
          "product": "Pickled Pigs Feet",
          "price": 9.00,
          "quantity": 10
        }'*/
        [HttpPost()]
        public IActionResult AddCartItem([FromBody]CartItem newItem)
        {
            newItem.Id = nextId;
            items.Add(newItem);
            nextId++;
            return Created($"/api/CartItems/{newItem.Id}", newItem);
        }
        //11. & 12.
        /*curl -X 'PUT' \
          'https://localhost:7238/api/CartItems/3' \
          -H 'accept: */ /*' \
          -H 'Content-Type: application/json' \
          -d '{
          "id": 3,
          "product": "donuts",
          "price": 10,
          "quantity": 10
        }'*/
        [HttpPut("{id}")]
        public IActionResult UpdateCartItem(int id, [FromBody] CartItem updateItem)
        {
            if (id != updateItem.Id) { return BadRequest(); }
            if (items.Any(i => i.Id == id)==false) { return BadRequest(); }
            int index = items.FindIndex(i => i.Id == id);
            items[index] = updateItem;
            return Ok(updateItem);
        }
        //13. & 14.
        /*curl -X 'DELETE' \
        'https://localhost:7238/api/CartItems/3' \
        -H 'accept: */ //*'
        [HttpDelete("{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            int index = items.FindIndex(i=>i.Id == id);
            if (index == -1) { return NotFound("We need a real ID.");}
            else
            {
                items.RemoveAt(index);
                return NoContent();
            }
        }


    }
}
