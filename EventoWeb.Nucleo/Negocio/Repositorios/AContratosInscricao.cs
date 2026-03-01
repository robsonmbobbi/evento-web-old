using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AContratosInscricao : ARepositorio<ContratoInscricao>
    {
        public AContratosInscricao(IPersistencia<ContratoInscricao> persistencia) : base(persistencia) { }

        public abstract ContratoInscricao ObterPorEvento(int idEvento);

        public override void Incluir(ContratoInscricao objeto)
        {
            if (ObterPorEvento(objeto.Evento.Id) != null)
                throw new ExcecaoNegocioRepositorio("AContratosInscricao", "Não é possível incluir mais de um contrato por evento");

            base.Incluir(objeto);
        }
    }
}
