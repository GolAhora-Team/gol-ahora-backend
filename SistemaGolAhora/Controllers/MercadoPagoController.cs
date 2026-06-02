using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
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
                        Pending = request.ReturnUrl ?? "https://golahora.runasp.net"
                    },
                    AutoReturn = "approved",
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(requestMP);

                // Usamos DIRECTAMENTE InitPoint. SandboxInitPoint puede fallar con tokens APP_USR
                return Ok(new { initPoint = preference.InitPoint });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear preferencia de MP: " + ex.Message });
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
