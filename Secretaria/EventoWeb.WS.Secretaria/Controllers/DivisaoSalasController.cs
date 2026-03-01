using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisaoSalasController : ControllerBase
    {
        private readonly AppDivisaoSalasEstudo mAppDivisaoSalas;

        public DivisaoSalasController(IContexto contexto)
        {
            mAppDivisaoSalas = new AppDivisaoSalasEstudo(contexto, contexto.RepositorioEventos, 
                contexto.RepositorioSalasEstudo, contexto.RepositorioInscricoes);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public IEnumerable<DTODivisaoSalaEstudo> ObterDivisao(int idEvento)
        {
            return mAppDivisaoSalas.ObterDivisao(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/divisao-automatica")]
        public IEnumerable<DTODivisaoSalaEstudo> RealizarDivisaoAutomatica(int idEvento)
        {
            return mAppDivisaoSalas.RealizarDivisaoAutomatica(idEvento);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/remover-todas-divisoes")]
        public IEnumerable<DTODivisaoSalaEstudo> RemoverTodasDivisoes(int idEvento)
        {
            return mAppDivisaoSalas.RemoverTodasDivisoes(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/inscricao/{idInscricao}/sala/{idSala}/incluir")]
        public IEnumerable<DTODivisaoSalaEstudo> IncluirInscricaoSala(int idEvento, int idSala, int idInscricao)
        {
            return mAppDivisaoSalas.IncluirParticipante(idEvento, idSala, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/mover-inscricao/{idInscricao}/da-sala/{daIdSala}/para-sala/{paraIdSala}")]
        public IEnumerable<DTODivisaoSalaEstudo> MoverInscricaoSalas(int idEvento, int daIdSala, int paraIdSala, int idInscricao)
        {
            return mAppDivisaoSalas.MoverParticipante(idEvento, idInscricao, daIdSala, paraIdSala);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/inscricao/{idInscricao}/sala/{idSala}/remover")]
        public IEnumerable<DTODivisaoSalaEstudo> RemoverInscricaoSala(int idEvento, int idInscricao, int idSala)
        {
            return mAppDivisaoSalas.RemoverParticipante(idEvento, idInscricao, idSala);
        }
    }
}