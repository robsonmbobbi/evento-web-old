using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetasController : ControllerBase
    {
        private const string TIPO_CONTEUDO_PDF = "application/pdf";

        private readonly AppGeracaoEtiquetaCracha mAppGeracaoCracha;
        private readonly AppGeracaoEtiquetaCaderno mAppGeracaoCaderno;
        private readonly AppListagemDadosEtiquetas mAppListagem;

        public EtiquetasController(IContexto contexto)
        {
            mAppListagem = new AppListagemDadosEtiquetas(contexto);
            mAppGeracaoCracha = new AppGeracaoEtiquetaCracha();
            mAppGeracaoCaderno = new AppGeracaoEtiquetaCaderno();
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listagem/filtro/{filtro}")]
        public IList<CrachaInscrito> Listar(int idEvento, EnumFiltroCracha filtro)
        {
            return mAppListagem.Listar(idEvento, filtro);
        }

        [Authorize("Bearer")]
        [HttpPut("geracao-cracha")]
        public IActionResult GerarCracha(IList<CrachaInscrito> inscritos)
        {
            return File(mAppGeracaoCracha.Gerar(inscritos), TIPO_CONTEUDO_PDF);
        }

        [Authorize("Bearer")]
        [HttpPut("geracao-caderno")]
        public IActionResult GerarCaderno(IList<CrachaInscrito> inscritos)
        {
            return File(mAppGeracaoCaderno.Gerar(inscritos), TIPO_CONTEUDO_PDF);
        }
    }
}