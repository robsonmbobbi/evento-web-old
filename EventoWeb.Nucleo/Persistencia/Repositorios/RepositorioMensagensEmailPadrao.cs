using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioMensagensEmailPadrao : AMensagensEmailPadrao
    {
        private readonly ISession mSessao;

        public RepositorioMensagensEmailPadrao(ISession sessao) : base(new PersistenciaNH<MensagemEmailPadrao>(sessao))
        {
            mSessao = sessao;
        }

        public override MensagemEmailPadrao Obter(int idEvento)
        {
            return mSessao
                .QueryOver<MensagemEmailPadrao>()
                .Where(x => x.Evento.Id == idEvento)
                .SingleOrDefault();
        }
    }
}
