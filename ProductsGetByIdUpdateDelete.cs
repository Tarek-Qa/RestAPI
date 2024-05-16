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
using Microsoft.OpenApi.Models;
using System.Net;

namespace RestAPI
{
    public class ProductByIdApi
    {
        private readonly AppDbContext _ctx;

        public ProductByIdApi(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [FunctionName("GetProductById")]

        public async Task<IActionResult> GetProductById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequest req, string id, ILogger log)
        {
            log.LogInformation($"Geting Product Witch Have Id {id}");
            var product = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return new NotFoundResult();

            return new OkObjectResult(product);
        }

        [FunctionName("UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id}")] HttpRequest req, string id, ILogger log)
        {
            log.LogInformation($"Update Product That have Id {id}");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);
            product.Id = id;
            _ctx.Products.Update(product);
            await _ctx.SaveChangesAsync();

            return new OkObjectResult(product);
        }

        [FunctionName("DeleteProduct")]

        
        public async Task<IActionResult> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{id}")] HttpRequest req, string id, ILogger log)
        {
            log.LogInformation($"DeletingProduct witch Have Id {id}");
            var product = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return new NotFoundResult();

            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
