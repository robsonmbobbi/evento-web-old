using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoOficinas : AAtividadeInscricao
    {
        private IList<Oficina> m_Oficinas;

        public AtividadeInscricaoOficinas(InscricaoParticipante inscrito, GestaoOficinasEscolhidas gestaoEscolhaOficinas)
            : base(inscrito)
        {
            AtualizarOficinas(gestaoEscolhaOficinas);
        }

        protected AtividadeInscricaoOficinas() { }

        public virtual void AtualizarOficinas(GestaoOficinasEscolhidas gestaoEscolhaOficinas)
        {
            if (gestaoEscolhaOficinas == null)
                throw new ArgumentException("É preciso informar as escolhas feitas das oficinas", "oficinas");

            m_Oficinas = new List<Oficina>(gestaoEscolhaOficinas.GerarLista());
           /* var lista = gestaoEscolhaOficinas.GerarLista();
            for (var indice = 0; indice < lista.Count(); indice++)
                m_Oficinas.Add(new OficinaEscolhida(this, lista.ElementAt(indice), indice));*/
        }

        public virtual IEnumerable<Oficina> Oficinas { get { return m_Oficinas; } }
    }

    public class AtividadeInscricaoOficinasCoordenacao : AAtividadeInscricao
    {
        private Oficina m_OficinaEscolhida;

        public AtividadeInscricaoOficinasCoordenacao(InscricaoParticipante inscrito, Oficina oficina)
            : base(inscrito)
        {
            OficinaEscolhida = oficina;
        }

        protected AtividadeInscricaoOficinasCoordenacao() { }

        public virtual Oficina OficinaEscolhida
        {
            get { return m_OficinaEscolhida; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("OficinaEscolhida");

                m_OficinaEscolhida = value;
            }
        }
    }
}
