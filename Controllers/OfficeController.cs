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
    
    public class OfficeController : Controller
    {
        private StoreContext _context;
 
        public OfficeController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Table")]
        public IActionResult ToolTable()
        {
            List<Products> allItems =_context.products.ToList();
            ViewBag.ProductTable = allItems;
            return View();        
        }

        //C.R.U.D FOR INVENTORY MANAGEMENT
        // [Authorize(Policy = "AdminPrivledges")]
        [HttpGet]
        [Route("Inventory")]
        public IActionResult Inventory()
        {
            return View();
        }
        
        // VALIDATE PRODUCT
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
        public IActionResult ValidateEdit(int id, EditProduct edit)
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




        // VIEW ALL ORDERS
        [HttpGet]
        [Route("Orders")]
        public IActionResult Orders()
        {
            return View();
        }   

        // VIEW SPECIFIC ORDER WITH CUSTOMER INFO ATTACHED
        [HttpGet]
        [Route("Orders/{id}")]
        public IActionResult Orderlist()
        {
            return View();
        }

        
    }
}