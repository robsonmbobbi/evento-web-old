using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppEstatisticasEvento : AppBase
    {
        private readonly AInscricoes m_RepInscricoes;

        public AppEstatisticasEvento(IContexto contexto, AInscricoes repInscricoes) : base(contexto)
        {
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppEstatisticasEvento", "repInscricoes não pode ser nulo");
        }

        public EstatisticaGeral Gerar(int idEvento)
        {
            EstatisticaGeral estatistica = null;
            ExecutarSeguramente(() =>
            {
                var servico = new ServicoEstatisticas(m_RepInscricoes, idEvento);
                estatistica = servico.GerarEstatisticas();
            });
            return estatistica;
        }
    }
}
