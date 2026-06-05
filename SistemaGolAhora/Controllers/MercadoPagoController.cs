using Aplication.Interfaces.IPago;
using Aplication.Interfaces.IReserva;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly IPagoQuery _pagoQuery;
        private readonly IPagoCommand _pagoCommand;
        private readonly IReservaQuery _reservaQuery;
        private readonly IReservaCommand _reservaCommand;
        private readonly MercadoPagoSettings _mpSettings;

        public MercadoPagoController(
            IPagoQuery pagoQuery,
            IPagoCommand pagoCommand,
            IReservaQuery reservaQuery,
            IReservaCommand reservaCommand,
            IOptions<MercadoPagoSettings> mpSettings)
        {
            _pagoQuery = pagoQuery;
            _pagoCommand = pagoCommand;
            _reservaQuery = reservaQuery;
            _reservaCommand = reservaCommand;
            _mpSettings = mpSettings.Value;
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequest request)
        {
            try
            {
                var extRef = request.ExternalReference ?? Guid.NewGuid().ToString();

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
                        success = string.IsNullOrWhiteSpace(request.ReturnUrl) ? _mpSettings.DefaultReturnUrl : request.ReturnUrl,
                        failure = string.IsNullOrWhiteSpace(request.ReturnUrl) ? _mpSettings.DefaultReturnUrl : request.ReturnUrl,
                        pending = string.IsNullOrWhiteSpace(request.ReturnUrl) ? _mpSettings.DefaultReturnUrl : request.ReturnUrl
                    },
                    notification_url = !string.IsNullOrEmpty(request.WebhookUrl) ? request.WebhookUrl : _mpSettings.WebhookUrl, // Webhook
                    auto_return = "approved",
                    external_reference = extRef
                };

                using var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _mpSettings.AccessToken);
                
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
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _mpSettings.AccessToken);
                
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

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] System.Text.Json.JsonElement? payload, [FromQuery] string? type, [FromQuery] string? topic, [FromQuery(Name = "data.id")] string? dataIdQuery, [FromQuery] string? id)
        {
            try
            {
                string eventType = type ?? topic ?? string.Empty;
                string dataId = dataIdQuery ?? id ?? string.Empty;

                if (payload.HasValue && payload.Value.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    if (string.IsNullOrEmpty(eventType))
                    {
                        if (payload.Value.TryGetProperty("type", out var typeProp)) eventType = typeProp.GetString() ?? "";
                        else if (payload.Value.TryGetProperty("action", out var actionProp)) eventType = actionProp.GetString() ?? "";
                        else if (payload.Value.TryGetProperty("topic", out var topicProp)) eventType = topicProp.GetString() ?? "";
                    }

                    if (string.IsNullOrEmpty(dataId))
                    {
                        if (payload.Value.TryGetProperty("data", out var dataProp) && dataProp.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            if (dataProp.TryGetProperty("id", out var idProp)) dataId = idProp.ToString();
                        }
                    }
                }

                if ((eventType == "payment" || eventType == "payment.created") && !string.IsNullOrEmpty(dataId))
                {
                    // Consultar la API de MP para ver los detalles del pago
                    using var httpClient = new System.Net.Http.HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _mpSettings.AccessToken);
                    
                    var response = await httpClient.GetAsync($"https://api.mercadopago.com/v1/payments/{dataId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                        
                        if (result.TryGetProperty("status", out var status) && status.GetString() == "approved")
                        {
                            if (result.TryGetProperty("external_reference", out var extRefElement))
                            {
                                string extRef = extRefElement.GetString() ?? "";
                                // Parseamos el external_reference, por ejemplo si es formato "Reserva_123" o solo el "123"
                                if (int.TryParse(extRef, out int reservaId) || (extRef.StartsWith("Reserva_") && int.TryParse(extRef.Replace("Reserva_", ""), out reservaId)))
                                {
                                    // Marcar Reserva como Confirmada
                                    var reserva = await _reservaQuery.GetReservaById(reservaId);
                                    if (reserva != null && reserva.Estado != EstadoReserva.Confirmada)
                                    {
                                        reserva.Estado = EstadoReserva.Confirmada;
                                        await _reservaCommand.UpdateReserva(reserva);
                                    }

                                    // Marcar el Pago correspondiente como Pagado
                                    if (reserva != null && reserva.FacturaId.HasValue)
                                    {
                                        var pagos = await _pagoQuery.GetListPagos();
                                        var pagoPendiente = pagos.FirstOrDefault(p => p.FacturaId == reserva.FacturaId.Value && p.Estado == EstadoPago.Pendiente);
                                        if (pagoPendiente != null)
                                        {
                                            pagoPendiente.Estado = EstadoPago.Pagado;
                                            await _pagoCommand.UpdatePago(pagoPendiente);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Log error, pero retornar 200 a MP para que no reintente locamente
                Console.WriteLine("Error procesando Webhook: " + ex.Message);
            }

            return Ok(); // Siempre retornar 200 OK a Mercado Pago
        }
    }

    public class CreatePreferenceRequest
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? ReturnUrl { get; set; }
        public string? WebhookUrl { get; set; }
        public string? ExternalReference { get; set; }
    }
}
