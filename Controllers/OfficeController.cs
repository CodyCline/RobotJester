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
        [HttpGet]
        [Route("Inventory")]
        public IActionResult Inventory()
        {
            var all = _context.products.ToList();
            ViewBag.Products = all;
            return View();
        }
        
        // Validate product
        [HttpPost]
        [Route("Inventory")]
        public IActionResult Validate(ViewProduct newProduct)
        {
            var all = _context.products.ToList();
            ViewBag.Products = all;
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

        // [HttpGet]
        // [Route("Inventory/Lookup")]
        // public IActionResult EditTable() => View();

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
        public IActionResult DeleteItem(int id)
        {
            Products item_to_be_removed = _context.products.SingleOrDefault(i => i.product_id == id);
            if(item_to_be_removed == null)
            {
                return RedirectToAction("Inventory");
            }

            List<Cart_Items> remove_from_user = _context.cart_items.Where(p => p.product_id==item_to_be_removed.product_id).ToList();
            foreach(var r in remove_from_user)
            {
                //Remove the product from the cart of users who have it added to their cart
                _context.Remove(r);
                _context.SaveChanges();
            }
            _context.Remove(item_to_be_removed);
            _context.SaveChanges();
            return RedirectToAction("Inventory");
        }


        //C.R.U.D For orders
        [HttpGet]
        [Route("Orders/All")]
        public IActionResult Orders()
        {
            List<Orders> order_list = _context.orders.Include(c => c.customer).ToList();
            return View(order_list);
        }   

        // View specific order with a customer
        [HttpGet]
        [Route("Orders/Show/{id}")]
        public IActionResult ShowOrder(int id)
        {
            var specific = _context.orders.Include(a => a.customer).Include(a => a.customer_address).SingleOrDefault(o => o.order_id==id);
            List<Cart_Items> ordered_items = _context.cart_items.Include(i => i.all_items).Where(a => a.order_id==id && a.is_active==0).ToList();
            ViewBag.Order = ordered_items;
            return View(specific);
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