using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosInscricaoController : ControllerBase
    {
        private readonly AppContratosInscricao mAppContratos;

        public ContratosInscricaoController(IContexto contexto)
        {
            mAppContratos = new AppContratosInscricao(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter")]
        public DTOContratoInscricao GetObter(int idEvento)
        {
            return mAppContratos.ObterPorEvento(idEvento);
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("evento/{idEvento}/criar")]
        public DTOId IncluirSala(int idEvento, [FromBody] DTOContratoInscricao dto)
        {
            var id = mAppContratos.Incluir(idEvento, dto);
            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar")]
        public void AlterarSala(int idEvento, int idSala, [FromBody] DTOContratoInscricao dto)
        {
            mAppContratos.Atualizar(idEvento, dto);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir")]
        public void ExcluirSala(int idEvento)
        {
            mAppContratos.Excluir(idEvento);
        }
    }
}
