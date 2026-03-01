using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia
{
    public class RepositorioDepartamentosNH: PersistenciaNH<Departamento>, ADepartamentos
    {
        private ISession mSessao;

        public RepositorioDepartamentosNH(ISession sessao)
            : base(sessao)
        {
            mSessao = sessao;
        }

        public IList<Departamento> ListarTodosPorEvento(int idEvento)
        {
            return mSessao
                .QueryOver<Departamento>()
                .Where(x => x.Evento.Id == idEvento)
                .List();
        }

        public Departamento ObterPorId(int id)
        {
            return mSessao
                .QueryOver<Departamento>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }
    }
}
