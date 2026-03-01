using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppRelatorioDivisaoOficinas : AppBase
    {
        private readonly IRelatorioDivisaoOficinas m_GeradorRelDivisaoOficinas;
        private readonly AOficinas m_RepOficinas;
        private readonly AInscricoes m_RepInscricoes;
        private readonly AEventos m_RepEventos;


        public AppRelatorioDivisaoOficinas(IContexto contexto, AEventos repEventos, AOficinas repOficinas,
            AInscricoes repInscricoes, IRelatorioDivisaoOficinas geradorRelDivisaoOficinas) : base(contexto)
        {
            m_RepEventos = repEventos ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoOficinas", "repEventos não pode ser nulo");
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoOficinas", "repInscricoes não pode ser nulo");
            m_RepOficinas = repOficinas ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoOficinas", "repOficinas não pode ser nulo");
            m_GeradorRelDivisaoOficinas = geradorRelDivisaoOficinas ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoOficinas", "geradorRelDivisaoOficinas não pode ser nulo");
        }

        public Stream GerarImpressoPDF(int idEvento)
        {
            Stream relatorio = new MemoryStream();
            ExecutarSeguramente(() =>
            {
                var evento = m_RepEventos.ObterEventoPeloId(idEvento);
                var salas = m_RepOficinas.ListarTodasComParticipantesPorEvento(evento);
                var atividadesOficinasCoordenadores = m_RepInscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoOficinasCoordenacao>(evento);

                relatorio = m_GeradorRelDivisaoOficinas.Gerar(salas, atividadesOficinasCoordenadores);
            });

            return relatorio;
        }
    }
}
