using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using RazorLight;
using System.Reflection;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class GeracaoMensagemEmailRazor : AGeracaoMensagem
    {
        private RazorLightEngine m_MotorRazor;

        public GeracaoMensagemEmailRazor()
        {
            m_MotorRazor = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
        }

        public override string GerarMensagemModelo<T>(string modeloMensagem, T objetoDados)
        {
            return m_MotorRazor.CompileRenderStringAsync("MODELO", modeloMensagem, objetoDados).Result;
        }
    }
}
