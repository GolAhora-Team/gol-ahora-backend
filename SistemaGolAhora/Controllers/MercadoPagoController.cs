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
            // Token proporcionado por el usuario
            MercadoPagoConfig.AccessToken = "APP_USR-3447691466274967-060117-86028c62ab417279b1562a4ae99ce8ad-3443332308"; 
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
