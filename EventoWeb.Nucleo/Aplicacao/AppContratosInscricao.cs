using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppContratosInscricao : AppBase
    {
        public AppContratosInscricao(IContexto contexto) : base(contexto)
        {
        }

        public DTOContratoInscricao ObterPorEvento(int idEvento)
        {
            DTOContratoInscricao dto = null;
            ExecutarSeguramente(() =>
            {
                var contrato = Contexto.RepositorioContratosInscricao.ObterPorEvento(idEvento);

                if (contrato != null)
                    dto = contrato.Converter();
            });

            return dto;
        }

        public DTOId Incluir(int idEvento, DTOContratoInscricao dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var contrato = new ContratoInscricao(evento, dto.Regulamento, dto.InstrucoesPagamento, dto.PassoAPassoInscricao);

                Contexto.RepositorioContratosInscricao.Incluir(contrato);
                retorno.Id = contrato.Id;
            });

            return retorno;
        }

        public void Atualizar(int idEvento, DTOContratoInscricao dto)
        {
            ExecutarSeguramente(() =>
            {
                var contrato = ObterContratoOuExcecaoSeNaoEncontrar(idEvento);
                contrato.InstrucoesPagamento = dto.InstrucoesPagamento;
                contrato.PassoAPassoInscricao= dto.PassoAPassoInscricao;
                contrato.Regulamento = dto.Regulamento;

                Contexto.RepositorioContratosInscricao.Atualizar(contrato);
            });
        }

        public void Excluir(int idEvento)
        {
            ExecutarSeguramente(() =>
            {
                var contrato = ObterContratoOuExcecaoSeNaoEncontrar(idEvento);

                Contexto.RepositorioContratosInscricao.Excluir(contrato);
            });
        }

        private ContratoInscricao ObterContratoOuExcecaoSeNaoEncontrar(int idEvento)
        {
            var contrato = Contexto.RepositorioContratosInscricao.ObterPorEvento(idEvento);

            if (contrato != null)
                return contrato;
            else
                throw new ExcecaoAplicacao("AppContratosInscricao", "Não foi encontrado nenhum contrato com o id informado.");
        }
    }
}
