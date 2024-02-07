namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; } // Secret é a chave de criptografia
        public int ExpiracaoHoras { get; set; } // ExpiracaoHoras é o tempo de expiração do token
        public string Emissor { get; set; } // Emissor é o emissor do token
        public string ValidoEm { get; set; } // ValidoEm é o domínio que o token é válido
    }
}