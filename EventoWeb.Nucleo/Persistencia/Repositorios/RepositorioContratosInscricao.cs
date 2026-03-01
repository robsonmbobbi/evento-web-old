using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioContratosInscricao : AContratosInscricao
    {
        private readonly ISession mSessao;

        public RepositorioContratosInscricao(ISession sessao) : base(new PersistenciaNH<ContratoInscricao>(sessao))
        {
            mSessao = sessao;
        }

        public override ContratoInscricao ObterPorEvento(int idEvento)
        {
            return mSessao
                .QueryOver<ContratoInscricao>()
                .Where(x => x.Evento.Id == idEvento)
                .SingleOrDefault();
        }
    }
}
