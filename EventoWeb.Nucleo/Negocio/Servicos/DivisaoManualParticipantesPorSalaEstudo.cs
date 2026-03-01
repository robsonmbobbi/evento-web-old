using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    internal class ParaOndeMoverParticipante : IParaOndeMover<SalaEstudo>
    {
        private InscricaoParticipante mParticipante;
        private SalaEstudo mSalaOrigem;

        public ParaOndeMoverParticipante(SalaEstudo salaOrigem, InscricaoParticipante participante)
        {
            mParticipante = participante;
            mSalaOrigem = salaOrigem;
        }

        public void Para(SalaEstudo sala)
        {
            if (sala == null)
                throw new ArgumentNullException("sala", "Sala não pode ser vazia.");

            if (sala.Evento != mParticipante.Evento)
                throw new ArgumentException("Esta sala é de outro evento.", "sala");

            if (sala.EstaNaListaDeParticipantes(mParticipante))
                throw new ArgumentException("Não se pode mover um participante para a sala em que já se esta.", "sala");

            mSalaOrigem.RemoverParticipante(mParticipante);
            sala.AdicionarParticipante(mParticipante);
        }
    }

    internal class MovimentacaoParticipanteSala : IMovimentacaoParticipante<SalaEstudo>
    {
        private SalaEstudo mSala;
        private ASalasEstudo mSalasEstudo;

        public MovimentacaoParticipanteSala(SalaEstudo sala, ASalasEstudo salasEstudo)
        {
            mSala = sala;
            mSalasEstudo = salasEstudo;
        }

        public void IncluirParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mSala.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mSalasEstudo.EhParticipanteDeSalaNoEvento(mSala.Evento, participante))
                throw new ArgumentException("Este participante já tem sala informada.", "participante");

            if (mSalasEstudo.EhCoordenadorDeSalaNoEvento(mSala.Evento, participante))
                throw new ArgumentException("Este participante é coordenador de sala.", "participante");

            mSala.AdicionarParticipante(participante);
        }

        public IParaOndeMover<SalaEstudo> MoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mSala.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mSalasEstudo.EhCoordenadorDeSalaNoEvento(mSala.Evento, participante))
                throw new ArgumentException("Este participante é coordenador de sala.", "participante");

            if (!mSalasEstudo.EhParticipanteDeSalaNoEvento(mSala.Evento, participante))
                throw new ArgumentException("Este participante não tem sala informada.", "participante");

            return new ParaOndeMoverParticipante(mSala, participante);
        }

        public void RemoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (!mSala.EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe nesta sala.");

            mSala.RemoverParticipante(participante);
        }
    }

    public class DivisaoManualParticipantesPorSalaEstudo
    {
        private Evento mEvento;
        private ASalasEstudo mSalasEstudo;

        public DivisaoManualParticipantesPorSalaEstudo(Evento evento, ASalasEstudo salasEstudo)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");

            if (salasEstudo == null)
                throw new ArgumentNullException("evento", "SalasEstudo não pode ser nulo.");

            if (salasEstudo.HaSalasSemCoordenadorDefinidoDoEvento(evento))
                throw new InvalidOperationException("Há salas sem coordenador definido.");
            
            mEvento = evento;
            mSalasEstudo = salasEstudo;
        }

        public IMovimentacaoParticipante<SalaEstudo> Sala(SalaEstudo sala)
        {
            if (sala == null)
                throw new ArgumentNullException("sala", "Sala não pode ser vazia.");

            if (sala.Evento != mEvento)
                throw new ArgumentException("Esta sala é de outro evento.", "sala");

            return new MovimentacaoParticipanteSala(sala, mSalasEstudo);
        }
    }
}
