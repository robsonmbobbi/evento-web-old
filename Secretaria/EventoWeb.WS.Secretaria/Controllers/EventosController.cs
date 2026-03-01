using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private AppEvento mAppEvento;

        public EventosController(IContexto contexto)
        {
            mAppEvento = new AppEvento(contexto);
        }

        // GET api/eventos/obter-todos
        [HttpGet("obter-todos")]
        [Authorize("Bearer")]
        public ActionResult<IEnumerable<DTOEventoMinimo>> Get()
        {
            return mAppEvento.ObterTodos().ToList();
        }

        // GET api/eventos/obter-id/5
        [HttpGet("obter-id/{id}")]
        [Authorize("Bearer")]
        public ActionResult<DTOEventoCompleto> Get(int id)
        {
            return mAppEvento.ObterPorId(id);
        }

        [HttpGet("obter-para-inscricao/{id}")]
        [Authorize("Bearer")]
        public ActionResult<DTOEventoCompletoInscricao> GetCompletoInscricao(int id)
        {
            return mAppEvento.ObterPorIdCompletoInscricao(id);
        }

        // POST api/eventos/incluir
        [HttpPost("incluir")]
        [Authorize("Bearer")]
        public DTOId Post([FromBody] DTOEvento eventoDTO)
        {
            return mAppEvento.Incluir(eventoDTO);
        }

        // PUT api/eventos/atualizar/5
        [HttpPut("atualizar/{id}")]
        [Authorize("Bearer")]
        public void Put(int id, [FromBody] DTOEvento eventoDTO)
        {
            mAppEvento.Atualizar(id, eventoDTO);
        }

        // DELETE api/eventos/excluir/5
        [HttpDelete("excluir/{id}")]
        [Authorize("Bearer")]
        public void Delete(int id)
        {
            mAppEvento.Excluir(id);
        }
    }
}
