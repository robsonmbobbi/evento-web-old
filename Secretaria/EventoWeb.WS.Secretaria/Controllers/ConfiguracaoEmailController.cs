using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoEmailController : ControllerBase
    {
        private AppConfiguracoesEmail mAppConfiguracoesEmail;

        public ConfiguracaoEmailController(IContexto contexto)
        {
            mAppConfiguracoesEmail = new AppConfiguracoesEmail(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public DTOConfiguracaoEmail GetObter(int idEvento)
        {
            var configuracao = mAppConfiguracoesEmail.Obter(idEvento);
            return configuracao;
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/salvar")]
        public void Salvar(int idEvento, [FromBody] DTOConfiguracaoEmail dto)
        {
            mAppConfiguracoesEmail.CriarOuAtualizar(idEvento, dto);
        }
    }
}