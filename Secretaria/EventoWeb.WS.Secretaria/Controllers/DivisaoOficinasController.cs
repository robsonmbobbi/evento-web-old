using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisaoOficinasController : ControllerBase
    {
        private readonly AppDivisaoOficinas mAppDivisaoOficinas;

        public DivisaoOficinasController(IContexto contexto)
        {
            mAppDivisaoOficinas = new AppDivisaoOficinas(contexto, contexto.RepositorioEventos, 
                contexto.RepositorioOficinas, contexto.RepositorioInscricoes);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public IEnumerable<DTODivisaoOficina> ObterDivisao(int idEvento)
        {
            return mAppDivisaoOficinas.ObterDivisao(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/divisao-automatica")]
        public IEnumerable<DTODivisaoOficina> RealizarDivisaoAutomatica(int idEvento)
        {
            return mAppDivisaoOficinas.RealizarDivisaoAutomatica(idEvento);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/remover-todas-divisoes")]
        public IEnumerable<DTODivisaoOficina> RemoverTodasDivisoes(int idEvento)
        {
            return mAppDivisaoOficinas.RemoverTodasDivisoes(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/inscricao/{idInscricao}/oficina/{idOficina}/incluir")]
        public IEnumerable<DTODivisaoOficina> IncluirInscricaoOficina(int idEvento, int idOficina, int idInscricao)
        {
            return mAppDivisaoOficinas.IncluirParticipante(idEvento, idOficina, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/mover-inscricao/{idInscricao}/da-oficina/{daIdOficina}/para-oficina/{paraIdOficina}")]
        public IEnumerable<DTODivisaoOficina> MoverInscricaoOficinas(int idEvento, int daIdOficina, int paraIdOficina, int idInscricao)
        {
            return mAppDivisaoOficinas.MoverParticipante(idEvento, idInscricao, daIdOficina, paraIdOficina);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/inscricao/{idInscricao}/oficina/{idOficina}/remover")]
        public IEnumerable<DTODivisaoOficina> RemoverInscricaoOficina(int idEvento, int idInscricao, int idOficina)
        {
            return mAppDivisaoOficinas.RemoverParticipante(idEvento, idInscricao, idOficina);
        }
    }
}