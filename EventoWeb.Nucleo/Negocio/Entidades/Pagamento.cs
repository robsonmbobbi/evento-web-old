using EventoWeb.Nucleo.Negocio.Excecoes;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumPagamento { Comprovante, ComprovanteOutraInscricao, Outro }

    public class Pagamento
    {
        private EnumPagamento? m_Forma;
        private IList<ComprovantePagamento> m_Comprovantes;
        private Inscricao m_Inscricao;

        public Pagamento(Inscricao inscricao)
        {
            m_Inscricao = inscricao ?? throw new ExcecaoNegocioAtributo("Pagamento", "Inscricao", "Inscrição é obrigatório");
            m_Comprovantes = new List<ComprovantePagamento>();
        }

        protected Pagamento() { }

        public virtual Inscricao Inscricao { get => m_Inscricao; }
        public virtual EnumPagamento? Forma { get => m_Forma; }
        public virtual IEnumerable<ComprovantePagamento> Comprovantes { get => m_Comprovantes; }
        public virtual string Observacao { get; set; }

        public virtual void AtribuirFormaPagamento(EnumPagamento forma, IEnumerable<ArquivoBinario> comprovantes)
        {
            if (forma != EnumPagamento.Comprovante && comprovantes != null && comprovantes.Count() > 0)
                throw new ExcecaoNegocioAtributo("Pagamento", "Forma", "Apenas a forma de pagamento por comprovante pode receber comprovantes");

            if (forma == EnumPagamento.Comprovante && (comprovantes == null || comprovantes.Count() == 0))
                throw new ExcecaoNegocioAtributo("Pagamento", "Forma", "A forma de pagamento por comprovante precisa de comprovantes");

            m_Forma = forma;
            m_Comprovantes.Clear();
            if (comprovantes != null)
            {
                foreach (var item in comprovantes)
                    m_Comprovantes.Add(new ComprovantePagamento(m_Inscricao, item));
            }
        }
    }

    public class ComprovantePagamento : Entidade
    {
        private ArquivoBinario m_ArquivoComprovante;
        private Inscricao m_Inscricao;

        public ComprovantePagamento(Inscricao inscricao, ArquivoBinario arquivoComprovante)
        {
            m_Inscricao = inscricao ?? throw new ExcecaoNegocioAtributo("ComprovantePagamento", "Inscricao", "Inscrição é obrigatório");
            m_ArquivoComprovante = arquivoComprovante ?? throw new ExcecaoNegocioAtributo("ComprovantePagamento", "ArquivoComprovante", "ArquivoComprovante é obrigatório");
        }
        protected ComprovantePagamento() { }

        public virtual Inscricao Inscricao { get => m_Inscricao; }
        public virtual ArquivoBinario ArquivoComprovante { get => m_ArquivoComprovante; }
    }
}
