using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    internal class ParaOndeMoverParticipanteOficina : IParaOndeMover<Oficina>
    {
        private InscricaoParticipante mParticipante;
        private Oficina mOficinaOrigem;

        public ParaOndeMoverParticipanteOficina(Oficina oficinaOrigem, InscricaoParticipante participante)
        {
            mParticipante = participante;
            mOficinaOrigem = oficinaOrigem;
        }

        public void Para(Oficina oficina)
        {
            if (oficina == null)
                throw new ArgumentNullException("oficina", "Oficina não pode ser vazia.");

            if (oficina.Evento != mParticipante.Evento)
                throw new ArgumentException("Esta oficina é de outro evento.", "oficina");

            if (oficina.EstaNaListaDeParticipantes(mParticipante))
                throw new ArgumentException("Não se pode mover um participante para a oficina em que já se esta.", "oficina");

            mOficinaOrigem.RemoverParticipante(mParticipante);
            oficina.AdicionarParticipante(mParticipante);
        }
    }

    internal class MovimentacaoParticipanteOficina : IMovimentacaoParticipante<Oficina>
    {
        private Oficina mOficina;
        private AOficinas mOficinas;

        public MovimentacaoParticipanteOficina(Oficina oficina, AOficinas oficinas)
        {
            mOficina = oficina;
            mOficinas = oficinas;
        }

        public void IncluirParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mOficina.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mOficinas.EhParticipanteDeOficinaNoEvento(mOficina.Evento, participante))
                throw new ArgumentException("Este participante já tem oficina informada.", "participante");

            if (mOficinas.InscritoEhResponsavelPorOficina(mOficina.Evento, participante))
                throw new ArgumentException("Este participante é responsável de oficina.", "participante");

            mOficina.AdicionarParticipante(participante);
        }

        public IParaOndeMover<Oficina> MoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (participante.Evento != mOficina.Evento)
                throw new ArgumentException("Este participante é de outro evento.", "participante");

            if (mOficinas.InscritoEhResponsavelPorOficina(mOficina.Evento, participante))
                throw new ArgumentException("Este participante é responsável de oficina.", "participante");

            if (!mOficinas.EhParticipanteDeOficinaNoEvento(mOficina.Evento, participante))
                throw new ArgumentException("Este participante não tem oficina informada.", "participante");

            return new ParaOndeMoverParticipanteOficina(mOficina, participante);
        }

        public void RemoverParticipante(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");

            if (!mOficina.EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe nesta oficina.");

            mOficina.RemoverParticipante(participante);
        }
    }

    public class DivisaoManualParticipantesPorOficina
    {
        private Evento mEvento;
        private AOficinas mOficinas;

        public DivisaoManualParticipantesPorOficina(Evento evento, AOficinas oficinas)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");

            if (oficinas == null)
                throw new ArgumentNullException("evento", "Oficina não pode ser nulo.");

            if (oficinas.HaAOficinasSemResponsavelDefinidoDoEvento(evento))
                throw new InvalidOperationException("Há oficinas sem responsável definido.");
            
            mEvento = evento;
            mOficinas = oficinas;
        }

        public IMovimentacaoParticipante<Oficina> Oficina(Oficina oficina)
        {
            if (oficina == null)
                throw new ArgumentNullException("oficina", "Oficina não pode ser vazia.");

            if (oficina.Evento != mEvento)
                throw new ArgumentException("Esta oficina é de outro evento.", "oficina");

            return new MovimentacaoParticipanteOficina(oficina, mOficinas);
        }
    }
}
