using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class TotalTransacoes
    {
        public EnumTipoTransacao Tipo { get; set; }
        public Decimal Valor { get; set; }
    }

    public class RepositorioTransacoesFinanceirasNH : ATransacoes
    {
        private readonly ISession mSessao;

        public RepositorioTransacoesFinanceirasNH(ISession sessao)
            : base(new PersistenciaNH<Transacao>(sessao))
        {
            mSessao = sessao;
        }

        public override decimal ObterTotalTransacoesPorData(int idConta, DateTime data)
        {
            data = data
                .AddHours(23 - data.Hour)
                .AddMinutes(59 - data.Minute)
                .AddSeconds(59 - data.Second);
            TotalTransacoes alias = null;
            var lista = mSessao
                .QueryOver<Transacao>()
                .Where(x => x.QualConta.Id == idConta && x.DataHora <= data)
                .Select(Projections.Group<Transacao>(x => x.Tipo).WithAlias(()=>alias.Tipo),
                        Projections.Sum<Transacao>(x => x.Valor).WithAlias(() => alias.Valor))
                .TransformUsing(Transformers.AliasToBean<TotalTransacoes>())
                .List<TotalTransacoes>();

            if (lista.Count == 0)
                return 0;
            else if (lista.Count == 1)
                return lista[0].Valor * (lista[0].Tipo == EnumTipoTransacao.Despesa ? -1 : 1);
            else
            {
                Decimal valor = 0;
                valor += lista[0].Valor * (lista[0].Tipo == EnumTipoTransacao.Despesa ? -1 : 1);
                valor += lista[1].Valor * (lista[1].Tipo == EnumTipoTransacao.Despesa ? -1 : 1);

                return valor;
            }
        }


        public override IList<Transacao> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, int idConta)
        {
            dataInicial = dataInicial
                .AddHours(-1 * dataInicial.Hour)
                .AddMinutes(-1 * dataInicial.Minute)
                .AddSeconds(-1 * dataInicial.Second);
            dataFinal = dataFinal
                .AddHours(23 - dataFinal.Hour)
                .AddMinutes(59 - dataFinal.Minute)
                .AddSeconds(59 - dataFinal.Second);

            var consulta = mSessao
                .QueryOver<Transacao>()
                .Where(x => x.QualEvento.Id == idEvento && x.DataHora.IsBetween(dataInicial).And(dataFinal));

            if (idConta > 0)
                consulta
                    .Where(x => x.QualConta.Id == idConta);

            consulta
                .Where(x => x.Tipo == EnumTipoTransacao.Despesa ||
                x.Tipo == EnumTipoTransacao.Receita);

            consulta
                .JoinQueryOver(x => x.QualConta);
                
            return consulta.List();
        }

        public override DateTime? ObterDataUltimaTransacaoDaConta(int idConta)
        {
            return mSessao
                .QueryOver<Transacao>()
                .Where(x => x.QualConta.Id == idConta)
                .Select(Projections.Max<Transacao>(x => x.DataHora))
                .SingleOrDefault<DateTime?>();
        }

        public override Transacao ObterPorId(int id)
        {
            return mSessao
                .QueryOver<Transacao>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }
    }
}
