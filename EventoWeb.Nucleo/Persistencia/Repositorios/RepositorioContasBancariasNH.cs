using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioContasBancariasNH : AContas
    {
        private ISession mSessao;
        public RepositorioContasBancariasNH(ISession sessao)
            : base(new PersistenciaNH<Conta>(sessao))
        {
            mSessao = sessao;
        }

        public override bool HaMovimentacao(Conta conta)
        {
            return mSessao
                .QueryOver<Transacao>()
                .Where(x => x.QualConta.Id == conta.Id)
                .Take(1)
                .SingleOrDefault() != null;
        }

        public override IList<Conta> ListarTodos(int idEvento)
        {
            return mSessao
                .QueryOver<Conta>()
                .Where(x=>x.QualEvento.Id == idEvento)
                .List();
        }

        public override Conta ObterPorId(int id)
        {
            return mSessao.Get<Conta>(id);
        }
    }
}
