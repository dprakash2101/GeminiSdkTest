using GeminiSdkTest.Models;
using GeminiSharp.API;
using GeminiSharp.Client;
using GeminiSharp.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace geminisdktest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiSDKStructuredController : ControllerBase
    {
        public class GenerateStructuredRequest
        {
            public string Prompt { get; set; } = string.Empty;
        }
        [HttpGet("generate-rocketstats")]
        public async Task<IActionResult> GenerateStructuredRocketStats(
            [FromBody] GenerateStructuredRequest request,
            [FromHeader(Name = "GeminiApiKey")] string apiKey,
            [FromHeader(Name = "Gemini-Model")] string? model)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest(new { error = "Prompt cannot be empty." });

            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest(new { error = "API key is required." });

            model ??= "gemini-1.5-flash"; // Default model

            using var httpClient = new HttpClient();
            var geminiClient = new GeminiStructuredClient(httpClient, apiKey);

            try
            {
                // Generate JSON schema for structured output
                object jsonSchema = JsonSchemaHelper.GenerateSchema<RocketStats>();

                var response = await geminiClient.GenerateStructuredContentAsync(
                    model: model,
                    prompt: request.Prompt,
                    jsonSchema: jsonSchema
                );

                var jsonText = response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

                if (string.IsNullOrWhiteSpace(jsonText))
                {
                    return BadRequest(new { error = "No valid structured content received from Gemini API." });
                }

                // Deserialize the structured data into PlayerStats
                RocketStats? structuredData;
                try
                {
                    structuredData = JsonSerializer.Deserialize<RocketStats>(jsonText, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                catch (JsonException jsonEx)
                {
                    return BadRequest(new { error = "Invalid structured response format.", details = jsonEx.Message });
                }

                return Ok(structuredData);
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

        [HttpPost("generate-structured")]
        public async Task<IActionResult> GenerateStructuredPlayerStats(
            [FromBody] GenerateStructuredRequest request,
            [FromHeader(Name = "GeminiApiKey")] string apiKey,
            [FromHeader(Name = "Gemini-Model")] string? model)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest(new { error = "Prompt cannot be empty." });

            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest(new { error = "API key is required." });

            model ??= "gemini-1.5-flash"; // Default model

            using var httpClient = new HttpClient();
            var geminiClient = new GeminiStructuredClient(httpClient, apiKey);

            try
            {
                // Generate JSON schema for structured output
                object jsonSchema = JsonSchemaHelper.GenerateSchema<PlayerStats>();

                var response = await geminiClient.GenerateStructuredContentAsync(
                    model: model,
                    prompt: request.Prompt,
                    jsonSchema: jsonSchema
                );

                var jsonText = response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

                if (string.IsNullOrWhiteSpace(jsonText))
                {
                    return BadRequest(new { error = "No valid structured content received from Gemini API." });
                }

                // Deserialize the structured data into PlayerStats
                PlayerStats? structuredData;
                try
                {
                    structuredData = JsonSerializer.Deserialize<PlayerStats>(jsonText, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                catch (JsonException jsonEx)
                {
                    return BadRequest(new { error = "Invalid structured response format.", details = jsonEx.Message });
                }

                return Ok(structuredData);
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
