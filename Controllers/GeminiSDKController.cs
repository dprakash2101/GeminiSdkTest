using GeminiSharp.Client;
using GeminiSharp.API;
using Microsoft.AspNetCore.Mvc;

namespace geminisdktest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiSDKController : ControllerBase
    {
        public class GenerateTextRequest
        {
            public string Prompt { get; set; } = string.Empty;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateText(
            [FromBody] GenerateTextRequest request,
            [FromHeader(Name = "GeminiApiKey")] string apiKey,
            [FromHeader(Name = "Gemini-Model")] string? model)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest(new { error = "Prompt cannot be empty." });

            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest(new { error = "API key is required." });

            model ??= "gemini-1.5-flash"; // Default model

            using var httpClient = new HttpClient();
            var geminiClient = new GeminiClient(httpClient, apiKey);

            try
            {
                var result = await geminiClient.GenerateContentAsync(model, request.Prompt);
                return Ok(new { Response = result });
            }
            catch (GeminiApiException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
            }
        }
    }
}
