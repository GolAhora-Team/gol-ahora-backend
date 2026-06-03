using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        public MercadoPagoController()
        {
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequest request)
        {
            try
            {
                var extRef = Guid.NewGuid().ToString();

                var body = new
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
                        success = request.ReturnUrl ?? "https://golahora.runasp.net",
                        failure = request.ReturnUrl ?? "https://golahora.runasp.net",
                        pending = request.ReturnUrl ?? "https://golahora.runasp.net"
                    },
                    auto_return = "approved",
                    external_reference = extRef
                };

                using var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "APP_USR-8827220724965081-060211-5262bca461db2a832b7cfa1ee3dd428c-3442109685");
                
                var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { mensaje = "Error de MP API: " + errorJson });
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                
                if (result.TryGetProperty("init_point", out var initPoint))
                {
                    return Ok(new { initPoint = initPoint.GetString(), externalReference = extRef });
                }
                
                return BadRequest(new { mensaje = "La API de MP no devolvió el init_point." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear preferencia de MP: " + ex.Message });
            }
        }

        [HttpGet("check-payment/{externalReference}")]
        public async Task<IActionResult> CheckPayment(string externalReference)
        {
            try
            {
                using var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "APP_USR-8827220724965081-060211-5262bca461db2a832b7cfa1ee3dd428c-3442109685");
                
                var response = await httpClient.GetAsync($"https://api.mercadopago.com/v1/payments/search?external_reference={externalReference}");
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { mensaje = "Error consultando API de Mercado Pago." });
                }
                
                var json = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                
                bool isApproved = false;
                if (result.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var payment in resultsArray.EnumerateArray())
                    {
                        if (payment.TryGetProperty("status", out var status) && status.GetString() == "approved")
                        {
                            isApproved = true;
                            break;
                        }
                    }
                }
                
                return Ok(new { approved = isApproved });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = "Error al verificar pago: " + ex.Message });
            }
        }
    }

    public class CreatePreferenceRequest
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
