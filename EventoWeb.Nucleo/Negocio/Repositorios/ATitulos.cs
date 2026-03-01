using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class ATitulos
    {
        public ATitulos(IPersistencia<Titulo> persistencia)
        {
            Persistencia = persistencia ?? throw new ExcecaoNegocioRepositorio("ATitulos", "persistencia não poder ser nula");
        }

        protected IPersistencia<Titulo> Persistencia { get; }

        public abstract IList<Titulo> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, String descricao, EnumTipoTransacao tipo);

        public abstract Titulo ObterPorId(int id);

        public virtual void Atualizar(Titulo objeto)
        {
            Persistencia.Atualizar(objeto);
        }
    }
}
