using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly AppSalasEstudo mAppSalasEstudo;

        public SalasController(IContexto contexto)
        {
            mAppSalasEstudo = new AppSalasEstudo(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idSala}")]
        public DTOSalaEstudo GetObter(int idEvento, int idSala)
        {
            var sala = mAppSalasEstudo.ObterPorId(idEvento, idSala);
            return sala;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodas")]
        public IEnumerable<DTOSalaEstudo> ListarTudo(int idEvento)
        {
            var lista = mAppSalasEstudo.ObterTodos(idEvento);
            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId IncluirSala(int idEvento, [FromBody] DTOSalaEstudo dtoSala)
        {
            var id = mAppSalasEstudo.Incluir(idEvento, dtoSala);
            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idSala}")]
        public void AlterarSala(int idEvento, int idSala, [FromBody] DTOSalaEstudo dtoSala)
        {
            mAppSalasEstudo.Atualizar(idEvento, idSala, dtoSala);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idSala}")]
        public void ExcluirSala(int idEvento, int idSala)
        {
            mAppSalasEstudo.Excluir(idEvento, idSala);
        }
    }
}
