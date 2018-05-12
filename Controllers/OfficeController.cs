using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;

namespace RobotJester.Controllers
{
    /*
    This is the OfficeController, here is where someone with admin privledges will be 
    redirected upon login. This is where you can manage the store settings like adding 
    more items looking at orders and so on.
     */
    
    public class OfficeController : Controller
    {
        private StoreContext _context;
 
        public OfficeController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Admin/Toolbox")]
        public IActionResult ToolTable()
        {
            return View();        
        }

        //C.R.U.D For inventory management
        // [Authorize(Policy = "AdminPrivledges")]
        [HttpGet]
        [Route("Inventory")]
        public IActionResult Inventory()
        {
            return View();
        }
        
        // Validate product
        [HttpPost]
        [Route("Inventory")]
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
                    z_dimension = newProduct.z_dimension,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };
                _context.products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Inventory");
            }
            return View("Inventory", newProduct);
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit()
        {
            var all = _context.products.ToList();
            ViewBag.Products = all;   
            return View();
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult EditItem(int id)
        {
            Products edit = _context.products.SingleOrDefault(item => item.product_id == id);
            return View(edit);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult ValidateEdit(int id, ViewProduct edit)
        {
            if(ModelState.IsValid)
            {
                Products current_product = _context.products.SingleOrDefault(p => p.product_id==id);
                {
                    current_product.name = edit.name;
                    current_product.price = edit.price; 
                    current_product.description = edit.description;
                    current_product.instock_quantity = edit.instock_quantity;
                    current_product.weight = edit.weight;
                    current_product.x_dimension = edit.x_dimension;
                    current_product.y_dimension = edit.y_dimension;
                    current_product.z_dimension = edit.z_dimension;
                    current_product.updated_at = DateTime.Now;
                    _context.SaveChanges();
                };
                return RedirectToAction("Edit");
            }
            return View("Inventory", edit);
        }

        //Delete a specific item
        [HttpGet]
        [Route("Delete/Product/{id}")]
        public IActionResult DeleteItem()
        {
            /*
            TODO: This method will delete a specific product. Simple enough, 
            however, there needs to be a foreach loop which removes all "cart_item"
            records that have matching product id's so that a user cannot purchase an item
            that no longer exists.
             */


            //This viewdata will be returned and rendered in a user's cart when they login.
            ViewData["Removed"] = "We removed item(s) from your cart that no longer exist in our inventory";

            return null;
        }


        //C.R.U.D For orders
        [HttpGet]
        [Route("Orders/OrderList")]
        public IActionResult Orders()
        {
            List<Orders> order_list = _context.orders.ToList();
            return View(order_list);
        }   

        // View specific order with a customer
        [HttpGet]
        [Route("Orders/Show/{id}")]
        public IActionResult Orderlist(int id)
        {
            return View();
        }

        //C.R.U.D For user's
        [HttpGet]
        [Route("Users")]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        [Route("Users/Show/{id}")]
        public IActionResult UserList(int id)
        {
            return null;
        }




        
    }
}