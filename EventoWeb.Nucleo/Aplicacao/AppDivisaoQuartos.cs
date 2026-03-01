using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Negocio.Servicos;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppDivisaoQuartos : AppBase
    {
        private readonly AEventos m_RepEventos;
        private readonly AQuartos m_RepQuartos;
        private readonly AInscricoes m_RepInscricoes;

        public AppDivisaoQuartos(IContexto contexto, AEventos repEventos, AQuartos repQuartos, 
            AInscricoes repInscricoes) : base(contexto)
        {
            m_RepEventos = repEventos ?? throw new ExcecaoAplicacao("AppDivisaoQuartos", "repEventos não pode ser nulo");
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppDivisaoQuartos", "repInscricoes não pode ser nulo"); 
            m_RepQuartos = repQuartos ?? throw new ExcecaoAplicacao("AppDivisaoQuartos", "repQuartos não pode ser nulo");
        }

        public IEnumerable<DTODivisaoQuarto> ObterDivisao(int idEvento)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> RealizarDivisaoAutomatica(int idEvento)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);                

                DivisaoAutomaticaInscricoesPorQuarto divisor =
                    new DivisaoAutomaticaInscricoesPorQuarto(evento, m_RepInscricoes, m_RepQuartos);
                IList<Quarto> quartos = quartos = divisor.Dividir();

                foreach (var quarto in quartos)
                    m_RepQuartos.Atualizar(quarto);

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> MoverParticipante(int idEvento, int idInscricao, int daIdQuarto, int paraIdQuarto, bool ehCoordenador)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                Quarto quartoOrigem = m_RepQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, daIdQuarto);
                Quarto quartoDestino = m_RepQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, paraIdQuarto);

                InscricaoParticipante participante = (InscricaoParticipante)
                        m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                DivisaoManualInscricaoPorQuarto divisor =
                    new DivisaoManualInscricaoPorQuarto(evento, m_RepQuartos);

                divisor.Quarto(quartoOrigem).MoverInscrito(participante).Para(quartoDestino, ehCoordenador);

                m_RepQuartos.Atualizar(quartoOrigem);
                m_RepQuartos.Atualizar(quartoDestino);

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> RemoverParticipante(int idEvento, int idInscricao, int idQuarto)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                var evento = m_RepEventos.ObterEventoPeloId(idEvento);
                var quarto = m_RepQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, idQuarto);
                var participante = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                var divisor = new DivisaoManualInscricaoPorQuarto(
                    evento, m_RepQuartos);

                divisor.Quarto(quarto).RemoverInscrito(participante);

                m_RepQuartos.Atualizar(quarto);

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> IncluirParticipante(int idEvento, int idQuarto, int idInscricao, bool ehCoordenador)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                InscricaoParticipante inscricao = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                Quarto quarto = m_RepQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, idQuarto);

                DivisaoManualInscricaoPorQuarto divisor =
                    new DivisaoManualInscricaoPorQuarto(evento, m_RepQuartos);

                divisor.Quarto(quarto).IncluirInscrito(inscricao, ehCoordenador);

                m_RepQuartos.Atualizar(quarto);

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> RemoverTodasDivisoes(int idEvento)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                IList<Quarto> quartos = m_RepQuartos.ListarTodosQuartosPorEvento(idEvento);
                
                foreach (var quarto in quartos)
                {
                    quarto.RemoverTodosInscritos();
                    m_RepQuartos.Atualizar(quarto);
                }

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        public IEnumerable<DTODivisaoQuarto> DefinirSeEhCoordenador(int idEvento, int idQuarto, int idInscricao, bool ehCoordenador)
        {
            IList<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                InscricaoParticipante inscricao = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                Quarto quarto = m_RepQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, idQuarto);

                DivisaoManualInscricaoPorQuarto divisor =
                    new DivisaoManualInscricaoPorQuarto(evento, m_RepQuartos);

                divisor.Quarto(quarto).TornarCoordenador(inscricao, ehCoordenador);

                m_RepQuartos.Atualizar(quarto);

                quartosDTO = ObterDivisaoQuartos(idEvento);
            });

            return quartosDTO;
        }

        private IList<DTODivisaoQuarto> ObterDivisaoQuartos(int idEvento)
        {
            List<DTODivisaoQuarto> quartosDTO = new List<DTODivisaoQuarto>();

            IList<Quarto> quartos = m_RepQuartos.ListarTodosQuartosPorEventoComParticipantes(idEvento);
            IList<Inscricao> listaInscricoes = m_RepInscricoes.ListarTodasInscricoesAceitasComPessoasDormemEvento(idEvento);

            var inscritosNosQuartos = quartos.SelectMany(x => x.Inscritos).Select(x => x.Inscricao);
            var inscritosSemQuartos = listaInscricoes.Where(x => !inscritosNosQuartos.Contains(x));

            quartosDTO.AddRange(quartos.Select(x => new DTODivisaoQuarto
            {
                Id = x.Id,
                Nome = x.Nome,
                Capacidade = x.Capacidade,
                EhFamilia = x.EhFamilia,
                Sexo = x.Sexo,
                Coordenadores = x.Inscritos.Where(y => y.EhCoordenador).Select(y => y.Inscricao.ConverterBasicoResp()).ToList(),
                Participantes = x.Inscritos.Where(y => !y.EhCoordenador).Select(y => y.Inscricao.ConverterBasicoResp()).ToList()
            }));

            if (inscritosSemQuartos.Count() > 0)
            {
                quartosDTO.Add(new DTODivisaoQuarto
                {
                    Id = 0,
                    Nome = "Participantes sem quarto definido",
                    Capacidade = null,
                    EhFamilia = false,
                    Sexo = EnumSexoQuarto.Misto,
                    Coordenadores = new List<DTOBasicoInscricaoResp>(),
                    Participantes = new List<DTOBasicoInscricaoResp>(
                            inscritosSemQuartos.Select(x => x.ConverterBasicoResp()).ToList())
                });
            }

            return quartosDTO;
        }
    }
}
