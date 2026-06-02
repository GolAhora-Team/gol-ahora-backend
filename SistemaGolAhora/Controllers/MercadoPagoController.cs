using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Common;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        public MercadoPagoController()
        {
            // Token de la nueva cuenta VENDEDORA de prueba
            MercadoPagoConfig.AccessToken = "APP_USR-8827220724965081-060211-5262bca461db2a832b7cfa1ee3dd428c-3442109685"; 
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequest request)
        {
            try
            {
                var extRef = Guid.NewGuid().ToString();

                var requestMP = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                    {
                        new PreferenceItemRequest
                        {
                            Title = request.Title,
                            Quantity = 1,
                            CurrencyId = "ARS",
                            UnitPrice = request.Price,
                        }
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        // Volvemos a usar la URL dinámica que manda el frontend (ej. tu Vercel)
                        Success = request.ReturnUrl ?? "https://golahora.runasp.net",
                        Failure = request.ReturnUrl ?? "https://golahora.runasp.net",
                    },
                    AutoReturn = "approved",
                    ExternalReference = extRef
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(requestMP);

                // Usamos DIRECTAMENTE InitPoint. SandboxInitPoint puede fallar con tokens APP_USR
                return Ok(new { initPoint = preference.InitPoint, externalReference = extRef });
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
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MercadoPagoConfig.AccessToken);
                
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
