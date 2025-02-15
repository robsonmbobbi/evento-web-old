using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private readonly AppInscOnlineEventoAcessoInscricoes mAppInscricao;
        private readonly ConfiguracaoJwtBearer mConfiguracaoJwt;

        public InscricoesController(IContexto contexto, ConfiguracaoJwtBearer configuracaoJwt, AppInscOnlineEventoAcessoInscricoes app)
        {
            mAppInscricao = app;
            mConfiguracaoJwt = configuracaoJwt;
        }

        /*[HttpGet("teste")]
        public void EnviarTeste()
        {
            var mensagem = mAppInscricao.Contexto.RepositorioMensagensEmailPadrao.Obter(10);
            var dto = new DTOInscricaoCompletaAdultoCodigo
            {
                CentroEspirita = "ssds",
                Codigo = "12134",
                Evento = new DTOEventoCompletoInscricao
                {
                    Id = 1,
                    Nome = "",
                },
            };

            var srv = new ServicoEmail();
            srv.Configuracao = mAppInscricao.Contexto.RepositorioConfiguracoesEmail.Obter(10);
            srv.Enviar(new Nucleo.Aplicacao.Comunicacao.Email()
            {
                Assunto = "Teste",
                Conteudo = "Teste de envio",
                Endereco = "robsonmbobbi@gmail.com"
            });
        }*/

        [HttpGet("basicoPorId/{id}")]
        public DTOBasicoInscricao ObterPorId(int id)
        {
            return mAppInscricao.ObterPorId(id);
        }

        [HttpPut("validarCodigo/{id}")]
        public DTOAcessoInscricao ValidarCodigo(int id, [FromBody]string codigo)
        {
            if (!mAppInscricao.ValidarCodigo(id, codigo))
                return null;
            else
                return new DTOAcessoInscricao
                {
                    Autorizacao = mConfiguracaoJwt.GerarTokenJWTBearer(id.ToString() + codigo),
                    IdInscricao = id
                };            
        }

        [HttpPut("enviarCodigo/{identificacao}")]
        public DTOEnvioCodigoAcessoInscricao EnviarCodigo(string identificacao)
        {
            return mAppInscricao.EnviarCodigo(identificacao);
        }

        [HttpPost("criar/{idEvento}")]
        public DTODadosConfirmacao CriarInscricao(int idEvento, [FromBody] DTOInscricaoAtualizacaoAdulto dadosInscricao)
        {
            return mAppInscricao.CriarInscricao(idEvento, dadosInscricao);
        }

        [HttpPut("validarCodigoEmail")]
        public bool ValidarCodigoEmail(DTOValidacaoCodigoEmail validar)
        {
            return mAppInscricao.ValidarCodigoEmail(validar.Identificacao, validar.Codigo);
        }

        [HttpPut("enviarCodigoEmail/{idEvento}")]
        public void EnviarCodigoEmail(int idEvento, DTOEnvioCodigoEmail envio)
        {
            mAppInscricao.EnviarCodigoEmail(idEvento, envio, envio.Email);
        }

        [HttpPost("criar-infantil/{idEvento}")]
        public void CriarInscricaoInfantil(int idEvento, [FromBody] DTOInscricaoAtualizacaoInfantil dadosInscricao)
        {
            mAppInscricao.CriarInscricaoInfantil(idEvento, dadosInscricao);
        }
    }   

    public class DTOValidacaoCodigoEmail
    {
        public string Identificacao { get; set; }
        public string Codigo { get; set; }
    }
}