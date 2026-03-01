using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly AppDepartamentos mAppDepartamentos;

        public DepartamentosController(IContexto contexto)
        {
            mAppDepartamentos = new AppDepartamentos(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{id}")]
        public DTODepartamento GetObter(int id)
        {
            var departamento = mAppDepartamentos.ObterPorId(id);
            return departamento;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodos")]
        public IEnumerable<DTODepartamento> ListarTudo(int idEvento)
        {
            var lista = mAppDepartamentos.ObterTodos(idEvento);

            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId Incluir(int idEvento, [FromBody] DTODepartamento dto)
        {
            var id = mAppDepartamentos.Incluir(idEvento, dto.Nome);

            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{id}")]
        public void Alterar(int id, [FromBody] DTODepartamento dto)
        {
            mAppDepartamentos.Atualizar(id, dto.Nome);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{id}")]
        public void Excluir(int id)
        {
            mAppDepartamentos.Excluir(id);
        }

        /*[HttpGet]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("relatorioDivisao")]
        public String GerarRelatorioDivisao(int idEvento)
        {
            Stream relatorio = null;

            Evento evento = mEventos.ObterEventoPeloId(idEvento);            

            relatorio = new GeradorRelatorioDivisaoDepartamento().Gerar(
                mInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoDepartamento>(evento));

            using (var memoryStream = new MemoryStream())
            {
                relatorio.Position = 0;
                relatorio.CopyTo(memoryStream);
                var relatorioEmBytes = memoryStream.ToArray();

                return Convert.ToBase64String(relatorioEmBytes);
            }
        }*/
    }
}
