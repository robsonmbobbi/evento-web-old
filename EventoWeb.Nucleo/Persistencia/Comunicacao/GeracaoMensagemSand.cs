using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using Scriban;
using Scriban.Runtime;
using System;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class GeracaoMensagemSand : AGeracaoMensagem
    {       
        public override string GerarMensagemModelo<T>(string modeloMensagem, T objetoDados)
        {
            var functions = new TemplateFunctionsScriban();
            functions.Import(objetoDados);
            
            var context = new TemplateContext();
            context.PushGlobal(functions);
            
            var template = Scriban.Template.Parse(modeloMensagem);            
            return template.Render(context);
        }
    }

    public class TemplateFunctionsScriban : ScriptObject
    {
        public static string FormatPhone(string phone)
        {
            return Convert.ToUInt64(phone).ToString(@"(00) 0 0000-0000");
        }
    }
}
