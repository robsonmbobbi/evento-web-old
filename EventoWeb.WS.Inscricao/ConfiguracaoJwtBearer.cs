using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace EventoWeb.WS.Inscricao
{
    public class ConfiguracaoJwtBearer
    {
        public ConfiguracaoJwtBearer()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                ChaveEmissor = new RsaSecurityKey(provider.ExportParameters(true));
            }

            CredencialAssinatura = new SigningCredentials(ChaveEmissor, SecurityAlgorithms.RsaSha256Signature);
            Publico = "EventoWeb";
            Emissor = "EventoWeb_Emissor";
        }

        public SecurityKey ChaveEmissor { get; }
        public string Publico { get; }
        public string Emissor { get; }
        public SigningCredentials CredencialAssinatura { get; }
        public double TempoSegExpirar { get; }

        public string GerarTokenJWTBearer(string identificacao)
        {
            ClaimsIdentity identidade = new ClaimsIdentity(
                    new GenericIdentity(identificacao, "IdInscricao"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, identificacao)
                    });

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Emissor,
                Audience = Publico,
                SigningCredentials = CredencialAssinatura,
                Subject = identidade,
                NotBefore = DateTime.Now,
                Expires = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59)
            });

            return handler.WriteToken(securityToken);
        }
    }
}
