using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencaoInscricoesController : ControllerBase
    {
        private readonly AppInscOnlineEventoManutencaoInscricoes mAppInscricao;

        public ManutencaoInscricoesController(IContexto contexto)
        {
            mAppInscricao = new AppInscOnlineEventoManutencaoInscricoes(contexto);
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("obterInscricao/{id}")]
        public DTOInscricaoCompletaAdulto ObterInscricao(int id)
        {
            return mAppInscricao.ObterInscricao(id);
        }

        [HttpGet("evento/{idEvento}/obterSarau/{codigo}")]
        public DTOSarau ObterSarau(int idEvento, string codigo) 
        {
            return mAppInscricao.ObterSarau(idEvento, codigo);
        }

        [HttpGet("obterInscricaoInfantil/{idInscricao}")]
        public DTOInscricaoCompletaInfantil ObterInscricaoInfantil(int idInscricao)
        {
            return mAppInscricao.ObterInscricaoInfantil(idInscricao);
        }

        [HttpGet("evento/{idEvento}/obter-inscricao-adulto/{codigo}")]
        public DTOInscricaoSimplificada ObterInscricaoAdultoCodigo(int idEvento, string codigo)
        {
            return mAppInscricao.ObterInscricaoAdultoPorCodigo(idEvento, codigo);
        }
    }
}