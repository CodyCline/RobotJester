/* NOTES: THIS CONTROLLER IS THE MAIN HUB WHERE CUSTOMERS
WILL BE LOOKING AT  */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RobotJester.Controllers
{
    public class StoreController : Controller
    {
        private StoreContext _context;
 
        public StoreController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("Products")]
        public IActionResult Products()
        {
            List<Products> allItems = _context.products.ToList();
            ViewBag.All = allItems;
            return View();
        }

        [HttpGet]
        [Route("product/{id}")]
        public IActionResult Show(int id)
        {
            Products show = _context.products.SingleOrDefault(item => item.product_id == id);
            return View(show);
        }

        [Authorize(Policy = "AdminPrivledges")]
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [Route("Validate")]
        public IActionResult Validate(ViewProduct newProduct)
        {
            if(ModelState.IsValid)
            {
                Products product = new Products
                {
                    name = newProduct.name,
                    price = newProduct.price, 
                    description = newProduct.description,
                    instock_quantity = newProduct.instock_quantity,
                    weight = newProduct.weight,
                    x_dimension = newProduct.x_dimension,
                    y_dimension = newProduct.y_dimension,
                    z_dimension = newProduct.z_dimension
                };
                _context.products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }
            return View("Create", newProduct);
        }
    }
}
