using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DivisaoAutomaticaInscricoesParticipantePorSalaEstudo
    {
        private Evento mEvento;
        private AInscricoes mInscricoes;
        private ASalasEstudo mSalasEstudo;

        public DivisaoAutomaticaInscricoesParticipantePorSalaEstudo(Evento evento, AInscricoes inscricoes, ASalasEstudo salasEstudo)
        {
            mEvento = evento;
            mInscricoes = inscricoes;
            mSalasEstudo = salasEstudo;
        }

        public IList<SalaEstudo> Dividir()
        {
            if (mSalasEstudo.HaSalasSemCoordenadorDefinidoDoEvento(mEvento))
                throw new InvalidOperationException("Há salas sem coordenador definido.");

            var participantes =
                mInscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoSalaEstudo>(mEvento);

            IList<SalaEstudo> salas = mSalasEstudo.ListarTodasPorEvento(mEvento.Id);
            foreach (var sala in salas)
                sala.RemoverTodosParticipantes();

            var salaComFaixaEtaria = salas.FirstOrDefault(x => x.FaixaEtaria != null);
            if (salaComFaixaEtaria != null)
            {
                var inscricoesDentroFaixaEtaria = participantes
                    .Where(x => x.Inscrito.Pessoa.CalcularIdadeEmAnos(mEvento.PeriodoRealizacaoEvento.DataInicial) >= salaComFaixaEtaria.FaixaEtaria.IdadeMin
                             && x.Inscrito.Pessoa.CalcularIdadeEmAnos(mEvento.PeriodoRealizacaoEvento.DataInicial) <= salaComFaixaEtaria.FaixaEtaria.IdadeMax)
                    .ToList();

                foreach (var participante in inscricoesDentroFaixaEtaria)
                {
                    salaComFaixaEtaria.AdicionarParticipante(participante.Inscrito);
                    participantes.Remove(participante);
                }

                salas.Remove(salaComFaixaEtaria);
            }

            int indiceSalaEstudo = 0;

            IList<InscricaoParticipante> participantesComSalaDefinida = new List<InscricaoParticipante>();

            foreach (var participante in participantes.OrderByDescending(x => x.Inscrito.Pessoa.DataNascimento))
            {
                salas[indiceSalaEstudo].AdicionarParticipante(participante.Inscrito);

                indiceSalaEstudo++;
                if (indiceSalaEstudo == salas.Count)
                    indiceSalaEstudo = 0;
            }

            return salas;
        }
    }
}
