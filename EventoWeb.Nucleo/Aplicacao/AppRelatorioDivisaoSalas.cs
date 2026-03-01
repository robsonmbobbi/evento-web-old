using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppRelatorioDivisaoSalas : AppBase
    {
        private readonly IRelatorioDivisaoSalasEstudo m_GeradorRelDivisaoSalas;
        private readonly ASalasEstudo m_RepSalasEstudo;
        private readonly AInscricoes m_RepInscricoes;
        private readonly AEventos m_RepEventos;


        public AppRelatorioDivisaoSalas(IContexto contexto, AEventos repEventos, ASalasEstudo repSalasEstudo,
            AInscricoes repInscricoes, IRelatorioDivisaoSalasEstudo geradorRelDivisaoSalas) : base(contexto)
        {
            m_RepEventos = repEventos ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoSalas", "repEventos não pode ser nulo");
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoSalas", "repInscricoes não pode ser nulo");
            m_RepSalasEstudo = repSalasEstudo ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoSalas", "repSalasEstudo não pode ser nulo");
            m_GeradorRelDivisaoSalas = geradorRelDivisaoSalas ?? throw new ExcecaoAplicacao("AppRelatorioDivisaoSalas", "geradorRelDivisaoSalas não pode ser nulo");
        }

        public Stream GerarImpressoPDF(int idEvento)
        {
            Stream relatorio = new MemoryStream();
            ExecutarSeguramente(() =>
            {
                var evento = m_RepEventos.ObterEventoPeloId(idEvento);
                var salas = m_RepSalasEstudo.ListarTodasSalasEstudoComParticipantesDoEvento(evento);
                var atividadesSalasCoordenadores = m_RepInscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoSalaEstudoCoordenacao>(evento);

                relatorio = m_GeradorRelDivisaoSalas.Gerar(salas, atividadesSalasCoordenadores);
            });

            return relatorio;
        }
    }
}
