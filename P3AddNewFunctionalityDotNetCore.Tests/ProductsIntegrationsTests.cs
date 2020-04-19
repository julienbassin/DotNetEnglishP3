using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductsIntegrationsTests
    {
        
        public P3Referential SetupDatabase()
        {
            //Act
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseInMemoryDatabase(databaseName: "P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a")
            .Options;

            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkSqlServer()
                                    .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<P3Referential>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);
            
            var _context = new P3Referential(builder.Options);
            _context.Database.Migrate();
            return _context;
        }

        [Fact]
        public void Test_Return_All_Products()
        {

            var _context = SetupDatabase();

            //var _context = new P3Referential(builder.Options);
            //Arrange
            var productService = new ProductRepository(_context);
            var results = productService.GetAllProducts();
            //Assert
            Assert.NotEmpty(results.ToList());
        }

        [Fact]
        public void Test_Add_Products_In_Database()
        {
            var _context = SetupDatabase();
             var product = new Product 
             { 
                 Name = "Playstation 4",
                 Description = "Playstation 4 has a great catalog of games",
                 Details = "Playstation 4 has could be played online",
                 Price = 499.0,
                 Quantity = 10
             };
            var productService = new ProductRepository(_context);
            productService.SaveProduct(product);            
        }

        [Fact]
        public void Test_Delete_Products_In_Database()
        {
            //Arrange
            var _context = SetupDatabase();
            var productService = new ProductRepository(_context);
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
        public void Test_Product_Controller()
        {

        }

        public void CleanupDatabase()
        {
            //clean all data 
        }
    }    
}
