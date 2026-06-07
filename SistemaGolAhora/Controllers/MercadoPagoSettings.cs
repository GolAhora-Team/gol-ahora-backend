namespace SistemaGolAhora.Controllers
{
    public class MercadoPagoSettings
    {
        public string AccessToken { get; set; } = string.Empty;
        public string ProductionAccessToken { get; set; } = string.Empty;
        public string WebhookUrl { get; set; } = string.Empty;
        public string DefaultReturnUrl { get; set; } = string.Empty;
    }
}
