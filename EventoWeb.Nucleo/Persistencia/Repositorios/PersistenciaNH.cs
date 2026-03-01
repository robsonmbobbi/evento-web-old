using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class PersistenciaNH<T>: IPersistencia<T>
    {
        private ISession mSessao;
        public PersistenciaNH(ISession sessao)
        {
            mSessao = sessao;
        }

        public void Incluir(T objeto)
        {
            mSessao.Save(objeto);
        }

        public void Excluir(T objeto)
        {
            mSessao.Delete(objeto);
        }

        public void Atualizar(T objeto)
        {
            mSessao.Update(objeto);
        }
    }
}
