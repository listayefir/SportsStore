using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index (Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                ReturnUrl=returnUrl,
                Cart=cart
            });
        }

        public RedirectToRouteResult AddToCart (Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products
                .Where(p => p.ProductID == productID)
                .FirstOrDefault();

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
           
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnUrl)
        {
            Product product = repository.Products
                .Where(p => p.ProductID == productID)
                .FirstOrDefault();

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

    public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }


       
    }
}