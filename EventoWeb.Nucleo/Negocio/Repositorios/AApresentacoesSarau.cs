using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AApresentacoesSarau : ARepositorio<ApresentacaoSarau>
    {
        public AApresentacoesSarau(IPersistencia<ApresentacaoSarau> persistencia) : base(persistencia)
        {
        }

        public abstract IList<ApresentacaoSarau> ListarTodas(int idEvento);
        public abstract ApresentacaoSarau ObterPorId(int idEvento, int id);
        public abstract IList<ApresentacaoSarau> ListarPorInscricao(int idInscricao);
        protected abstract int ObterTempoTotalApresentacoes(Evento evento, ApresentacaoSarau apresentacaoNaoConsiderar = null);

        public override void Incluir(ApresentacaoSarau objeto)
        {
            ValidarTempoApresentacoes(objeto);
            base.Incluir(objeto);
        }

        public override void Atualizar(ApresentacaoSarau objeto)
        {
            ValidarTempoApresentacoes(objeto);
            base.Atualizar(objeto);
        }

        public void ValidarTempoApresentacoes(ApresentacaoSarau apresentacao)
        {
            if (ObterTempoTotalApresentacoes(apresentacao.Evento, apresentacao) + apresentacao.DuracaoMin >
                  apresentacao.Evento.ConfiguracaoTempoSarauMin.Value)
                throw new ExcecaoNegocioRepositorio("AApresentacoesSarau", "A soma do tempo de todas as apresentações, inclusive com esta, ultrapassa o tempo definido para o evento.");
        }
    }
}
