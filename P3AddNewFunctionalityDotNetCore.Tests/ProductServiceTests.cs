using Xunit;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests 
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void Test_Products_NameIsMissing()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var errorNameMessage = new LocalizedString("MissingName", "Please enter a name");

            mockLocalizer.Setup(ml => ml["MissingName"]).Returns(errorNameMessage);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);
            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = null,
                Price = "10",
                Description = "ABC",
                Details = "test",
                Stock = "10"
            };

            var result = service.CheckProductModelErrors(newProduct);
            //Assert
            Assert.Contains("Please enter a name", result); //result is null, type => list

        }

        [Fact]
        public void Test_Products_StockIsMissing()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var errorStockMessage = new LocalizedString("MissingStock", "Please enter a stock value");
            mockLocalizer.Setup(ml => ml["MissingStock"]).Returns(errorStockMessage);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object) ;
            
            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = "10",
                Description = "ABC",
                Details = "test",
                Stock = null
            };

            var result  = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("Please enter a stock value", result);

        }

        [Fact]
        public void Test_Products_PriceIsMissing()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var priceIsMissing = new LocalizedString("MissingPrice", "Please enter a price value");
            mockLocalizer.Setup(ml => ml["MissingPrice"]).Returns(priceIsMissing);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);

            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = null,
                Description = "ABC",
                Details = "test",
                Stock = "10"
            };

            var result = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("Please enter a price value", result);

        }

        [Fact]
        public void Test_Products_PriceIsNotANumber()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var priceIsNotANumber = new LocalizedString("PriceNotANumber", "Please enter a price value");
            mockLocalizer.Setup(ml => ml["PriceNotANumber"]).Returns(priceIsNotANumber);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);

            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = "ABC",
                Description = "ABC",
                Details = "test",
                Stock = "10"
            };

            var result = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("Please enter a price value", result);

        }

        [Fact]
        public void Test_Products_PriceNotGreaterThanZero()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var priceNotGreaterThanZero = new LocalizedString("PriceNotGreaterThanZero", "The price value must be a number greater than zero");
            mockLocalizer.Setup(ml => ml["PriceNotGreaterThanZero"]).Returns(priceNotGreaterThanZero);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);

            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = "0",
                Description = "ABC",
                Details = "test",
                Stock = "10"
            };

            var result = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("The price value must be a number greater than zero", result);

        }

        [Fact]
        public void Test_Products_StockNotAnInteger()
        {
            //Act
            //Mock all interfaces to use Product Service class

            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var stockNotAnInteger = new LocalizedString("StockNotAnInteger", "Please enter a stock value");
            mockLocalizer.Setup(ml => ml["StockNotAnInteger"]).Returns(stockNotAnInteger);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);

            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = "10",
                Description = "ABC",
                Details = "test",
                Stock = "ABC"
            };

            var result = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("Please enter a stock value", result);
        }

        [Fact]
        public void Test_Products_QuantityNotGreaterThanZero()
        {
            //Act
            //Mock all interfaces to use Product Service class
            // maybe refactor all these mocks method for example
            Mock<ICart> mockCart = new Mock<ICart>();
            Mock<IOrderRepository> mockOrder = new Mock<IOrderRepository>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            Mock<IStringLocalizer<ProductService>> mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            var quantityNotGreaterThanZero = new LocalizedString("StockNotGreaterThanZero", "The stock value must be a number greater than zero");
            mockLocalizer.Setup(ml => ml["StockNotGreaterThanZero"]).Returns(quantityNotGreaterThanZero);
            var service = new ProductService(mockCart.Object, mockRepository.Object, mockOrder.Object, mockLocalizer.Object);

            //Arrange
            var newProduct = new ProductViewModel()
            {
                Name = "ABC",
                Price = "10",
                Description = "ABC",
                Details = "test",
                Stock = "0"
            };

            var result = service.CheckProductModelErrors(newProduct);

            //Assert
            Assert.Contains("The stock value must be a number greater than zero", result);
        }
    }
}