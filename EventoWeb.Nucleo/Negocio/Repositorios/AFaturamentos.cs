using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System.Collections.Generic;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AFaturamentos : ARepositorio<Faturamento>
    {
        public AFaturamentos(IPersistencia<Faturamento> persistencia) : base(persistencia)
        {
        }

        public abstract Faturamento ObterPorId(int id);

        public abstract IList<FaturamentoCompra> ListarCompras(string descricao, DateTime dataInicio, DateTime dataFim);
        public abstract IList<FaturamentoDoacao> ListarDoacoes(string descricao, DateTime dataInicio, DateTime dataFim);
        public abstract IList<FaturamentoInscricao> ListarFaturamentoInscricoes(string descricao, DateTime dataInicio, DateTime dataFim);

        public override void Excluir(Faturamento objeto)
        {
            if (objeto.Faturado)
                throw new ExcecaoNegocioRepositorio("AFaturamentos", "Nâo é possível excluir um faturamento já fechado.");

            base.Excluir(objeto);
        }
    }
}
