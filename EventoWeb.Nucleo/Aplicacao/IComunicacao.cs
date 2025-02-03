using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IComunicacao
    {
        void EnviarCodigoValidacao(int idEvento, string destinatario, string codigo);
        void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo);
        void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao);
        void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao);
        void EnviarInscricaoAceita(Inscricao inscricao);
        void EnviarInscricaoRejeitada(Inscricao inscricao);
    }
}
