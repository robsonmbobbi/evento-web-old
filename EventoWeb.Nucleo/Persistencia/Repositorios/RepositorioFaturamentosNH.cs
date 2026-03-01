using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioFaturamentosNH : AFaturamentos
    {
        private readonly ISession mSessao;

        public RepositorioFaturamentosNH(ISession sessao) : base(new PersistenciaNH<Faturamento>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<FaturamentoCompra> ListarCompras(string descricao, DateTime dataInicio, DateTime dataFim)
        {
            return CriarConsultaListagem<FaturamentoCompra>(descricao, dataInicio, dataFim)
                .List();
        }

        public override IList<FaturamentoDoacao> ListarDoacoes(string descricao, DateTime dataInicio, DateTime dataFim)
        {
            return CriarConsultaListagem<FaturamentoDoacao>(descricao, dataInicio, dataFim)
                .List();
        }

        public override IList<FaturamentoInscricao> ListarFaturamentoInscricoes(string descricao, DateTime dataInicio, DateTime dataFim)
        {
            return CriarConsultaListagem<FaturamentoInscricao>(descricao, dataInicio, dataFim)
                .List();
        }

        private IQueryOver<T,T> CriarConsultaListagem<T>(string descricao, DateTime dataInicio, DateTime dataFim) where T: Faturamento
        {
            dataInicio = dataInicio
                .AddHours(-1 * dataInicio.Hour)
                .AddMinutes(-1 * dataInicio.Minute)
                .AddSeconds(-1 * dataInicio.Second);
            dataFim = dataFim
                .AddHours(23 - dataFim.Hour)
                .AddMinutes(59 - dataFim.Minute)
                .AddSeconds(59 - dataFim.Second);

            return mSessao
                .QueryOver<T>()
                .Where(x => x.Descricao.Upper().IsLike(descricao.ToUpper(), MatchMode.Anywhere) &&
                    x.Data.IsBetween(dataInicio).And(dataFim));
        }

        public override Faturamento ObterPorId(int id)
        {
            return mSessao
                .QueryOver<Faturamento>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }
    }
}
