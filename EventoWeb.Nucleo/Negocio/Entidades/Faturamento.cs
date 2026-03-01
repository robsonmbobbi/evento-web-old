using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class Faturamento : EntidadeFinanceira
    {
        private string m_Descricao;
        private decimal m_ValorBruto;

        public Faturamento(Evento evento, string descricao, decimal valorBruto, EnumTipoTransacao tipo) : 
            base(evento, tipo)
        {
            Faturado = false;
            RemoverDesconto();

            Descricao = descricao;
            ValorBruto = valorBruto;
            Data = DateTime.Now;
        }

        public virtual String Descricao
        {
            get { return m_Descricao; }
            set
            {
                ValidarSePodeAlterar();

                if (String.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo("Faturamento", "Descricao", "A descrição precisa ser informada.");

                m_Descricao = value;
            }
        }

        public virtual Decimal ValorBruto
        {
            get { return m_ValorBruto; }
            set
            {
                ValidarSePodeAlterar();

                if (value <= 0)
                    throw new ExcecaoNegocioAtributo("Faturamento", "Valor", "O valor deve ser maior que zero.");

                if (value < ValorDesconto)
                    throw new ExcecaoNegocioAtributo("Faturamento", "Valor", "O Valor de Desconto informado é maior que o novo valor bruto. Remova o desconto para poder atribuir este novo valor");

                m_ValorBruto = value;
            }
        }

        public virtual Decimal ValorDesconto { get; protected set; }

        public virtual string MotivoDesconto { get; protected set; }

        public virtual Decimal ValorLiquido { get => ValorBruto - ValorDesconto; }

        public virtual bool Faturado { get; set; }

        public virtual DateTime Data { get; protected set; }

        public void DarDesconto(decimal valorDesconto, string motivo)
        {
            ValidarSePodeAlterar();

            if (valorDesconto <= 0)
                throw new ExcecaoNegocioAtributo("Faturamento", "valorDesconto", "O valor de desconto deve ser maior que zero.");
            
            if (valorDesconto > ValorBruto)
                throw new ExcecaoNegocioAtributo("Faturamento", "valorDesconto", "O valor de desconto deve ser menor ou igual ao valor da fatura.");

            if (String.IsNullOrWhiteSpace(motivo))
                throw new ExcecaoNegocioAtributo("Faturamento", "motivo", "O motivo do desconto precisa ser informado");

            ValorDesconto = valorDesconto;
            MotivoDesconto = motivo;
        }

        public void RemoverDesconto()
        {
            ValorDesconto = 0;
            MotivoDesconto = "";
        }       

        protected virtual void ValidarSePodeAlterar()
        {
            if (Faturado)
                throw new ExcecaoNegocio("Faturamento", "Não é possível alterar esta fatura");
        }
    }

    public class FaturamentoInscricao : Faturamento
    {
        private IList<Inscricao> m_Inscricoes;

        public FaturamentoInscricao(Evento evento, IEnumerable<Inscricao> inscricoes) : 
            base(evento, "a", 1, EnumTipoTransacao.Receita)
        {
            m_Inscricoes = new List<Inscricao>();
            AlterarInscricoes(inscricoes);
        }

        public virtual IEnumerable<Inscricao> Inscricoes { get => m_Inscricoes; }

        public virtual void AlterarInscricoes(IEnumerable<Inscricao> inscricoes)
        {
            ValidarSePodeAlterar();

            if (inscricoes == null)
                throw new ExcecaoNegocioAtributo("FaturamentoInscricao", "inscricoes", "As inscrições não podem ser nulas.");

            if (inscricoes.Count() == 0)
                throw new ExcecaoNegocioAtributo("FaturamentoInscricao", "inscricoes", "É preciso pelo menos uma inscrição");

            m_Inscricoes.Clear();

            var descricao = "";
            decimal valor = 0;
            foreach (var inscricao in inscricoes)
            {
                if (inscricao.Evento != Evento)
                    throw new ExcecaoNegocioAtributo("FaturamentoInscricao", "inscricoes", "Há inscrições que não são do evento deste faturamento.");

                if (descricao != "")
                    descricao += "\n";
                descricao = $"{descricao}Inscrição de {inscricao.Pessoa.Nome} da cidade {inscricao.Pessoa.Endereco.Cidade}/{inscricao.Pessoa.Endereco.UF}";

                if (inscricao is InscricaoInfantil)
                    valor += Evento.ValorInscricaoCrianca;
                else
                    valor += Evento.ValorInscricaoAdulto;

                m_Inscricoes.Add(inscricao);
            }

            ValorBruto = valor;
            Descricao = descricao;
        }
    }

    public class FaturamentoDoacao : Faturamento
    {
        public FaturamentoDoacao(Evento evento, string descricao, decimal valor) : 
            base(evento, descricao, valor, EnumTipoTransacao.Receita)
        {
        }
    }

    public class FaturamentoCompra : Faturamento
    {
        public FaturamentoCompra(Evento evento, string descricao, decimal valorBruto) :
            base(evento, descricao, valorBruto, EnumTipoTransacao.Despesa)
        {
        }
    }
}
