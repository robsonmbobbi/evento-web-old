using System;
using System.Collections.Generic;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioEventosNH: AEventos
    {
        private ISession mSessao;

        public RepositorioEventosNH(ISession sessao): base(new PersistenciaNH<Evento>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Evento> ObterTodosEventos()
        {
            return mSessao.QueryOver<Evento>().List();
        }

        public override Evento ObterEventoPeloId(int id)
        {
            return mSessao.Get<Evento>(id);
        }

        public override IList<Evento> ObterTodosEventosEmPeriodoInscricaoOnline(DateTime data)
        {
            return mSessao
                .QueryOver<Evento>()
                .Where(x => x.PeriodoInscricaoOnLine.DataInicial <= data && x.PeriodoInscricaoOnLine.DataFinal >= data)
                .List();
        }
    }
}
