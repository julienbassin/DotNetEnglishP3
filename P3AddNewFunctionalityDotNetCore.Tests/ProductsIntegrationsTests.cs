using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System;
using Xunit;
using System.Linq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Threading.Tasks;
using System.Security.Claims;
using P3AddNewFunctionalityDotNetCore.Controllers;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ConnectionFactory : IDisposable
    {

        #region IDisposable Support  
        private bool disposedValue = false; // To detect redundant calls  

        public P3Referential CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<P3Referential>()
                        .UseInMemoryDatabase(databaseName: "P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a")
                        .Options;

            var context = new P3Referential(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        public P3Referential CreateContextSQLServer()
        {
            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkSqlServer()
                                    .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<P3Referential>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

            var context = new P3Referential(builder.Options);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }


    public class ProductsIntegrationsTests
    {
        [Fact]
        public void Test_Return_All_Products_In_Memory_Database()
        {
            // Act
            var factory = new ConnectionFactory();
            using (var context = factory.CreateContextForInMemory())
            {
                var product = new Product
                {
                    Name = "Playstation 4",
                    Description = "Playstation 4 has a great catalog of games",
                    Details = "Playstation 4 has could be played online",
                    Price = 499.0,
                    Quantity = 10
                };

                var product2 = new Product
                {
                    Name = "Nintendo Switch",
                    Description = "Nintendo Switch has a great catalog of games",
                    Details = "Nintendo Switch has could be played online",
                    Price = 399.0,
                    Quantity = 100
                };

                // Arrange
                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);
                var results = productService.GetAllProducts();

                // Assert
                Assert.NotEmpty(results.ToList());
            }
        }

        [Fact]
        public void Test_Add_Products_In_Persistent_Database()
        {
            // Act
            var factory = new ConnectionFactory();
            var context = factory.CreateContextSQLServer();

            var product = new Product 
             { 
                 Name = "Playstation 4",
                 Description = "Playstation 4 has a great catalog of games",
                 Details = "Playstation 4 has could be played online",
                 Price = 499.0,
                 Quantity = 10
             };

            // Arrange
            var productService = new ProductRepository(context);
            productService.SaveProduct(product);
            var results = productService.GetAllProducts();

            // Assert
            Assert.NotNull(results.FirstOrDefault(p=>p.Id == product.Id));
        }

        [Fact]
        public void Test_Delete_Products_In_Persistent_Database()
        {
            // Act
            var factory = new ConnectionFactory();
            var context = factory.CreateContextSQLServer();
            var productService = new ProductRepository(context);
            var product = new Product
            {
                Name = "Playstation 4",
                Description = "Playstation 4 has a great catalog of games",
                Details = "Playstation 4 has could be played online",
                Price = 499.0,
                Quantity = 10
            };

            // Arrange
            productService.SaveProduct(product);
            var Products = productService.GetAllProducts().ToList();
            
            foreach (var item in Products)
            {
                productService.DeleteProduct(item.Id);
            }

            //Assert
            var Results = productService.GetAllProducts().ToList();
            Assert.Empty(Results);
        }

        [Fact]
        public void Index_Should_Return_Public_ProductController_View()
        {
            // Act
            Mock<IProductService> mockProductService = new Mock<IProductService>();
            Mock<ILanguageService> mockLanguageService = new Mock<ILanguageService>();

            // Arrange
            var productController = new ProductController(mockProductService.Object, mockLanguageService.Object);
            IActionResult ViewResult = productController.Index();

            // Assert
            Assert.IsType<ViewResult>(ViewResult);

        }

    }
}
