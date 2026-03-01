using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{

    public abstract class AppInscOnlineIdentificacao
    {

        public AppInscOnlineIdentificacao(string prefixo, int tamanhoId)
        {
            Prefixo = prefixo;
            TamanhoPrefixo = prefixo.Length;
            TamanhoId = tamanhoId;
        }

        public string Prefixo { get; }
        public int TamanhoPrefixo { get; }
        public int TamanhoId { get; }
        public int TamanhoCodigo { get => TamanhoPrefixo + TamanhoId; }

        public int ExtrarId(string codigo)
        {
            if (codigo.Length == TamanhoCodigo &&
                codigo.Substring(0, TamanhoPrefixo) == Prefixo &&
                int.TryParse(codigo.Substring(TamanhoPrefixo, TamanhoId), out int idInscricao))
                return idInscricao;
            else
                throw new ExcecaoAplicacao("AppInscOnlineCodigoIdentificacao", "Código Inválido");
        }

        public string GerarCodigo(int id)
        {
            return Prefixo + id.ToString("0".PadLeft(TamanhoId, '0'));
        }
    }

    public class AppInscOnLineIdentificacaoInscricao : AppInscOnlineIdentificacao
    {
        public AppInscOnLineIdentificacaoInscricao() : base("INSC", 6)
        {
        }
    }

    public class AppInscOnLineIdentificacaoSarau : AppInscOnlineIdentificacao
    {
        public AppInscOnLineIdentificacaoSarau() : base("APSA", 6)
        {
        }
    }
}
