using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppComunicacao : IComunicacao
    {
        private IList<IComunicacao> m_Comunicadores;

        public AppComunicacao(IList<IComunicacao> comunicadores)
        {
            m_Comunicadores = comunicadores;
        }

        public void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarCodigoAcompanhamentoInscricao(inscricao, codigo);
        }

        public void EnviarCodigoValidacao(int idEvento, DTOEnvioCodigoEmail dadosEnvio, string codigo)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarCodigoValidacao(idEvento, dadosEnvio, codigo);
        }

        public void EnviarInscricaoAceita(Inscricao inscricao)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarInscricaoAceita(inscricao);
        }

        public void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarInscricaoRegistradaAdulto(inscricao);
        }

        public void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarInscricaoRegistradaInfantil(inscricao);
        }

        public void EnviarInscricaoRejeitada(Inscricao inscricao)
        {
            foreach (var comunicador in m_Comunicadores)
                comunicador.EnviarInscricaoRejeitada(inscricao);
        }
    }
}
