using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Collections.Generic;
using System.Net;

namespace RestAPI
{
    public class ProductsApi
    {
        private readonly AppDbContext _ctx;


        public ProductsApi(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [FunctionName("GetAllProducts")]

        

        public async Task<IActionResult> GetAllProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"Geting All Products ");
            var products = await _ctx.Products.ToListAsync();
            return new OkObjectResult(products);
        }

        [FunctionName("CreateProduct")]

        
        public async Task<IActionResult> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new product");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);
            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            return new CreatedResult("/products", product);
        }

    }
}
