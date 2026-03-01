using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private const string TIPO_CONTEUDO_PDF = "application/pdf";
        private readonly IContexto m_Contexto;

        public RelatoriosController(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/divisao-salas")]
        public IActionResult GerarRelatorioDivisaoSalas(int idEvento)
        {
            var appRelDivisao = new AppRelatorioDivisaoSalas(m_Contexto, 
                m_Contexto.RepositorioEventos, m_Contexto.RepositorioSalasEstudo, m_Contexto.RepositorioInscricoes,
                m_Contexto.RelatorioDivisaoSalasEstudo);

            return File(appRelDivisao.GerarImpressoPDF(idEvento), TIPO_CONTEUDO_PDF);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/divisao-oficinas")]
        public IActionResult GerarRelatorioDivisaoOficinas(int idEvento)
        {
            var appRelDivisao = new AppRelatorioDivisaoOficinas(m_Contexto,
                m_Contexto.RepositorioEventos, m_Contexto.RepositorioOficinas, m_Contexto.RepositorioInscricoes,
                m_Contexto.RelatorioDivisaoOficinas);

            return File(appRelDivisao.GerarImpressoPDF(idEvento), TIPO_CONTEUDO_PDF);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/divisao-quartos")]
        public IActionResult GerarRelatorioDivisaoQuartos(int idEvento)
        {
            var appRelDivisao = new AppRelatorioDivisaoQuartos(m_Contexto,
                m_Contexto.RepositorioQuartos, m_Contexto.RelatorioDivisaoQuartos);

            return File(appRelDivisao.GerarImpressoPDF(idEvento), TIPO_CONTEUDO_PDF);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/inscritos-departamentos")]
        public IActionResult GerarRelatorioInscritosDepartamentos(int idEvento)
        {
            var appRelDivisao = new AppDepartamentos(m_Contexto);

            return File(appRelDivisao.GerarImpressoPDFInscritos(idEvento), TIPO_CONTEUDO_PDF);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/listagem-sarau")]
        public IActionResult GerarRelatorioListagemSarau(int idEvento)
        {
            var appRelSarau = new AppApresentacaoSarau(m_Contexto);

            return File(appRelSarau.GerarImpressoPDF(idEvento), TIPO_CONTEUDO_PDF);
        }
    }
}