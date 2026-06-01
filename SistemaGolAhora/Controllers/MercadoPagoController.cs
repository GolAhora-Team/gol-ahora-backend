using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace SistemaGolAhora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MercadoPagoController : ControllerBase
    {
        // Token de integración de desarrollo proporcionado
        private readonly string _accessToken = "APP_USR-5338456150462045-060113-46b91ba92c8c81ccc93c791a9678912a-3441156271";

        public class CreatePreferenceRequest
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequest request)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

                var payload = new
                {
                    items = new[]
                    {
                        new
                        {
                            title = request.Title,
                            quantity = 1,
                            currency_id = "ARS",
                            unit_price = request.Price
                        }
                    },
                    back_urls = new
                    {
                        success = "http://localhost:8081/",
                        failure = "http://localhost:8081/",
                        pending = "http://localhost:8081/"
                    },
                    auto_return = "approved"
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { error = "Error creando preferencia en MP", details = responseString });
                }

                var responseData = JsonSerializer.Deserialize<JsonElement>(responseString);
                var initPoint = responseData.GetProperty("init_point").GetString();

                return Ok(new { initPoint });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
