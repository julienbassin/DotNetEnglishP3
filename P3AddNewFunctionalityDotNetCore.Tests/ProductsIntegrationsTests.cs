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
        
        public void SetupDatabase()
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
        }

        [Fact]
        public void Test_Return_All_Products()
        {
            
            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkSqlServer()
                                    .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<P3Referential>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

            var _context = new P3Referential(builder.Options);
            //Arrange
            var productService = new ProductRepository(_context);
            var results = productService.GetAllProducts();
            //Assert
            Assert.NotEmpty(results.ToList());
        }

        [Fact]
        public void Test_Add_Products_In_Database()
        {
            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkSqlServer()
                                    .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<P3Referential>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

           var  _context = new P3Referential(builder.Options);
            _context.Database.Migrate();

            _context = new P3Referential(builder.Options);
            var product = new Product { Name = "Playstation 4" };
            var productService = new ProductRepository(_context);
            productService.SaveProduct(product);
            
        }

        [Fact]
        public void Test_Delete_Products_In_Database()
        {
            
            //Arrange

            //var productService = new ProductService(_productService.Object, _languageService.Object);
            //Assert
            //productService;
        }

        
        public void CleanupDatabase()
        {
            //clean all data 
        }
    }

    
}
