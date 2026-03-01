using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatisticasController : ControllerBase
    {
        private readonly AppEstatisticasEvento mAppEstatisticas;

        public EstatisticasController(IContexto contexto)
        {
            mAppEstatisticas = new AppEstatisticasEvento(contexto, contexto.RepositorioInscricoes);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}")]
        public EstatisticaGeral Obter(int idEvento)
        {
            return mAppEstatisticas.Gerar(idEvento);
        }
    }
}