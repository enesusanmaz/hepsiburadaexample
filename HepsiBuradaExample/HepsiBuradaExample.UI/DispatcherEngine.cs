using HepsiBuradaExample.Services.Abstract;
using HepsiBuradaExample.Services.Dtos.Campaign;
using HepsiBuradaExample.Services.Dtos.Order;
using HepsiBuradaExample.Services.Dtos.Product;
using HepsiBuradaExample.UI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;


namespace HepsiBuradaExample.UI
{
    public class DispatcherEngine
    {

        #region Delegate Dictionary
        public static Dictionary<string, Delegate> _CommandSet;

        public static Dictionary<string, Delegate> CommandSet
        {
            get
            {
                if (_CommandSet == null)
                {
                    _CommandSet = new Dictionary<string, Delegate>();

                    //Product
                    _CommandSet.Add("create_product", new DelegateCreateProduct(DispatcherEngine.CreateProduct));
                    _CommandSet.Add("get_product_info", new DelegateGetProductInfo(DispatcherEngine.GetProductInfo));

                    //Order
                    _CommandSet.Add("create_order", new DelegateCreateOrder(DispatcherEngine.CreateOrder));

                    //Campaign
                    _CommandSet.Add("create_campaign", new DelegateCreateCampaign(DispatcherEngine.CreateCampaign));
                    _CommandSet.Add("get_campaign_info", new DelegateGetCampaignInfo(DispatcherEngine.GetCampaignInfo));

                    //Application 
                    _CommandSet.Add("help_all", new DelegateGetHelpAll(DispatcherEngine.GetHelpAll));
                    _CommandSet.Add("help", new DelegateGetHelpByCommand(DispatcherEngine.GetHelpByCommand));
                    _CommandSet.Add("clear", new DelegateClear(DispatcherEngine.Clear));
                    _CommandSet.Add("exit", new DelegateExit(DispatcherEngine.Exit));
                    _CommandSet.Add("start_time", new DelegateStartTime(DispatcherEngine.StartTime));
                    _CommandSet.Add("end_time", new DelegateEndTime(DispatcherEngine.EndTime));
                }
                return _CommandSet;
            }
        }
        #endregion



        #region Product Delegates
        public delegate InfoModel DelegateCreateProduct(string productCode, string productName, decimal unitPrice, int unitInStock);
        public delegate InfoModel DelegateGetProductInfo(string productCode);
        #endregion

        #region Order Delegates
        public delegate InfoModel DelegateCreateOrder(string productCode, int quantity);
        #endregion

        #region Campaign Delegates
        public delegate InfoModel DelegateCreateCampaign(string campaignName, string productCode, decimal discountPercent, int duration, decimal limit, int targetSalesCount);
        public delegate InfoModel DelegateGetCampaignInfo(string campaignName);
        #endregion

        #region Application Delegates
        public delegate InfoModel DelegateGetHelpAll();
        public delegate InfoModel DelegateGetHelpByCommand(string command);
        public delegate void DelegateClear();
        public delegate InfoModel DelegateExit();
        public delegate void DelegateStartTime();
        public delegate void DelegateEndTime();
        #endregion




        #region Product Methods
        public static InfoModel CreateProduct(string productCode, string productName, decimal unitPrice, int unitInStock)
        {
            var model = new InfoModel();
            var productDTO = new ProductDTO 
            {
                ProductCode = productCode,
                ProductName = productName,
                UnitPrice = unitPrice,
                UnitsInStock = unitInStock
            };
            
            var productService = Startup.ServiceProvider.GetService<IProductService>();
            var serviceResult = productService.CreateProduct(productDTO);

            if (serviceResult.IsSucessed)
                model.Message = $"Product is created; Product Code : {productDTO.ProductCode} , Product Name : {productDTO.ProductName} Price : {productDTO.UnitPrice}, Stock : {productDTO.UnitsInStock}";           
            else
                model.Message = serviceResult.ServiceMessage;
            
            return model;
        }

        public static InfoModel GetProductInfo(string productCode)
        {
            var model = new InfoModel();
            var productService = Startup.ServiceProvider.GetService<IProductService>();
            var serviceResult = productService.GetProductByCode(productCode);

            if (serviceResult.IsSucessed)
                model.Message = $"Product {serviceResult.ProductCode} info; price {serviceResult.UnitPrice}, stock {serviceResult.UnitsInStock}";
            else
                model.Message = serviceResult.ServiceMessage;
            
            return model;
        }

        #endregion

        #region Order Methods

        public static InfoModel CreateOrder(string productCode, int quantity)
        {
            var model = new InfoModel();
            var orderDTO = new OrderDTO 
            {
                ProductCode = productCode,
                Quantity = quantity,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            
            var orderService = Startup.ServiceProvider.GetService<IOrderService>();
            var serviceResult = orderService.CreateOrder(orderDTO);

            if (serviceResult.IsSucessed)
                model.Message = $"Order is created; Product Code : {serviceResult.ProductCode} , Quantity : {serviceResult.Quantity}";
            else
                model.Message = serviceResult.ServiceMessage;

            return model;
        }

        #endregion

        #region Campaign Methods
        public static InfoModel CreateCampaign(string campaignName, string productCode, decimal discountPercent, int duration, decimal limit, int targetSalesCount)
        {
            var model = new InfoModel();

            var campaignDTO = new CampaignDTO
            {
                ProductCode = productCode,
                Limit = limit,
                CampaignName = campaignName,
                Duration = duration,
                DiscountPercent = discountPercent,
                TargetSalesCount = targetSalesCount
            };

            var campaignService = Startup.ServiceProvider.GetService<ICampaignService>();
            var serviceResult = campaignService.CreateCampaign(campaignDTO);

            if (serviceResult.IsSucessed)
                model.Message = $"Campaign is created; name {serviceResult.CampaignName}, product {serviceResult.ProductCode}, begin date {serviceResult.BeginDate},end date {serviceResult.EndDate} limit {serviceResult.Limit}, target sales count {serviceResult.TargetSalesCount} ";
            else
                model.Message = serviceResult.ServiceMessage;

            return model;
        }

        public static InfoModel GetCampaignInfo(string campaignName)
        {
            var model = new InfoModel();
            var campaignService = Startup.ServiceProvider.GetService<ICampaignService>();
            var serviceResult = campaignService.GetCampaignByName(campaignName);

            if (serviceResult.IsSucessed)
                model.Message = $"Campaign {serviceResult.CampaignName} info; Status {serviceResult.Status}, Target Sales {serviceResult.TargetSalesCount}, Total Sales 50, Average Item Price 100 ";
            else
                model.Message = serviceResult.ServiceMessage;

            return model;
        }
        #endregion

        #region Application Methods

        public static Dictionary<string, string> _HelpMenu;

        public static Dictionary<string, string> HelpMenu
        {
            get
            {
                if (_HelpMenu == null)
                {
                    _HelpMenu = new Dictionary<string, string>();

                    _HelpMenu.Add("create_product", "create_product PRODUCTCODE PRODUCTNAME PRICE STOCK|Creates product in your system with given product information.");
                    _HelpMenu.Add("get_product_info", "get_product_info PRODUCTCODE|Prints product information for given product code.");
                    _HelpMenu.Add("create_order", "create_order PRODUCTCODE QUANTITY|Creates order in your system with given information.");
                    _HelpMenu.Add("create_campaign", "create_campaign NAME PRODUCTCODE DISCOUNTPERCENT DURATION PMLIMIT TARGETSALESCOUNT|Creates campaign in your system with giveninformation");
                    _HelpMenu.Add("get_campaign_info", "get_campaign_info NAME|Prints campaign information for given campaign name");
                    _HelpMenu.Add("increase_time", "increase_time HOUR|Increases time in your system.");

                }
                return _HelpMenu;
            }
        }

        public static InfoModel GetHelpAll()
        {
            string helpInfoMenu = string.Empty;
            helpInfoMenu += Environment.NewLine;
            foreach (var item in HelpMenu)
            {
                var values = item.Value.Split("|");
                helpInfoMenu += "Command Name : " + item.Key + Environment.NewLine + "How to use : " + values[0] + Environment.NewLine + "What it does : " + values[1] + Environment.NewLine + Environment.NewLine;
            }
            var helpInfoModel = new InfoModel();
            helpInfoModel.Message = helpInfoMenu;
            return helpInfoModel;
        }

        public static InfoModel GetHelpByCommand(string command)
        {
            string helpInfoMenu = string.Empty;
            string item = string.Empty;

            HelpMenu.TryGetValue(command, out item);

            if (!string.IsNullOrEmpty(item))
            {
                var values = item.Split("|");
                helpInfoMenu += "Command Name : " + command + Environment.NewLine + "How to use : " + values[0] + Environment.NewLine + "What it does : " + values[1];
            }
            else
                helpInfoMenu += "There is no such command.";

            var helpInfoModel = new InfoModel();
            helpInfoModel.Message = helpInfoMenu;
            return helpInfoModel;
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static InfoModel Exit()
        {
            var model = new InfoModel();
            model.Message = "Good Bye :)";
            return model;
        }
        public static void StartTime()
        {
            var hostedService = Startup.ServiceProvider.GetService<IHostedService>();
            hostedService.StartAsync(new System.Threading.CancellationToken());
        }

        public static void EndTime()
        {
            var hostedService = Startup.ServiceProvider.GetService<IHostedService>();
            hostedService.StopAsync(new System.Threading.CancellationToken());
        }

        #endregion

    }
}
