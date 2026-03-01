using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class CodigoAcessoInscricao: Entidade
    {
        public CodigoAcessoInscricao(string codigo, Inscricao inscricao, DateTime dataHoraValidade)
            : this(codigo, dataHoraValidade)
        {
            Inscricao = inscricao ?? throw new ExcecaoNegocioAtributo("CodigoAcessoInscricao", "inscricao", "inscricao não pode ser nula");
        }

        public CodigoAcessoInscricao(string codigo, string identificacao, DateTime dataHoraValidade)
            : this(codigo, dataHoraValidade)
        {
            if (string.IsNullOrEmpty(identificacao))
                throw new ExcecaoNegocioAtributo("CodigoAcessoInscricao", "identificacao", "identificacao não pode ser nula ou vazia");

            Identificacao = identificacao;
        }

        private CodigoAcessoInscricao(string codigo, DateTime dataHoraValidade)
        {
            if (string.IsNullOrEmpty(codigo))
                throw new ExcecaoNegocioAtributo("CodigoAcessoInscricao", "codigo", "codigo não pode ser nulo ou vazio");

            Codigo = codigo;
            DataHoraValidade = dataHoraValidade;
        }

        protected CodigoAcessoInscricao() { }

        public virtual Inscricao Inscricao { get; protected set; }
        public virtual string Identificacao { get; protected set; }
        public virtual string Codigo { get; protected set; }
        public virtual DateTime DataHoraValidade { get; protected set; }
    }
}
