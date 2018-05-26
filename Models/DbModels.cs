using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models //Where all of our models are kept
{
    public class Products
    {
        [Key]
        public int product_id { get; set; } //PRIMARY KEY
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
        public string image_url { get; set;}
        
    }

    public class User
    {

        [Key]
        public int user_id { get; set; } //Primary key
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int phone { get; set; }

        public string password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }  
    }

    public class Cart 
    {
        [Key]
        public int cart_id { get; set; } //PRIMARY KEY
        public int user_id { get; set; } //FOREIGN KEY
        public sbyte is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //Ef core
        public List<Cart_Items> items_in_cart { get; set; }


    }

    public class Cart_Items 
    {
        [Key]
        public int item_id { get; set; } //Primary key
        public int product_id { get; set; } //Foreign key
        public int cart_id { get; set; } //Foreign key
        public int order_id { get; set; }
        public int quantity { get; set; }
        public sbyte is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //EF Core
        public Products all_items { get; set; }
        public Cart user_cart { get; set; }

    }

    public class Orders
    {
        [Key]
        public int order_id { get; set; } //PRIMARY KEY
        public int user_id { get; set; } //FORIEGN KEY
        public int address_id { get; set; } //FOREIGN KEY
        public float subtotal { get; set; }
        public float tax { get; set; }
        public float total_billed { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //Accessing properties
        public List<Cart_Items> ordered_items { get; set; }
        public User customer { get; set; }
        public Addresses customer_address { get; set; }
    }

    public class Addresses
    {
        [Key]
        public int address_id { get; set; } //Primary Key
        public int user_id { get; set; } //Foreign Key

        public string address_line_one { get; set; }
        public string address_line_two { get; set; }

        public string city { get; set; }

        public string state_or_province { get; set; }
        public int zip_or_postal { get; set; }
        public string country { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public User corresponding_user { get; set; }

    }

    public class Reviews
    {
        [Key]
        public int review_id { get; set; }
        public int product_id { get; set; } //FOREIGN KEY
        public int user_id { get; set; } //FOREIGN KEY
        public int star_rating { get; set; }
        public string review { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        

        //RELATIONSHIP(S) -- ONE TO MANY WITH CUSTOMERS
        public User reviewer { get; set; }
    }

    
}