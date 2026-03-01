using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class ATransacoes
    {
        public ATransacoes(IPersistencia<Transacao> persistencia)
        {
            Persistencia = persistencia?? throw new ExcecaoNegocioRepositorio("ATransacoes", "persistencia não poder ser nula");
        }

        protected IPersistencia<Transacao> Persistencia { get; }

        public abstract decimal ObterTotalTransacoesPorData(int idConta, DateTime data);
        public abstract IList<Transacao> ListarTodos(int idEvento, DateTime dataInicial, DateTime dataFinal, int idConta);
        public abstract DateTime? ObterDataUltimaTransacaoDaConta(int idConta);
        public abstract Transacao ObterPorId(int id);

        public virtual void Incluir(Transacao objeto)
        {
            Persistencia.Incluir(objeto);
        }
    }
}
