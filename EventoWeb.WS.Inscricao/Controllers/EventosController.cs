using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly AppInscOnlineEvento mAppEvento;
        private readonly AppContratosInscricao mAppContrato;

        public EventosController(IContexto contexto)
        {
            mAppEvento = new AppInscOnlineEvento(contexto);
            mAppContrato = new AppContratosInscricao(contexto);
        }

        [HttpGet("disponiveis")]
        public IEnumerable<DTOEventoMinimo> ListarEventosDisponiveis()
        {
            return mAppEvento.ListarEventosDisponiveisInscricaoOnline();
        }

        [HttpGet("{id}")]
        public DTOEventoMinimo ObterEventoMinimo(int id)
        {
            return mAppEvento.ObterPorIdDisponivelInscricaoOnline(id);
        }

        [HttpGet("{id}/completo")]
        public DTOEventoCompleto ObterEventoCompleto(int id)
        {
            return mAppEvento.ObterPorIdCompleto(id);
        }


        [HttpGet("{id}/contrato")]
        public DTOContratoInscricao ObterContrato(int id)
        {
            return mAppContrato.ObterPorEvento(id);
        }
    }
}