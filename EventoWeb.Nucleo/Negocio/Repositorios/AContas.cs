using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AContas: ARepositorio<Conta>
    {
        public AContas(IPersistencia<Conta> persistencia) : base(persistencia)
        {
        }

        public abstract IList<Conta> ListarTodos(int idEvento);

        public abstract Conta ObterPorId(int id);

        public abstract bool HaMovimentacao(Conta conta);

        public override void Excluir(Conta objeto)
        {
            if (HaMovimentacao(objeto))
                throw new ExcecaoNegocioRepositorio("AContas", "Não é possível excluir uma conta com movimentações.");

            base.Excluir(objeto);
        }
    }
}
