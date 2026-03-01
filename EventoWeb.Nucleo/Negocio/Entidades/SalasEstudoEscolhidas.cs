using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ExcecaoSalaEstudoInvalida : Exception
    {
        public ExcecaoSalaEstudoInvalida(String mensagem) : base(mensagem) { }
    }

    public class GestaoSalasEstudoEscolhidas
    {
        private SalasEstudoEscolhidas mEscolhas;
        private ASalasEstudo mRepositorioSlEstudo;

        public GestaoSalasEstudoEscolhidas(ASalasEstudo repositorio, SalasEstudoEscolhidas escolhas)
        {
            mRepositorioSlEstudo = repositorio;
            mEscolhas = escolhas;
        }

        public virtual IEnumerable<SalaEstudo> GerarLista()
        {
            var totalSalas = mRepositorioSlEstudo.ContarTotalSalas(mEscolhas.Evento);
            var salas = mEscolhas.Salas;
            if (totalSalas == 0 && salas.Count() > 0)
                throw new ArgumentException("Não há nenhuma sala neste evento.", "evento");

            if (totalSalas > 0 && salas.Count() != totalSalas)
                throw new ArgumentException("Todas as salas do evento devem ser escolhidas e ordenadas.", "evento");

            return salas;
        }
    }

    public class SalasEstudoEscolhidas
    {
        private IList<SalaEstudo> mSalas;
        private Evento mEvento;

        public SalasEstudoEscolhidas(Evento evento)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");
            mEvento = evento;
            mSalas = new List<SalaEstudo>();
        }
        
        public virtual void DefinirPrimeiraPosicao(SalaEstudo sala)
        {
            ValidarSalaNula(sala);
            ValidarSalaExisteEvento(sala);
            ValidarSalaEstaLista(sala);

            mSalas.Clear();
            mSalas.Add(sala);
        }

        public virtual void DefinirProximaPosicao(SalaEstudo sala)
        {
            ValidarSalaNula(sala);
            ValidarSalaExisteEvento(sala);
            ValidarSalaEstaLista(sala);

            if (mSalas.Count == 0)
                throw new IndexOutOfRangeException("Deve-se definir a primeira posição.");

            mSalas.Add(sala);
        }

        private void ValidarSalaNula(SalaEstudo sala)
        {
            if (sala == null)
                throw new ArgumentNullException("item", "Sala não pode ser nula");
        }

        private void ValidarSalaExisteEvento(SalaEstudo sala)
        {
            if (mEvento != sala.Evento)
                throw new ExcecaoSalaEstudoInvalida("A sala informada não existe no evento.");
        }

        private void ValidarSalaEstaLista(SalaEstudo sala)
        {
            if (mSalas.Count(x=> x == sala) > 0)
                throw new ExcecaoSalaEstudoInvalida("A sala informada já esta na lista.");

            if (sala.Id == 0)
                throw new ExcecaoSalaEstudoInvalida("A sala informada não foi efetivada no banco de dados.");
        }

        public virtual Evento Evento { get { return mEvento; } }

        public virtual IEnumerable<SalaEstudo> Salas { get { return mSalas; } }
    }
}
