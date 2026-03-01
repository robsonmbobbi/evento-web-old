using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOEnvioCodigoEmail
    {
        public string Identificacao { get; set; }
        public string Email { get; set; }
        public string Whatsapp { get; set; }
    }

    public interface IComunicacao
    {
        void EnviarCodigoValidacao(int idEvento, DTOEnvioCodigoEmail dadosEnvio, string codigo);
        void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo);
        void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao);
        void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao);
        void EnviarInscricaoAceita(Inscricao inscricao);
        void EnviarInscricaoRejeitada(Inscricao inscricao);
    }
}
