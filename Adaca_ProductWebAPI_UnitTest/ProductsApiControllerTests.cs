using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Adaca_ProductWebAPI.Models;
using Newtonsoft.Json;
using System.Text;

namespace Adaca_ProductWebAPI_UnitTest
{
    public class ProductsApiControllerTests
    {
        // HttpClient to make API requests
        private HttpClient client = new HttpClient();
        public string urlEndpoint = "https://localhost:7075/api/products";

        // Constructor initializes common settings for HttpClient
        public ProductsApiControllerTests()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task GetProducts_ReturnsSuccessStatusCode()
        {
            // Arrange
            var url = urlEndpoint;
            client.BaseAddress = new Uri(url);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetProductById_ReturnsSuccessStatusCode()
        {
            // Arrange
            var id = 5004;
            var url = $"{urlEndpoint}/{id}";
            client.BaseAddress = new Uri(url);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task CreateProduct_ReturnsSuccessStatusCode()
        {
            // Arrange
            var url = urlEndpoint;
            client.BaseAddress = new Uri(url);

            var newProduct = new Product
            {
                Name = "NewProduct",
                Description = "Description",
                Price = (decimal)10.5
            };

            // Act
            var response = await client.PostAsJsonAsync(url, newProduct);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task UpdateProduct_ValidData_ReturnsNoContent()
        {
            // Arrange
            var id = 5004;
            var url = $"{urlEndpoint}/{id}";
            client.BaseAddress = new Uri(url);

            
            var updatedProduct = new Product { Id = id, Name = "Updated Product Name", Description = "Updated Product Description", Price = (decimal)999 };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(updatedProduct), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var id = 6003;
            var url = $"{urlEndpoint}/{id}";
            client.BaseAddress = new Uri(url);

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }

}