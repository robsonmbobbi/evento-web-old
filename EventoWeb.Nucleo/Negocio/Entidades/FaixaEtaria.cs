using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class FaixaEtaria
    {
        private int m_IdadeMax;
        private int m_IdadeMin;

        public FaixaEtaria(int idadeMin, int idadeMax)
        {
            AtribuirFaixa(idadeMin, idadeMax);
        }

        protected FaixaEtaria() { }

        public virtual void AtribuirFaixa(int idadeMin, int idadeMax)
        {
            if (idadeMin < 0)
                throw new ArgumentException("Idade Mínima deve ser maior ou igual a zero", "idadeMin");

            if (idadeMax < 0)
                throw new ArgumentException("Idade Máxima deve ser maior ou igual a zero", "idadeMax");

            if (idadeMin > idadeMax)
                throw new ArgumentException("Idade Mínima deve ser igual ou menor a Idade Máxima", "idadeMin");

            m_IdadeMin = idadeMin;
            m_IdadeMax = idadeMax;
        }

        public virtual int IdadeMin { get { return m_IdadeMin; } }
        public virtual int IdadeMax { get { return m_IdadeMax; } }
    }
}
