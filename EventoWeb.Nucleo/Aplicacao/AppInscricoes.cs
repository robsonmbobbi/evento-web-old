using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscricoes : AppBase
    {
        private readonly AppComunicacao m_AppComunicacao;

        public AppInscricoes(IContexto contexto, AppComunicacao appComunicacao) : 
            base(contexto)
        {
            m_AppComunicacao = appComunicacao;
        }

        public IEnumerable<DTOBasicoInscricao> ListarTodas(int idEvento, EnumSituacaoInscricao situacao)
        {
            var lista = new List<DTOBasicoInscricao>();
            ExecutarSeguramente(() =>
            {
                var inscricoes = Contexto.RepositorioInscricoes.ListarTodasPorEventoESituacao(idEvento, situacao);
                if (inscricoes.Count > 0)
                    lista.AddRange(inscricoes.Select(x => x.ConverterBasico()));
            });

            return lista;
        }

        public void Excluir(int idEvento, int idInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioInscricoes;
                var inscricao = repositorio.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                    {
                        var inscricoesInfantis = repositorio.ListarInscricoesInfantisDoResponsavel(inscricao);
                        foreach(var crianca in inscricoesInfantis)
                        {
                            if (crianca.InscricaoResponsavel1 == inscricao)
                            {
                                if (crianca.InscricaoResponsavel2 == null)
                                    repositorio.Excluir(crianca);
                                else
                                {
                                    crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel2, null);
                                    repositorio.Atualizar(crianca);
                                }
                            }
                            else
                            {
                                crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel1, null);
                                repositorio.Atualizar(crianca);
                            }
                        }
                    }

                    repositorio.Excluir(inscricao);
                }
                else
                    throw new ExcecaoAplicacao("AppInscricoes", "Inscrição não encontrada para este evento");
            });
        }

        public void Rejeitar(int idEvento, int idInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    inscricao.Rejeitar();

                    m_AppComunicacao.EnviarInscricaoRejeitada(inscricao);
                }
            });
        }

        public void Aceitar(int idEvento, int idInscricao, DTOInscricaoAtualizacaoAdulto atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    AtualizarInscricao(participante, atualizacao);
                    participante.Aceitar();

                    m_AppComunicacao.EnviarInscricaoAceita((InscricaoParticipante)inscricao);
                }
            });
        }

        public void IncluirInfantil(int idEvento, DTOInscricaoAtualizacaoInfantil dtoInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscricoes", "Evento encerrado");

                var pessoa = new Pessoa(dtoInscricao.DadosPessoais.Nome,
                    new Endereco(dtoInscricao.DadosPessoais.Cidade, dtoInscricao.DadosPessoais.Uf), dtoInscricao.DadosPessoais.DataNascimento,
                    dtoInscricao.DadosPessoais.Sexo, dtoInscricao.DadosPessoais.Email);

                Inscricao responsavel1 = null;
                Inscricao responsavel2 = null;

                if (dtoInscricao.Responsavel1 != null)
                    responsavel1 = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(dtoInscricao.Responsavel1.Id);

                if (dtoInscricao.Responsavel2 != null)
                    responsavel2 = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(dtoInscricao.Responsavel2.Id);

                InscricaoInfantil inscInfantil = new InscricaoInfantil(pessoa, evento, responsavel1, responsavel2, DateTime.Now, 
                    dtoInscricao.DormeEvento);
                inscInfantil.AtribuirDados(dtoInscricao);

                inscInfantil.TornarPendente();

                var repInscricoes = Contexto.RepositorioInscricoes;
                repInscricoes.Incluir(inscInfantil);

                var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscInfantil, dtoInscricao.Sarais);

                m_AppComunicacao.EnviarInscricaoRegistradaInfantil(inscInfantil);
            });
        }

        public void Incluir(int idEvento, DTOInscricaoAtualizacaoAdulto dtoInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscricoes", "Evento encerrado");

                var pessoa = new Pessoa(dtoInscricao.DadosPessoais.Nome,
                    new Endereco(dtoInscricao.DadosPessoais.Cidade, dtoInscricao.DadosPessoais.Uf), dtoInscricao.DadosPessoais.DataNascimento,
                    dtoInscricao.DadosPessoais.Sexo, dtoInscricao.DadosPessoais.Email);

                var inscParticipante = new InscricaoParticipante(evento, pessoa, DateTime.Now, dtoInscricao.TipoInscricao);
                inscParticipante.AtribuirDados(dtoInscricao);

                inscParticipante.RemoverTodasAtividade();
                inscParticipante.AtribuirAtividadeOficina(dtoInscricao.Oficina, Contexto.RepositorioOficinas);
                inscParticipante.AtribuirAtividadeSalaEstudo(dtoInscricao.SalasEstudo, Contexto.RepositorioSalasEstudo);
                inscParticipante.AtribuirAtividadeDepartamento(dtoInscricao.Departamento, Contexto.RepositorioDepartamentos);

                inscParticipante.TornarPendente();

                var repInscricoes = Contexto.RepositorioInscricoes;
                repInscricoes.Incluir(inscParticipante);

                var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);

                m_AppComunicacao.EnviarInscricaoRegistradaAdulto(inscParticipante);
            });
        }

        public void AceitarInfantil(int idEvento, int idInscricao, DTOInscricaoAtualizacaoInfantil atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser Participante");
                    var crianca = (InscricaoInfantil)inscricao;
                    AtualizarInscricaoInfantil(crianca, atualizacao);
                    crianca.Aceitar();

                    m_AppComunicacao.EnviarInscricaoAceita((InscricaoInfantil)inscricao);
                }
            });
        }

        public void Atualizar(int idEvento, int idInscricao, DTOInscricaoAtualizacaoAdulto atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    AtualizarInscricao(participante, atualizacao);
                }
            });
        }

        public void AtualizarInfantil(int idEvento, int idInscricao, DTOInscricaoAtualizacaoInfantil atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser participante");
                    var crianca = (InscricaoInfantil)inscricao;
                    AtualizarInscricaoInfantil(crianca, atualizacao);
                }
            });
        }

        private void AtualizarInscricao(InscricaoParticipante inscParticipante, DTOInscricaoAtualizacaoAdulto dtoInscricao)
        {
            var repInscricoes = Contexto.RepositorioInscricoes;

            inscParticipante.AtribuirDados(dtoInscricao);

            inscParticipante.RemoverTodasAtividade();
            inscParticipante.AtribuirAtividadeOficina(dtoInscricao.Oficina, Contexto.RepositorioOficinas);
            inscParticipante.AtribuirAtividadeSalaEstudo(dtoInscricao.SalasEstudo, Contexto.RepositorioSalasEstudo);
            inscParticipante.AtribuirAtividadeDepartamento(dtoInscricao.Departamento, Contexto.RepositorioDepartamentos);

            repInscricoes.Atualizar(inscParticipante);

            var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
            appApresentacaoSarau
                .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);
        }

        private void AtualizarInscricaoInfantil(InscricaoInfantil inscricaoInfantil, DTOInscricaoAtualizacaoInfantil dtoInscricao)
        {
            var repInscricoes = Contexto.RepositorioInscricoes;

            inscricaoInfantil.AtribuirDados(dtoInscricao);

            Inscricao responsavel1 = null;
            Inscricao responsavel2 = null;

            if (dtoInscricao.Responsavel1 != null)
                responsavel1 = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(dtoInscricao.Responsavel1.Id);

            if (dtoInscricao.Responsavel2 != null)
                responsavel2 = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(dtoInscricao.Responsavel2.Id);
            
            inscricaoInfantil.AtribuirResponsaveis(responsavel1, responsavel2);

            repInscricoes.Atualizar(inscricaoInfantil);

            var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
            appApresentacaoSarau
                .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscricaoInfantil, dtoInscricao.Sarais);
        }

        public DTOInscricaoCompletaAdulto Obter(int idEvento, int idInscricao)
        {
            DTOInscricaoCompletaAdulto dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");

                    var inscParticipante = (InscricaoParticipante)inscricao;
                    dto = inscParticipante.Converter();

                    dto.Evento.Departamentos = Contexto.RepositorioDepartamentos.ListarTodosPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.Oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.SalasEstudo = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscParticipante.Id)
                        .Select(x => x.Converter()).ToList();
                }
            });
            return dto;
        }

        public DTOInscricaoCompletaInfantil ObterInfantil(int idEvento, int idInscricao)
        {
            DTOInscricaoCompletaInfantil dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser Participante");

                    var inscricaoInfantil = (InscricaoInfantil)inscricao;
                    dto = inscricaoInfantil.Converter();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricaoInfantil.Id)
                        .Select(x => x.Converter()).ToList();
                }
            });
            return dto;
        }
    }
}
