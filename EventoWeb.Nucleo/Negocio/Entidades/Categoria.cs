using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Categoria : Entidade
    {
        private string m_Descricao;
        private Categoria m_CategoriaPai;
        private Evento m_QualEvento;

        public Categoria(Evento evento, EnumTipoTransacao qualTransacao, String descricao)
        {
            QualEvento = evento;
            QualTransacao = qualTransacao;
            Descricao = descricao;
        }

        protected Categoria() { }

        public virtual Evento QualEvento
        {
            get { return m_QualEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento");

                m_QualEvento = value;
            }
        }

        public virtual EnumTipoTransacao QualTransacao { get; protected set; }

        public virtual String Descricao 
        {
            get { return m_Descricao; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Descricao");

                m_Descricao = value;
            }
        }

        public virtual Categoria CategoriaPai 
        {
            get { return m_CategoriaPai; }
            set
            {
                if (value != null && value.QualTransacao != QualTransacao)
                    throw new ArgumentException("A categoria pai deve ter o mesmo tipo de transação desta categoria.");

                m_CategoriaPai = value;
            }
        }
    }
}
