using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagensEmailInscricaoController : ControllerBase
    {
        private readonly AppMensagensEmailInscricao mAppMensagem;

        public MensagensEmailInscricaoController(IContexto contexto)
        {
            mAppMensagem = new AppMensagensEmailInscricao(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public DTOMensagemEmailInscricao GetObter(int idEvento)
        {
            return mAppMensagem.ObterPorEvento(idEvento);
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/atualizar")]
        public void Atualizar(int idEvento, [FromBody] DTOMensagemEmailInscricao dto)
        {
            mAppMensagem.Atualizar(idEvento, dto);            
        }        
    }
}
