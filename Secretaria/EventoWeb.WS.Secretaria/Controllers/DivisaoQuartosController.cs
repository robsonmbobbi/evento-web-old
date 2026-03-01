using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisaoQuartosController : ControllerBase
    {
        private readonly AppDivisaoQuartos mAppDivisaoQuartos;

        public DivisaoQuartosController(IContexto contexto)
        {
            mAppDivisaoQuartos = new AppDivisaoQuartos(contexto, contexto.RepositorioEventos, 
                contexto.RepositorioQuartos, contexto.RepositorioInscricoes);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public IEnumerable<DTODivisaoQuarto> ObterDivisao(int idEvento)
        {
            return mAppDivisaoQuartos.ObterDivisao(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/divisao-automatica")]
        public IEnumerable<DTODivisaoQuarto> RealizarDivisaoAutomatica(int idEvento)
        {
            return mAppDivisaoQuartos.RealizarDivisaoAutomatica(idEvento);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/remover-todas-divisoes")]
        public IEnumerable<DTODivisaoQuarto> RemoverTodasDivisoes(int idEvento)
        {
            return mAppDivisaoQuartos.RemoverTodasDivisoes(idEvento);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/inscricao/{idInscricao}/quarto/{idQuarto}/incluir")]
        public IEnumerable<DTODivisaoQuarto> IncluirInscricaoQuarto(int idEvento, int idQuarto, int idInscricao)
        {
            return mAppDivisaoQuartos.IncluirParticipante(idEvento, idQuarto, idInscricao, false);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/mover-inscricao/{idInscricao}/da-quarto/{daIdQuarto}/para-quarto/{paraIdQuarto}")]
        public IEnumerable<DTODivisaoQuarto> MoverInscricaoQuartos(int idEvento, int daIdQuarto, int paraIdQuarto, int idInscricao)
        {
            return mAppDivisaoQuartos.MoverParticipante(idEvento, idInscricao, daIdQuarto, paraIdQuarto, false);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/inscricao/{idInscricao}/quarto/{idQuarto}/remover")]
        public IEnumerable<DTODivisaoQuarto> RemoverInscricaoQuarto(int idEvento, int idInscricao, int idQuarto)
        {
            return mAppDivisaoQuartos.RemoverParticipante(idEvento, idInscricao, idQuarto);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/inscricao/{idInscricao}/quarto/{idQuarto}/coordenador/{ehCoordenador}")]
        public IEnumerable<DTODivisaoQuarto> DefinirSeEhCoordenador(int idEvento, int idInscricao, int idQuarto, bool ehCoordenador)
        {
            return mAppDivisaoQuartos.DefinirSeEhCoordenador(idEvento, idQuarto, idInscricao, ehCoordenador);
        }
    }
}