using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumTipoParticipante { Participante, ParticipanteTrabalhador, Trabalhador }    

    public class InscricaoParticipante: Inscricao
    {
        private IList<AAtividadeInscricao> m_Atividades;
        

        public InscricaoParticipante(Evento evento, Pessoa pessoa, DateTime dataRecebimento, EnumTipoParticipante tipo) :
            base(evento, pessoa, dataRecebimento)
        {
            m_Atividades = new List<AAtividadeInscricao>();
            Tipo = tipo;            
        }

        protected InscricaoParticipante() { }

        public virtual IEnumerable<AAtividadeInscricao> Atividades { get { return m_Atividades; } }

        public virtual EnumTipoParticipante Tipo { get; set; }

        
        public virtual String InstituicoesEspiritasFrequenta { get; set; }      

        public virtual void AdicionarAtividade(AAtividadeInscricao atividade)
        {
            if (atividade.Inscrito != this)
                throw new ArgumentException("Atividade é de outra inscrição", "atividade");

            if (m_Atividades.Count(x=> x.GetType() == atividade.GetType()) > 0)
                throw new ArgumentException("Não é possível ter a mesma atividade mais de uma vez", "atividade");

            if ((!Evento.TemDepartamentalizacao && atividade.GetType() == typeof(AtividadeInscricaoDepartamento)) ||
                (Evento.ConfiguracaoOficinas == null && (atividade.GetType() == typeof(AtividadeInscricaoOficinas) || atividade.GetType() == typeof(AtividadeInscricaoOficinasCoordenacao))) ||
                (Evento.ConfiguracaoSalaEstudo == null && 
                   (atividade.GetType() == typeof(AtividadeInscricaoSalaEstudo) || 
                    atividade.GetType() == typeof(AtividadeInscricaoSalaEstudoCoordenacao) ||
                    atividade.GetType() == typeof(AtividadeInscricaoSalaEstudoOrdemEscolha))) ||
                (Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorIdadeCidade  && atividade.GetType() == typeof(AtividadeInscricaoSalaEstudoOrdemEscolha)) ||
                (Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao && atividade.GetType() == typeof(AtividadeInscricaoSalaEstudo)))
                throw new ArgumentException("Tipo de atividade não é aceita pelo evento", "atividade");

            m_Atividades.Add(atividade);
        }

        public virtual void RemoverTodasAtividade()
        {
            m_Atividades.Clear();
        }

        public override bool EhValidaIdade(int idade)
        {
            return idade >= Evento.IdadeMinimaInscricaoAdulto;
        }

        protected override void ValidarInscricaoParaSeTornarPendente()
        {
            // ToDo: Escrever código aqui
        }
    }
}
