using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumValorAvaliacao { Otimo, MuitoBom, Regular, Ruim }

    public abstract class Avaliacao: Entidade
    {
        protected EnumValorAvaliacao mValor;
        private Evento mEvento;

        public Avaliacao(Evento evento,  EnumValorAvaliacao valor)
        {
            Evento = evento;
            Valor = valor;
        }

        protected Avaliacao() { }

        public virtual Evento Evento
        {
            get { return mEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                mEvento = value;
            }
        }
       
        public virtual EnumValorAvaliacao Valor { get => mValor; set => mValor = value; }
    }

    public enum EnumAreaAvaliada { Recepcao, Secretaria, Musica, Cozinha, Integracao, PrimeiraPalestra,
                                   SegundaPalestra, Limpeza, CoordenacaoGeral, Sarau, Evangelizacao,
                                   AtendimentoFraterno, AtracaoArtistica, Ornamentacao }

    public class AvaliacaoGeral: Avaliacao
    {
        private EnumAreaAvaliada mQual;

        public AvaliacaoGeral(Evento evento, EnumAreaAvaliada qual, EnumValorAvaliacao valor)
            :base(evento, valor)
        {
            mQual = qual;
        }

        protected AvaliacaoGeral() { }

        public virtual EnumAreaAvaliada Qual { get => mQual; set => mQual = value; }
    }

    public abstract class AvaliacaoAtividade<TAtividade>: Avaliacao where TAtividade: Entidade
    {
        private TAtividade mQual;

        public AvaliacaoAtividade(Evento evento, TAtividade qual, EnumValorAvaliacao valor)
            : base(evento, valor)
        {
            mQual = qual;
        }

        protected AvaliacaoAtividade() { }

        public virtual TAtividade Qual
        {
            get => mQual;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Qual", "A oficina não pode ser nula.");

                mQual = value;
            }
        }
    }

    public class AvaliacaoOficina : AvaliacaoAtividade<Oficina>
    {
        public AvaliacaoOficina(Evento evento, Oficina qual, EnumValorAvaliacao valor)
            : base(evento, qual, valor)
        {
        }

        protected AvaliacaoOficina() { }
    }

    public class AvaliacaoSalaEstudo : AvaliacaoAtividade<SalaEstudo>
    {
        public AvaliacaoSalaEstudo(Evento evento, SalaEstudo qual, EnumValorAvaliacao valor)
            : base(evento, qual, valor)
        {
        }

        protected AvaliacaoSalaEstudo() { }
    }

    public class AvaliacaoDepartamento : AvaliacaoAtividade<Departamento>
    {
        public AvaliacaoDepartamento(Evento evento, Departamento qual, EnumValorAvaliacao valor)
            : base(evento, qual, valor)
        {
        }

        protected AvaliacaoDepartamento() { }
    }
}
