using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AQuartos: ARepositorio<Quarto>
    {
        public AQuartos(IPersistencia<Quarto> persistencia) : base(persistencia) { }
        public abstract IList<Quarto> ListarTodosQuartosPorEvento(int idEvento);
        public abstract Quarto ObterQuartoPorIdEventoEQuarto(int idEvento, int idQuarto);
        public abstract IList<Quarto> ListarTodosQuartosPorEventoComParticipantes(int idEvento);
        public abstract Quarto BuscarQuartoDoInscrito(int idEvento, int idInscricao);
        protected abstract Boolean HaOutroQuartoComCapacidadeInfinita(Quarto quarto);

        public override void Incluir(Quarto objeto)
        {
            ValidarQuartoCapacidadeInfinita(objeto);
            base.Incluir(objeto);
        }

        public override void Atualizar(Quarto objeto)
        {
            ValidarQuartoCapacidadeInfinita(objeto);
            base.Atualizar(objeto);
        }

        private void ValidarQuartoCapacidadeInfinita(Quarto quarto)
        {
            if (quarto.Capacidade == null && HaOutroQuartoComCapacidadeInfinita(quarto))
                throw new ExcecaoNegocioRepositorio("AQuartos", "Ao não informar a capacidade do quarto, o mesmo é considerado com " +
                    "capacidade infinita. E já há um quarto nessa condição.");
        }
    }
}
