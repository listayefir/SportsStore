using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            //Arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length==2);
            Assert.AreEqual(prodArray[0].Name,"P4");
            Assert.AreEqual(prodArray[1].Name, "P5");

        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            //Arrange
            HtmlHelper helper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrl = i => "Page" + i;
            //Act
            MvcHtmlString link = helper.PageLinks(pagingInfo, pageUrl);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
 link.ToString());
        }


        [TestMethod]
        public void CanSendPaginationViewModel()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            //Assert            
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void CanFilterProducts()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            ProductController controller = new ProductController(mock.Object);


            //Act
            var result = ((ProductListViewModel)controller.List("Cat2").Model).Products.ToArray();


            //Assert
            Assert.IsTrue(result.Length == 2);
            Assert.AreEqual( "P2", result[0].Name);
            Assert.AreEqual("P4",result[1].Name);
            
        }

        [TestMethod]
        public void CanCreateCategories()
        {

            //Arrange
            Mock<IProductRepository> repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            });

            NavController controller = new NavController(repo.Object);

            //Act
            var result = (IEnumerable<string>)controller.Menu().Model;

            //Assert
            var categories = result.ToArray();
            Assert.IsTrue(categories.Length == 3);
            Assert.AreEqual("Apples", categories[0]);
            Assert.AreEqual("Oranges", categories[1]);
            Assert.AreEqual("Plums", categories[2]);
        }

        //
        [TestMethod]
        public void CanReportCategory()
        {
            //Arrange
            Mock<IProductRepository> repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            });

            NavController controller = new NavController(repo.Object);

            //Act
            var result = (string)controller.Menu("Apples").ViewBag.SelectedCategory;

            //Assert           
            Assert.AreEqual("Apples", result);
           
        }

        [TestMethod]
        public void CanCountPages()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            ProductController controller = new ProductController(mock.Object);

            //Act
            var res1 = ((ProductListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            var res2 = ((ProductListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            var res3 = ((ProductListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            var res4 = ((ProductListViewModel)controller.List(null).Model).PagingInfo.TotalItems;


            //Assert
            Assert.IsTrue(res1 == 2);
            Assert.IsTrue(res2 == 2);
            Assert.IsTrue(res3 == 1);
            Assert.IsTrue(res4 == 5);
        }

    }
}
