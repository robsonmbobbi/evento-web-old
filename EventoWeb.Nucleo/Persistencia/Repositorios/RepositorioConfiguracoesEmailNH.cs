using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioConfiguracoesEmailNH : AConfiguracoesEmail
    {
        private ISession mSessao;

        public RepositorioConfiguracoesEmailNH(ISession sessao) : base(new PersistenciaNH<ConfiguracaoEmail>(sessao))
        {
            mSessao = sessao;
        }

        public override ConfiguracaoEmail Obter(int idEvento)
        {
            var lista = mSessao
                .QueryOver<ConfiguracaoEmail>()
                .Where(x => x.Evento.Id == idEvento)
                .List();

            if (lista.Count == 0)
                return null;
            else
                return lista[0];
        }
    }
}
