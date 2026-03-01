using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioCodigosAcessoInscricaoNH : ACodigosAcessoInscricao
    {
        private readonly ISession mSessao;

        public RepositorioCodigosAcessoInscricaoNH(ISession sessao)
            : base(new PersistenciaNH<CodigoAcessoInscricao>(sessao))
        {
            mSessao = sessao;
        }

        public override void ExcluirCodigosVencidos()
        {
            mSessao
                .Query<CodigoAcessoInscricao>()
                .Where(x => x.DataHoraValidade < DateTime.Now)
                .Delete();
        }

        public override CodigoAcessoInscricao ObterIdInscricao(int idInscricao)
        {
            return mSessao
                .QueryOver<CodigoAcessoInscricao>()
                .Where(x => x.Inscricao.Id == idInscricao)
                .SingleOrDefault();
        }

        public override CodigoAcessoInscricao ObterPeloCodigo(string codigo)
        {
            return mSessao
                .QueryOver<CodigoAcessoInscricao>()
                .Where(x => x.Codigo == codigo)
                .SingleOrDefault();
        }
    }
}
