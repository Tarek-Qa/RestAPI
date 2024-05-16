using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;

namespace RestAPI
{
    public class ChatFunction
    {
        private readonly OpenAIService _openAIService;

        public ChatFunction(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [FunctionName("GetAIResponse")]

        [OpenApiOperation(operationId: "GetAIResponse", tags: new[] { "Chat With AI ask what ever you want and wait for response from AI " })]
        [OpenApiParameter(name: "prompt", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The prompt to send to OpenAI")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The AI response")]

        public async Task<IActionResult> GetAIResponse(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chat/{prompt}")] HttpRequest req, string prompt, ILogger log)
        {
            log.LogInformation("Received AI request.");
            try
            {
                var response = await _openAIService.GenerateResponseAsync(prompt);
                return new OkObjectResult(response);
            }
            catch (System.Exception ex)
            {
                log.LogError($"Error calling OpenAI: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

    }
}
