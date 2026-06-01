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
            // Token de prueba genérico para desarrolladores de Mercado Pago.
            // Para producción, se debe reemplazar por un token de producción y colocarlo en appsettings.json.
            MercadoPagoConfig.AccessToken = "TEST-4008272993876356-021015-8914ba14fb96a096c4293c6ebf2d7253-1678128330"; 
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
                        // En local, podrías poner localhost. Para dev/prod, la URL de tu app.
                        Success = "https://golahora.runasp.net",
                        Failure = "https://golahora.runasp.net",
                        Pending = "https://golahora.runasp.net"
                    },
                    AutoReturn = "approved",
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(requestMP);

                // Retornamos el InitPoint que es la URL a la que el frontend debe redirigir al usuario.
                // Usar SandboxInitPoint si quieres forzar el entorno de sandbox, o InitPoint que se adapta al token.
                return Ok(new { initPoint = preference.SandboxInitPoint ?? preference.InitPoint });
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
    }
}
