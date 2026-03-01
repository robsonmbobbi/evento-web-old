using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartosController : ControllerBase
    {
        private readonly AppQuartos mAppQuartos;

        public QuartosController(IContexto contexto)
        {
            mAppQuartos = new AppQuartos(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idQuarto}")]
        public DTOQuarto GetObter(int idEvento, int idQuarto)
        {
            var quarto = mAppQuartos.ObterPorId(idEvento, idQuarto);
            return quarto;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodos")]
        public IEnumerable<DTOQuarto> ListarTudo(int idEvento)
        {
            var lista = mAppQuartos.ObterTodos(idEvento);
            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId IncluirQuarto(int idEvento, [FromBody] DTOQuarto dtoQuarto)
        {
            var id = mAppQuartos.Incluir(idEvento, dtoQuarto);
            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idQuarto}")]
        public void AlterarQuarto(int idEvento, int idQuarto, [FromBody] DTOQuarto dtoQuarto)
        {
            mAppQuartos.Atualizar(idEvento, idQuarto, dtoQuarto);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idQuarto}")]
        public void ExcluirQuarto(int idEvento, int idQuarto)
        {
            mAppQuartos.Excluir(idEvento, idQuarto);
        }        
    }
}
