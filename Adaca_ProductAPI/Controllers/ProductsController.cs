using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Adaca_ProductWebApp.Models;

namespace Adaca_ProductWebApp.Controllers
{
    public class ProductsController : Controller
    {
        // HttpClient to make API requests
        private HttpClient client = new HttpClient();
        //ILogger for logging
        private readonly ILogger<ProductsController> _logger;
        public string urlEndpoint = "https://localhost:7075/api/products";

        // Constructor initializes common settings for HttpClient
        public ProductsController(ILogger<ProductsController> logger)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
        }

        // Display list of products
        public async Task<IActionResult> Index()
        {
            try
            {
                var url = urlEndpoint;
                List<Product> products = new List<Product>();

                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(data);
                }

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index action: {ex.Message}");
                return View("Error");
            }
        }

        // Display details of a specific product
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var url = $"{urlEndpoint}/{id}";
                Product product = new Product();

                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(data);
                }

                if (id == null || product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Details action: {ex.Message}");
                return View("Error");
            }
        }

        // Display view for creating a new product
        public IActionResult Create()
        {
            return View();
        }

        // Create a new product using data from the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product newProduct)
        {
            try
            {
                var url = urlEndpoint;
                Product product = new Product();

                if (ModelState.IsValid)
                {
                    // Set base address for HttpClient
                    client.BaseAddress = new Uri(url);

                    // Convert newProduct to JSON and send POST request
                    var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(newProduct), System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(responseData);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError($"Error in Create action: {response.ReasonPhrase}");
                        return View("Error");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create action: {ex.Message}");
                return View("Error");
            }
        }

        // Display view for editing an existing product
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var url = $"{urlEndpoint}/{id}";
                Product product = new Product();

                // Set base address for HttpClient
                client.BaseAddress = new Uri(url);

                // Get product details for editing
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(data);
                }

                if (id == null || product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Edit action: {ex.Message}");
                return View("Error");
            }
        }


        // Update an existing product using data from the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product updatedProduct)
        {
            try
            {
                var url = $"{urlEndpoint}/{id}";
                Product product = new Product();

                if (ModelState.IsValid)
                {
                    // Set base address for HttpClient
                    client.BaseAddress = new Uri(url);

                    // Convert updatedProduct to JSON and send PUT request
                    var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updatedProduct), System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(responseData);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError($"Error in Edit action: {response.ReasonPhrase}");
                        return View("Error");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Edit action: {ex.Message}");
                return View("Error");
            }
        }

        // Display view for confirming product deletion
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var url = $"{urlEndpoint}/{id}";
                Product product = new Product();

                // Set base address for HttpClient
                client.BaseAddress = new Uri(url);

                // Get product details for deletion confirmation
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(data);
                }

                if (id == null || product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete (GET) action: {ex.Message}");
                return View("Error");
            }
        }

        // Delete an existing product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var url = $"{urlEndpoint}/{id}";

                if (ModelState.IsValid)
                {
                    // Set base address for HttpClient
                    client.BaseAddress = new Uri(url);

                    // Send DELETE request
                    HttpResponseMessage response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError($"Error in Delete (POST) action: {response.ReasonPhrase}");
                        return View("Error");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete (POST) action: {ex.Message}");
                return View("Error");
            }
        }

    }
}