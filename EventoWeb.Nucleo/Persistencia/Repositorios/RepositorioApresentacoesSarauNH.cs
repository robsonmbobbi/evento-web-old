using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioApresentacoesSarauNH : AApresentacoesSarau
    {
        private readonly ISession mSessao;

        public RepositorioApresentacoesSarauNH(ISession sessao) : base(new PersistenciaNH<ApresentacaoSarau>(sessao))        
        {
            mSessao = sessao;
        }

        public override IList<ApresentacaoSarau> ListarPorInscricao(int idInscricao)
        {
            ApresentacaoSarau apresentacaoAlias = null;

            return mSessao
                .QueryOver<ApresentacaoSarau>(() => apresentacaoAlias)
                .WithSubquery
                    .WhereExists(QueryOver.Of<ApresentacaoSarau>()
                                    .Where(a=>a.Id == apresentacaoAlias.Id)
                                    .Inner.JoinQueryOver<Inscricao>(a=>a.Inscritos)
                                        .Where(i => i.Id == idInscricao)
                                    .Select(a=>a.Id))
                .Inner.JoinQueryOver<Inscricao>(x => x.Inscritos)
                .Inner.JoinQueryOver<Pessoa>(x => x.Pessoa)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List();
        }

        public override IList<ApresentacaoSarau> ListarTodas(int idEvento)
        {
            var consulta = mSessao
                .QueryOver<ApresentacaoSarau>()
                .Where(x => x.Evento.Id == idEvento);

            consulta
                .Inner.JoinQueryOver<Inscricao>(x => x.Inscritos)
                .Inner.JoinQueryOver<Pessoa>(x => x.Pessoa);

            return consulta
                .TransformUsing(Transformers.DistinctRootEntity)
                .List();
        }

        public override ApresentacaoSarau ObterPorId(int idEvento, int id)
        {
            var consulta = mSessao
                .QueryOver<ApresentacaoSarau>()
                .Where(x => x.Evento.Id == idEvento && x.Id == id);

            consulta
                .JoinQueryOver<Inscricao>(x => x.Inscritos)
                .JoinQueryOver<Pessoa>(x => x.Pessoa);

            return consulta
                .TransformUsing(Transformers.DistinctRootEntity)
                .SingleOrDefault();
        }

        protected override int ObterTempoTotalApresentacoes(Evento evento, ApresentacaoSarau apresentacaoNaoConsiderar = null)
        {
            var consulta = mSessao.QueryOver<ApresentacaoSarau>()
                .Where(apresentacao => apresentacao.Evento.Id == evento.Id);

            if (apresentacaoNaoConsiderar != null)
                consulta.Where(apresentacao => apresentacao.Id != apresentacaoNaoConsiderar.Id);

            return consulta
                .Select(Projections.Sum<ApresentacaoSarau>(x => x.DuracaoMin))
                .SingleOrDefault<int>();
        }
    }
}
