using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEventoAcessoInscricoes : AppBase
    {
        private AppComunicacao m_AppComunicacao;

        public AppInscOnlineEventoAcessoInscricoes(IContexto contexto, AppComunicacao appComunicacao)
            : base(contexto) 
        {
            m_AppComunicacao = appComunicacao;
        }

        public DTOBasicoInscricao ObterPorId(int id)
        {
            DTOBasicoInscricao dtoInscricao = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id);
                if (inscricao != null && inscricao is InscricaoParticipante)
                    dtoInscricao = inscricao.ConverterBasico();
            });

            return dtoInscricao;
        }

        public bool ValidarCodigo(int id, string codigo)
        {
            bool valido = false;
            ExecutarSeguramente(() =>
            {
                var repCodigos = Contexto.RepositorioCodigosAcessoInscricao;
                repCodigos.ExcluirCodigosVencidos();

                var codigoAcesso = repCodigos.ObterPeloCodigo(codigo);
                valido = codigoAcesso != null && codigoAcesso.Inscricao.Id == id;
            });

            return valido;
        }

        public DTOEnvioCodigoAcessoInscricao EnviarCodigo(string identificacao)
        {
            DTOEnvioCodigoAcessoInscricao dto = new DTOEnvioCodigoAcessoInscricao
            {
                IdInscricao = null,
                Resultado = EnumResultadoEnvio.InscricaoNaoEncontrada
            };
            ExecutarSeguramente(() =>
            {
                try
                {
                    int idInscricao = new AppInscOnLineIdentificacaoInscricao().ExtrarId(identificacao);
                    var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);

                    if (inscricao != null)
                    {
                        dto.IdInscricao = inscricao.Id;
                        if (inscricao.Evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || inscricao.Evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                            dto.Resultado = EnumResultadoEnvio.EventoEncerradoInscricao;
                        else
                            dto.Resultado = EnumResultadoEnvio.InscricaoOK;

                        if (dto.Resultado == EnumResultadoEnvio.InscricaoOK)
                        {
                            string codigo = GerarCodigoUnico();
                            var codigoAcesso = new CodigoAcessoInscricao(codigo, inscricao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                            Contexto.RepositorioCodigosAcessoInscricao.Incluir(codigoAcesso);

                            m_AppComunicacao.EnviarCodigoAcompanhamentoInscricao(inscricao, codigo);                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ExcecaoAplicacao)
                        dto.Resultado = EnumResultadoEnvio.IdentificacaoInvalida;
                    else
                        throw ex;
                }
            });

            return dto;
        }

        public bool ValidarCodigoEmail(string identificacao, string codigo)
        {
            bool valido = false;
            ExecutarSeguramente(() =>
            {
                var repCodigos = Contexto.RepositorioCodigosAcessoInscricao;
                repCodigos.ExcluirCodigosVencidos();

                var codigoAcesso = repCodigos.ObterPeloCodigo(codigo);
                valido = codigoAcesso != null && codigoAcesso.Identificacao.ToUpper() == identificacao.ToUpper();
            });

            return valido;
        }

        public void EnviarCodigoEmail(int idEvento, DTOEnvioCodigoEmail dadosEnvio, string codigo)
        {
            ExecutarSeguramente(() =>
            {
                string codigo = GerarCodigoUnico();
                var codigoAcesso = new CodigoAcessoInscricao(codigo, dadosEnvio.Identificacao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                Contexto.RepositorioCodigosAcessoInscricao.Incluir(codigoAcesso);

                m_AppComunicacao.EnviarCodigoValidacao(idEvento, dadosEnvio, codigo);
            });
        }

        private string GerarCodigoUnico()
        {
            var m_RepCodigosAcessoInscricao = Contexto.RepositorioCodigosAcessoInscricao;
            m_RepCodigosAcessoInscricao.ExcluirCodigosVencidos();

            string codigo;
            do
            {
                codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigo5Caracteres();
            } while (m_RepCodigosAcessoInscricao.ObterPeloCodigo(codigo) != null);

            return codigo;
        }


        public DTODadosConfirmacao CriarInscricao(int idEvento, DTOInscricaoAtualizacaoAdulto dtoInscricao)
        {
            DTODadosConfirmacao dto = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoAcessoInscricoes", "Evento encerrado");

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

                dto = new DTODadosConfirmacao
                {
                    EnderecoEmail = inscParticipante.Pessoa.Email,
                    IdInscricao = inscParticipante.Id
                };
            });

            return dto;
        }

        public void CriarInscricaoInfantil(int idEvento, DTOInscricaoAtualizacaoInfantil dtoInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoAcessoInscricoes", "Evento encerrado");

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
    }
}
