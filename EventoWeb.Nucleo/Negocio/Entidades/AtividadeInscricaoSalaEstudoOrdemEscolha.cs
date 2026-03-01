using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoSalaEstudoOrdemEscolha: AAtividadeInscricao
    {
        private IEnumerable<SalaEstudo> m_Salas;

        public AtividadeInscricaoSalaEstudoOrdemEscolha(InscricaoParticipante inscrito, GestaoSalasEstudoEscolhidas gestaoEscolha)
            : base(inscrito)
        {
            Atualizar(gestaoEscolha);
        }

        protected AtividadeInscricaoSalaEstudoOrdemEscolha() { }

        public virtual void Atualizar(GestaoSalasEstudoEscolhidas gestao)
        {
            if (gestao == null)
                throw new ArgumentException("É preciso informar as escolhas feitas das salas de estudo", "gestao");

            m_Salas = new List<SalaEstudo>(gestao.GerarLista());
        }

        public virtual IEnumerable<SalaEstudo> Salas { get { return m_Salas; } }
    }
}