using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace EventoWeb.WS.Secretaria
{
    public class ConfiguracaoAutorizacao
    {
        public ConfiguracaoAutorizacao()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                ChaveEmissor = new RsaSecurityKey(provider.ExportParameters(true));
            }

            CredenciasAssinatura = new SigningCredentials(ChaveEmissor, SecurityAlgorithms.RsaSha256Signature);
            Publico = "EventoWeb";
            Emissor = "EventoWeb_Emissor";
            TempoSegExpirar = 24 * 3600;
        }

        public SecurityKey ChaveEmissor { get; }
        public string Publico { get; }
        public string Emissor { get; }
        public SigningCredentials CredenciasAssinatura { get; }
        public double TempoSegExpirar { get; }
    }
}
