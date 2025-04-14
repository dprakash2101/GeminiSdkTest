using GeminiSharp.Client;
using GeminiSharp.Helpers;
using GeminiSharp.Models.Request;
using GeminiSharp.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace GeminiSdkTest.Controllers
{
    [Route("imagegeneration")]
    [ApiController]
    public class GeminiImageGenerationController : ControllerBase
    {
        [HttpPost("generateimage")]
        public async Task<IActionResult> GenerateImage(
            [FromBody] GenerateImageRequest request,
            [FromHeader(Name = "GeminiApiKey")] string apiKey,
            [FromHeader(Name = "Gemini-Model")] string? model)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                Log.Error("Prompt is empty or null in GenerateImage.");
                return BadRequest("Prompt cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Log.Error("GeminiApiKey header is empty or null in GenerateImage.");
                return BadRequest("GeminiApiKey header is required.");
            }

            try
            {
                Log.Information("Generating image with prompt: {Prompt}, model: {Model}", request.Prompt, model ?? "default");

                // Default model if not provided
                string selectedModel = model ?? "gemini-2.0-flash-exp-image-generation";

                // Initialize client with API key from header
                var imageClient = new GeminiImageGenerationClient(apiKey);

                var config = new ImageGenerationConfig
                {
                    ResponseModalities = new List<string> { "Text", "Image" } // Ensure image output
                };

                var response = await imageClient.GenerateImageAsync(request.Prompt, config, selectedModel);

                // Extract base64 image data (adjust based on actual response structure)
                var base64Image = response.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.InlineData?.Data;
                if (string.IsNullOrWhiteSpace(base64Image))
                {
                    Log.Error("No image data found in response for prompt: {Prompt}, model: {Model}", request.Prompt, selectedModel);
                    return Ok(response);
                }

                // Convert base64 to MemoryStream using ImageConversionHelper
                string mimeType = "image/png"; // Adjust if API specifies
                var imageStream = ImageConversionHelper.ConvertBase64ToImageStream(base64Image, mimeType);
                if (imageStream == null)
                {
                    Log.Error("Failed to convert base64 image for prompt: {Prompt}, model: {Model}", request.Prompt, selectedModel);
                    return StatusCode(500, "Failed to process image data.");
                }

                Log.Information("Successfully prepared image response for prompt: {Prompt}, model: {Model}", request.Prompt, selectedModel);

                // Return image as file for Postman to display
                using (var image = Image.Load<Rgba32>(imageBytes))
                {
                    // Resize to 1920x1080 with crop mode
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1920, 1080),
                        Mode = ResizeMode.Crop
                    }));

                    using (var ms = new MemoryStream())
                    {
                        // Save the image to memory stream with PNG encoder settings
                        image.Save(ms, new PngEncoder
                        {
                            CompressionLevel = PngCompressionLevel.BestCompression
                        });

                        ms.Position = 0;

                        // Return the image file
                        return new FileContentResult(ms.ToArray(), "image/png")
                        {
                            FileDownloadName = "generated-image-highres.png"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error generating image for prompt: {Prompt}, model: {Model}", request.Prompt, model ?? "default");
                return StatusCode(500, $"Error generating image: {ex.Message}");
            }
        }
    }

    // DTO for request body
    public class GenerateImageRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }
}