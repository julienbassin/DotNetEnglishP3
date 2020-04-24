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
        
        //public P3Referential SetupDatabase()
        //{
        //    //Act
        //    var options = new DbContextOptionsBuilder<P3Referential>()
        //    .UseInMemoryDatabase(databaseName: "P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a")
        //    .Options;           
            
        //    var context = new P3Referential(options);
        //    return context;
        //}

        [Fact]
        public void Test_Return_All_Products_In_Memory_Database()
        {
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

                var productService = new ProductRepository(context);
                productService.SaveProduct(product);
                productService.SaveProduct(product2);
                var results = productService.GetAllProducts();
                Assert.NotEmpty(results.ToList());
            }           
            //Assert
            

            //var _context = SetupDatabase();
           
            //var _context = new P3Referential(builder.Options);
            //Arrange
            //var productService = new ProductRepository(_context);
            //var results = productService.GetAllProducts();
            //Assert
            //Assert.NotEmpty(results.ToList());
        }

        [Fact]
        public void Test_Add_Products_In_Persistent_Database()
        {
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
            var productService = new ProductRepository(context);
            productService.SaveProduct(product);            
        }

        [Fact]
        public void Test_Delete_Products_In_Persistent_Database()
        {
            //Arrange
            var factory = new ConnectionFactory();
            var context = factory.CreateContextSQLServer();
            var productService = new ProductRepository(context);
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
            //test product controller
        }

        // test authentication user 

        //test authentication admin
       
    }    
}

//public void Dispose()
//{
//    if (null != this.CurrentDatabaseConnection)
//    {
//        this.CurrentDatabaseConnection.Dispose();
//        this.CurrentDatabaseConnection = null;
//    }
//}
