using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioTitulosFinanceirosNH : ATitulos
    {

        private ISession mSessao;
        public RepositorioTitulosFinanceirosNH(ISession sessao)
            : base(new PersistenciaNH<Titulo>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Titulo> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, String descricao, EnumTipoTransacao tipo)
        {
            return mSessao
                .QueryOver<Titulo>()
                .Where(x => x.Evento.Id == idEvento
                           && x.DataVencimento.IsBetween(dataInicial).And(dataFinal)
                           && x.Tipo == tipo
                           && x.Descricao.Upper().IsLike(descricao.ToUpper(), MatchMode.Anywhere))
                .List();
        }

        public override Titulo ObterPorId(int id)
        {
            return mSessao
                .QueryOver<Titulo>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }
    }
}
