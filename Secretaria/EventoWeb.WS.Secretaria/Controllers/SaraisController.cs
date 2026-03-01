using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaraisController : ControllerBase
    {
        private readonly AppApresentacaoSarau mAppSarais;

        public SaraisController(IContexto contexto)
        {
            mAppSarais = new AppApresentacaoSarau(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idSarau}")]
        public DTOSarau GetObter(int idEvento, int idSarau)
        {
            var Sarau = mAppSarais.Obter(idEvento, idSarau);
            return Sarau;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodos")]
        public IEnumerable<DTOSarau> ListarTudo(int idEvento)
        {
            var lista = mAppSarais.Listar(idEvento);
            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId IncluirSarau(int idEvento, [FromBody] DTOSarau dtoSarau)
        {
            var id = mAppSarais.Incluir(idEvento, dtoSarau);
            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idSarau}")]
        public void AlterarSarau(int idEvento, int idSarau, [FromBody] DTOSarau dtoSarau)
        {
            mAppSarais.Atualizar(idEvento, idSarau, dtoSarau);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idSarau}")]
        public void ExcluirSarau(int idEvento, int idSarau)
        {
            mAppSarais.Excluir(idEvento, idSarau);
        }        
    }
}
