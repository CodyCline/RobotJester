using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models
{
    public class Products
    {
        [Key]
        public long product_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int instock_quantity { get; set; }
        public float price { get; set; }
        public float weight { get; set; }
        public float x_dimension { get; set; }
        public float y_dimension { get; set; }
        public float z_dimension { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //RELATIONSHIP(S) -- MANY TO MANY WITH CUSTOMERS AND ORDERS
        public List<Orders> Consumers { get; set; }
      
    }

    public class User
    {
        internal int user_id;

        [Key]
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int phone { get; set; }

        public string password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
        //RELATIONSHIP -- MANY TO MANY WITH ORDERS, CUSTOMERS.
        public List<Orders> Orders { get; set; }
        
        public List<Addresses> address_list { get;set;}
        public List<Reviews> reviews { get; set; }

        
    }

    public class Orders
    {
        [Key]
        public int order_id { get; set; }
        public int quantity { get; set; }
        public float invoiced_amount { get; set; }
        public float tax { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
        //RELATIONSHIP(S) -- MANY TO MANY JOINING PRODUCTS AND CUSTOMERS IN TABLE "ORDERS"
        public long customer_id { get; set; }
        public User user { get; set; }
        
        public long product_id { get; set; }
        public Products products { get; set; }

        //SEPERATE ONE-TO-ONE FOR ADDRESSES
        // public long shipping_address_id { get; set; }
        // public Addresses addresses { get; set; }
    }

    public class Addresses
    {
        [Key]
        public long address_id { get; set; }

        public string address_line_one { get; set; }
        public string address_line_two { get; set; }

        public string city { get; set; }

        public string state_or_province { get; set; }
        public int zip_or_postal { get; set; }
        public string country { get; set; }
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}

        //RELATIONSHIP(S) -- ONE-TO-ONE WITH ORDERS & ONE TO MANY WITH CUSTOMERS
        public Orders orders { get; set; }

        // public long customer_id { get; set; }
        // public Customers customers { get; set; }

    }

    public class Reviews
    {
        [Key]
        public long review_id { get; set; }
        public string review { get; set; }
        public int stars { get; set; }
        public float price { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        

        //RELATIONSHIP(S) -- ONE TO MANY WITH CUSTOMERS
        public long customer_id { get; set; }
        public User user { get; set; }
    }

    
}