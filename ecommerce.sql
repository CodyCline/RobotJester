CREATE DATABASE  IF NOT EXISTS `ecommerce` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ecommerce`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: ecommerce
-- ------------------------------------------------------
-- Server version	5.7.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `addresses`
--

DROP TABLE IF EXISTS `addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `addresses` (
  `address_id` int(11) NOT NULL AUTO_INCREMENT,
  `address_line_one` varchar(255) DEFAULT NULL,
  `address_line_two` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `state_or_province` varchar(255) DEFAULT NULL,
  `zip_or_postal` int(11) DEFAULT NULL,
  `country` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `user_id` int(11) NOT NULL,
  PRIMARY KEY (`address_id`),
  KEY `fk_addresses_users2_idx` (`user_id`),
  CONSTRAINT `fk_addresses_users2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addresses`
--

LOCK TABLES `addresses` WRITE;
/*!40000 ALTER TABLE `addresses` DISABLE KEYS */;
/*!40000 ALTER TABLE `addresses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cart_items`
--

DROP TABLE IF EXISTS `cart_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cart_items` (
  `item_id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL,
  `cart_id` int(11) NOT NULL,
  `quantity` int(11) DEFAULT NULL,
  PRIMARY KEY (`item_id`,`product_id`,`cart_id`),
  KEY `fk_products_has_carts_carts2_idx` (`cart_id`),
  KEY `fk_products_has_carts_products2_idx` (`product_id`),
  CONSTRAINT `fk_products_has_carts_carts2` FOREIGN KEY (`cart_id`) REFERENCES `carts` (`cart_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_products_has_carts_products2` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart_items`
--

LOCK TABLES `cart_items` WRITE;
/*!40000 ALTER TABLE `cart_items` DISABLE KEYS */;
INSERT INTO `cart_items` VALUES (6,1,8,1),(7,1,8,40),(8,1,9,1),(16,1,10,1),(17,1,10,1);
/*!40000 ALTER TABLE `cart_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `carts`
--

DROP TABLE IF EXISTS `carts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `carts` (
  `cart_id` int(11) NOT NULL AUTO_INCREMENT,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `user_id` int(11) NOT NULL,
  PRIMARY KEY (`cart_id`),
  KEY `fk_carts_users2_idx` (`user_id`),
  CONSTRAINT `fk_carts_users2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carts`
--

LOCK TABLES `carts` WRITE;
/*!40000 ALTER TABLE `carts` DISABLE KEYS */;
INSERT INTO `carts` VALUES (2,'2018-04-30 00:00:00','2018-04-30 00:00:00',1),(4,'2018-04-30 00:00:00','2018-04-30 00:00:00',1),(5,'2018-04-30 00:00:00','2018-04-30 00:00:00',4),(6,'2018-04-30 00:00:00','2018-04-30 00:00:00',5),(7,'2018-04-30 00:00:00','2018-04-30 00:00:00',6),(8,'2018-04-30 00:00:00','2018-04-30 00:00:00',7),(9,'2018-04-30 00:00:00','2018-04-30 00:00:00',8),(10,'2018-05-02 00:00:00','2018-05-02 00:00:00',9),(11,'2018-05-02 00:00:00','2018-05-02 00:00:00',10);
/*!40000 ALTER TABLE `carts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `orders` (
  `order_id` int(11) NOT NULL AUTO_INCREMENT,
  `quantity` int(11) DEFAULT NULL,
  `total_billed` float DEFAULT NULL,
  `tax` float DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `user_id` int(11) NOT NULL,
  `addresses` int(11) NOT NULL,
  PRIMARY KEY (`order_id`),
  KEY `fk_orders_users1_idx` (`user_id`),
  KEY `fk_orders_addresses2_idx` (`addresses`),
  CONSTRAINT `fk_orders_addresses2` FOREIGN KEY (`addresses`) REFERENCES `addresses` (`address_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_orders_users1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `products` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `price` float DEFAULT NULL,
  `description` text,
  `instock_quantity` int(11) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `x_dimension` float DEFAULT NULL,
  `y_dimension` float DEFAULT NULL,
  `z_dimension` float DEFAULT NULL,
  `image_url` text,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Test',100,'Really awesome test product that will work!',100,1,1,1,1,NULL,'2018-04-30 18:09:23','2018-04-30 18:09:23');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `reviews` (
  `product_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `star_rating` int(11) DEFAULT NULL,
  `review` text,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`product_id`,`user_id`),
  KEY `fk_products_has_users_users3_idx` (`user_id`),
  KEY `fk_products_has_users_products1_idx` (`product_id`),
  CONSTRAINT `fk_products_has_users_products1` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_products_has_users_users3` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `phone` int(11) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `privledge_level` int(11) DEFAULT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Cody','Cody','test@cody.com',0,'AQAAAAEAACcQAAAAEB7muziv2txp4+G3WuvBd+MEpUQQNn6TkrIK/UzK9i7fCKHkeBu8yjvFbh6rXWWDqQ==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(2,'Ygar','Ygar','tag@test.com',0,'AQAAAAEAACcQAAAAEARGfV9wNu6PlNrOKdXwC0Vcb84N+BQBk2yLaqNCnlRBtgdvHpauOWVHWFOpvPTNjw==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(3,'Cody','Cody','yag@ted.com',0,'AQAAAAEAACcQAAAAEDqEylCWpK2Qa5NTRqRXIGCtXh836SzVQMFA+QSX9H+PQ64s+3+DsBldZQNyzH2Ykw==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(4,'Cody','Cody','cody@yody.com',0,'AQAAAAEAACcQAAAAEIlXzBTQ41AqFliELguSJIv2Oaub1zaVCzusGiG36tInFbvHGji8PEPSztJXflAgXA==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(5,'Cody','Cody','test@comcast.net',0,'AQAAAAEAACcQAAAAEMbW8kWXv4n9XReXqcqNBJ1dFI96Fnx3MSQT0YVWUldIlCm7WTslDkoUNqStsRTBkA==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(6,'kjasdhgakjlsdg','sdgjhsadkgsadlkjg','12@12.net',0,'AQAAAAEAACcQAAAAEDMqSANald/YwGq7/ss0DJ+eP5jG7RuAe6A4fodcLEwi+Xx4NLB2QD+TrkQu8hGloQ==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(7,'casdgasd','dsgaadsgdsgdsa','afgar@test.com',0,'AQAAAAEAACcQAAAAEBBghB2gVMbVGgVGmK1wvBEzUmIFfkxk90Zzszs6Pry29jfcMu6A4EvZ6/GxNCzn3g==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(8,'sdgsdg','wsdgsdgsd','bag@bagger.net',0,'AQAAAAEAACcQAAAAEK0ZLhivdLfubnbjUi1jnprH2b3oOqDu+fz2frNnqVtrrHR9T3+KaT2goUkhtgiUBA==','2018-04-30 00:00:00','2018-04-30 00:00:00',0),(9,'Cody','Cody','cody@test.com',0,'AQAAAAEAACcQAAAAENOFgL7NRvCwNK5H2EEr/Tzzj6ovcOkn1aqYlzSPSk90bZo9d/LVoCgRu7WEkirEHA==','2018-05-02 00:00:00','2018-05-02 00:00:00',0),(10,'Cody','Cody','y@y.com',0,'AQAAAAEAACcQAAAAEA94U88p4fBHrEchtf/Qv8u/Cfktf3qTXOJRcgHEnU+L8zNt8VqZZbJaqONM6SLbVw==','2018-05-02 00:00:00','2018-05-02 00:00:00',0);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-04 10:35:12
