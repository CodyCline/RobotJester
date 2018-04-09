/*TODO: RENAME TO OFFICE CONTROLLER THIS IS WHERE
ONLY ADMINS AND MANAGERS CAN VIEW ORDERS AND OTHER
SENSITIVE DATA */

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
        [Route("Create")]
        public IActionResult Create()
        {   
            
            return View();
        }
        
        // VALIDATE PRODUCT
        [HttpPost]
        [Route("ChkItem")]
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
                return RedirectToAction("Create");
            }
            return View("Create", newProduct);
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
        public IActionResult Show()
        {
            return View();
        }

        
    }
}