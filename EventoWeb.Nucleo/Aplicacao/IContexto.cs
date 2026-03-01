using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IContexto : IDisposable, IRelatorios
    {
        void IniciarTransacao();

        void SalvarTransacao();

        void CancelarTransacao();

        AEventos RepositorioEventos { get; }
        AUsuarios RepositorioUsuarios { get; }
        ADepartamentos RepositorioDepartamentos { get; }
        ASalasEstudo RepositorioSalasEstudo { get; }
        AOficinas RepositorioOficinas { get; }
        AInscricoes RepositorioInscricoes { get; }
        AConfiguracoesEmail RepositorioConfiguracoesEmail { get; }
        AConfiguracoesWhatsapp RepositorioConfiguracoesWhatsapp { get; }
        AMensagensEmailPadrao RepositorioMensagensEmailPadrao { get; }
        AMensagensWhatsappPadrao RepositorioMensagensWhatsappPadrao { get; }
        ACodigosAcessoInscricao RepositorioCodigosAcessoInscricao { get; }
        AApresentacoesSarau RepositorioApresentacoesSarau { get; }
        AArquivosBinarios RepositorioArquivosBinarios { get; }
        AQuartos RepositorioQuartos { get; }
        AContratosInscricao RepositorioContratosInscricao { get; }

        IServicoGeradorCodigoSeguro ServicoGeradorCodigoSeguro { get; }

        ATitulos RepositorioTitulosFinanceiros { get; }
        ATransacoes RepositorioTransacoesFinanceiras { get; }
        AContas RepositorioContasBancarias { get; }
        AFaturamentos RepositorioFaturamentos { get; }        
    }
}
