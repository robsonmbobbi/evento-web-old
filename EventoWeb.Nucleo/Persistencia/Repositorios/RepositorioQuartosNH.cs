using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioQuartosNH : AQuartos
    {
        private readonly ISession mSessao;

        public RepositorioQuartosNH(ISession sessao)
            : base(new PersistenciaNH<Quarto>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Quarto> ListarTodosQuartosPorEvento(int idEvento)
        {
            return mSessao.QueryOver<Quarto>()
                .Where(quarto => quarto.Evento.Id == idEvento)
                .List();
        }

        public override Quarto ObterQuartoPorIdEventoEQuarto(int idEvento, int idQuarto)
        {
            return mSessao.QueryOver<Quarto>()
                .Where(quarto => quarto.Evento.Id == idEvento && quarto.Id == idQuarto)
                .SingleOrDefault();
        }

        protected override Boolean HaOutroQuartoComCapacidadeInfinita(Quarto quarto)
        {
            return mSessao.QueryOver<Quarto>()
                .Where(x => x.Id != quarto.Id && x.Sexo == quarto.Sexo && x.EhFamilia == quarto.EhFamilia &&
                    x.Capacidade == null)
                .RowCount() > 0;
        }

        public override IList<Quarto> ListarTodosQuartosPorEventoComParticipantes(int idEvento)
        {
            return mSessao.QueryOver<Quarto>()
                .Where(quarto => quarto.Evento.Id == idEvento)
                .Left.JoinQueryOver(x => x.Inscritos)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List();
        }

        public override Quarto BuscarQuartoDoInscrito(int idEvento, int idInscricao)
        {
            var quartoInscrito = mSessao.QueryOver<QuartoInscrito>()
                .Where(x => x.Inscricao.Id == idInscricao)
                .JoinQueryOver(x => x.Quarto)
                .Where(x => x.Evento.Id == idEvento)
                .SingleOrDefault();

            if (quartoInscrito == null)
                return null;
            else
                return quartoInscrito.Quarto;
        }
    }
}
