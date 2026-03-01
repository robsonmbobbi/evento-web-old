using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OficinasController : ControllerBase
    {
        private readonly AppOficinas mAppOficinas;

        public OficinasController(IContexto contexto)
        {
            mAppOficinas = new AppOficinas(contexto);
        }

        // GET api/oficina/obter/1
        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idOficina}")]
        public DTOOficina GetObterOficina(int idEvento, int idOficina)
        {
            var oficina = mAppOficinas.ObterPorId(idEvento, idOficina);
            return oficina;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodas")]
        public IEnumerable<DTOOficina> ListarOficinas(int idEvento)
        {
            var lista = mAppOficinas.ObterTodos(idEvento);

            return lista;
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId IncluirOficina(int idEvento, [FromBody] DTOOficina dtoOficina)
        {
            var id = mAppOficinas.Incluir(idEvento, dtoOficina);

            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idOficina}")]
        public void AlterarOficina(int idEvento, int idOficina, [FromBody] DTOOficina dtoOficina)
        {
            mAppOficinas.Atualizar(idEvento, idOficina, dtoOficina);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idOficina}")]
        public void ExcluirOficina(int idEvento, int idOficina)
        {
            mAppOficinas.Excluir(idEvento, idOficina);
        }
    }
}