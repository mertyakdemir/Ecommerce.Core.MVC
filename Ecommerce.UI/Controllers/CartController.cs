using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.Entities;
using Ecommerce.UI.Identity;
using Ecommerce.UI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 

namespace Ecommerce.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private ICartBusiness cartBusiness;
        private UserManager<User> userManager;
        private IOrderBusiness orderBusiness;

        public CartController(ICartBusiness _cartBusiness, UserManager<User> _userManager, IOrderBusiness _orderBusiness)
        {
            cartBusiness = _cartBusiness;
            userManager = _userManager;
            orderBusiness = _orderBusiness;
        }

        public IActionResult Index()
        {
                var cart = cartBusiness.GetCartByUserId(userManager.GetUserId(User));
            
                CartModel model  = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.ProductName,
                        Price = i.Product.ProductPrice,
                        ImageUrl = i.Product.ProductImage,
                        Quantity = i.Quantity,
                    }).ToList()
                };
             return View(model);
            
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            cartBusiness.AddToCart(userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            cartBusiness.DeleteFromCart(userManager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = cartBusiness.GetCartByUserId(userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.ProductName,
                    Price = i.Product.ProductPrice,
                    ImageUrl = i.Product.ProductImage,
                    Quantity = i.Quantity,
                }).ToList()
            };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                var cart = cartBusiness.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.ProductName,
                        Price = i.Product.ProductPrice,
                        ImageUrl = i.Product.ProductImage,
                        Quantity = i.Quantity,
                    }).ToList()
                };

                var payment = PaymenProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(model.CartModel.CartId);
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", payment.ErrorMessage);
                }
            }
            return View(model);
        }

        public IActionResult Orders()
        {
            var orders = orderBusiness.GetOrders(userManager.GetUserId(User));


            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var item in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = item.Id;
                orderModel.OrderNumber = item.OrderNumber;
                orderModel.OrderDate = item.OrderDate;
                orderModel.Phone = item.Phone;
                orderModel.FirstName = item.FirstName;
                orderModel.LastName = item.LastName;
                orderModel.Email = item.Email;
                orderModel.Address = item.Address;
                orderModel.City = item.City;
                orderModel.OrderState = item.OrderState;
                orderModel.PaymentType = item.PaymentType;
                orderModel.OrderItems = item.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ProductImage,
                }).ToList();

                orderListModel.Add(orderModel);
            }
            return View(orderListModel);
        }

        private void ClearCart(int cartId)
        {
            cartBusiness.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.Completed;
            order.PaymentType = EnumPaymentTypes.CreditCart;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.OrderNote = model.OrderNote;
            order.OrderItems = new List<Entities.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new Entities.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            orderBusiness.Create(order);

        }

        private Payment PaymenProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-J6tqsQY2c56k1skiO1Vv8qJPk6oHyXjP";
            options.SecretKey = "sandbox-W1twaY4nPYIg7rvMpTXOcHQbaD5qIPfW";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111,999999).ToString();
            request.Price = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = model.Phone;
            buyer.Email = model.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Phone";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = (item.Price * item.Quantity).ToString();
                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;

            return Payment.Create(request, options);
        }
    }
}