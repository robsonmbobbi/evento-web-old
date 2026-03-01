using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private readonly AppInscricoes m_App;

        public InscricoesController(IContexto contexto, AppInscricoes app)
        {
            m_App = app;
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodas")]
        public IEnumerable<DTOBasicoInscricao> ListarTodas(int idEvento, EnumSituacaoInscricao situacao)
        {
            return m_App.ListarTodas(idEvento, situacao);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idInscricao}")]
        public void Excluir(int idEvento, int idInscricao)
        {
            m_App.Excluir(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idInscricao}")]
        public DTOInscricaoCompletaAdulto Obter(int idEvento, int idInscricao)
        {
            return m_App.Obter(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter-infantil/{idInscricao}")]
        public DTOInscricaoCompletaInfantil ObterInfantil(int idEvento, int idInscricao)
        {
            return m_App.ObterInfantil(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/aceitar/{idInscricao}")]
        public void Aceitar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacaoAdulto atualizacao)
        {
            m_App.Aceitar(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/aceitar-infantil/{idInscricao}")]
        public void AceitarInfantil(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacaoInfantil atualizacao)
        {
            m_App.AceitarInfantil(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/rejeitar/{idInscricao}")]
        public void Rejeitar(int idEvento, int idInscricao)
        {
            m_App.Rejeitar(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idInscricao}")]
        public void Atualizar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacaoAdulto atualizacao)
        {
            m_App.Atualizar(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar-infantil/{idInscricao}")]
        public void AtualizarInfantil(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacaoInfantil atualizacao)
        {
            m_App.AtualizarInfantil(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/incluir")]
        public void Incluir(int idEvento, [FromBody] DTOInscricaoAtualizacaoAdulto inscricao)
        {
            m_App.Incluir(idEvento, inscricao);
        }

        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/incluir-infantil")]
        public void IncluirInfantil(int idEvento, [FromBody] DTOInscricaoAtualizacaoInfantil inscricao)
        {
            m_App.IncluirInfantil(idEvento, inscricao);
        }

        [HttpGet("teste")]
        public void EnviarTeste([FromServices] AppComunicacao appComunicacao)
        {
            var inscricao = m_App.Contexto.RepositorioInscricoes.ObterInscricaoPeloId(575);
            //appComunicacao.EnviarInscricaoAceita(inscricao);
            appComunicacao.EnviarInscricaoRegistradaInfantil(inscricao as InscricaoInfantil);
            //appComunicacao.EnviarCodigoAcompanhamentoInscricao(inscricao, "codigo_enviar");
        }
    }
}