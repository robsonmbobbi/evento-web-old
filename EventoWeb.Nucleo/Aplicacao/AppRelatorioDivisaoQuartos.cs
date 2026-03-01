using EventoWeb.Nucleo.Negocio.Repositorios;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppRelatorioDivisaoQuartos : AppBase
    {
        private readonly IRelatorioDivisaoQuartos m_GeradorRelDivisaoQuartos;
        private readonly AQuartos m_RepQuartos;


        public AppRelatorioDivisaoQuartos(IContexto contexto, AQuartos repQuartos,
            IRelatorioDivisaoQuartos geradorRelDivisaoQuartos) : base(contexto)
        {
            m_RepQuartos = repQuartos ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoQuartos", "repQuartos não pode ser nulo");
            m_GeradorRelDivisaoQuartos = geradorRelDivisaoQuartos ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoQuartos", "geradorRelDivisaoQuartos não pode ser nulo");
        }

        public Stream GerarImpressoPDF(int idEvento)
        {
            Stream relatorio = new MemoryStream();
            ExecutarSeguramente(() =>
            {
                var quartos = m_RepQuartos.ListarTodosQuartosPorEvento(idEvento);
                
                relatorio = m_GeradorRelDivisaoQuartos.Gerar(quartos);
            });

            return relatorio;
        }
    }
}
