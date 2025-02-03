using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using Scriban;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class GeracaoMensagemSand : AGeracaoMensagem
    {       
        public override string GerarMensagemModelo<T>(string modeloMensagem, T objetoDados)
        {
            var template = Template.Parse(modeloMensagem);
            return template.Render(objetoDados);
        }
    }
}
