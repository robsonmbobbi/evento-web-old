using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface ASituacoesConfirmacaoInscricao : IPersistencia<SituacaoConfirmacaoInscricao>
    {
        IList<SituacaoConfirmacaoInscricao> ListarSituacoesPorIdInscricao(int idInscricao);
        void ExcluirTodasPorInscricao(int idInsricao);
    }
}
