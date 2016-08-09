using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;


namespace SportsStore.UnitTests
{
    /// <summary>
    /// Summary description for CartTests
    /// </summary>
    [TestClass]
    public class CartTests
    {
       
        [TestMethod]
        public void CanAddNewLines()
        {
            //Arrange
            Product prod1 = new Product { ProductID = 1, Name = "Keds", Price = 20.50M };
            Product prod2 = new Product { ProductID = 2, Name = "Water Ball", Price = 5.00M };

            //Act
            var cart = new Cart();
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 2);
            CartLine[] result = cart.Order.ToArray();

            //Assert
            Assert.IsTrue(result.Length == 2);
            Assert.AreEqual("Keds", result[0].Product.Name);
            Assert.AreEqual("Water Ball", result[1].Product.Name);            
            Assert.IsTrue(result[0].Quantity == 1);
            Assert.IsTrue(result[1].Quantity == 2);
        }

        [TestMethod]
        public void CanAddQuantity()
        {
            //Arrange
            Product prod1 = new Product { ProductID = 1, Name = "Keds", Price = 20.50M };
            Product prod2 = new Product { ProductID = 2, Name = "Water Ball", Price = 5.00M };

            //Act
            var cart = new Cart();
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 2);
            cart.AddItem(prod1, 2);
            CartLine[] result = cart.Order.ToArray();

            //Assert
            Assert.IsTrue(result.Length == 2);
            Assert.AreEqual(3, result[0].Quantity);
        }

        [TestMethod]
        public void CanRemoveLine()
        {

            //Arrange
            Product prod1 = new Product { ProductID = 1, Name = "Keds", Price = 20.50M };
            Product prod2 = new Product { ProductID = 2, Name = "Water Ball", Price = 5.00M };
            Product prod3 = new Product { ProductID = 3, Name = "T-Shirt", Price = 10.00M };


            //Act
            var cart = new Cart();
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 2);
            cart.AddItem(prod3, 3);
            cart.RemoveLine(prod2);
            CartLine[] result = cart.Order.OrderBy(p=>p.Product.ProductID).ToArray();

            //Assert
            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result.Where(p => p.Product == prod2).Count() == 0);
        }

        [TestMethod]
        public void CanCalculateCartTotal()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            // Arrange - create a new cart
            Cart target = new Cart();
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.TotalPrice();
            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void CanClearOrder()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            // Arrange - create a new cart
            Cart target = new Cart();
            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            //Act
            target.Clear();

            //Assert
            Assert.IsTrue(target.Order.Count() == 0);
        }

       
    }
}
