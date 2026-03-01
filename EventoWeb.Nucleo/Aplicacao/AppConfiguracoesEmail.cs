using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppConfiguracoesEmail : AppBase
    {
        public AppConfiguracoesEmail(IContexto contexto) : base(contexto)
        {
        }

        public void CriarOuAtualizar(int idEvento, DTOConfiguracaoEmail dto)
        {
            ExecutarSeguramente(() =>
            {
                var evento = ObterEventoOuExcecaoSeNaoEncontrar(idEvento);
                var repositorioCnf = Contexto.RepositorioConfiguracoesEmail;
                var configuracao = repositorioCnf.Obter(idEvento);
                var EhParaCriar = true;

                if (configuracao == null)
                    configuracao = new ConfiguracaoEmail(evento);
                else
                    EhParaCriar = false;

                configuracao.EnderecoEmail = dto.EnderecoEmail;
                configuracao.PortaServidor = dto.PortaServidor;
                configuracao.SenhaEmail = dto.SenhaEmail;
                configuracao.ServidorEmail = dto.ServidorEmail;
                configuracao.TipoSeguranca = dto.TipoSeguranca;
                configuracao.UsuarioEmail = dto.UsuarioEmail;

                if (EhParaCriar)
                    repositorioCnf.Incluir(configuracao);
                else
                    repositorioCnf.Atualizar(configuracao);

            });
        }

        public DTOConfiguracaoEmail Obter(int idEvento)
        {
            DTOConfiguracaoEmail dto = null;
            ExecutarSeguramente(() =>
            {
                var configuracao = Contexto.RepositorioConfiguracoesEmail.Obter(idEvento);
                if (configuracao == null)
                    dto = null;
                else
                    dto = new DTOConfiguracaoEmail
                    {
                        EnderecoEmail = configuracao.EnderecoEmail,
                        PortaServidor = configuracao.PortaServidor,
                        SenhaEmail = configuracao.SenhaEmail,
                        ServidorEmail = configuracao.ServidorEmail,
                        TipoSeguranca = configuracao.TipoSeguranca,
                        UsuarioEmail = configuracao.UsuarioEmail
                    };
            });

            return dto;
        }

        private Evento ObterEventoOuExcecaoSeNaoEncontrar(int id)
        {
            Evento evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);

            if (evento != null)
                return evento;
            else
                throw new ExcecaoAplicacao("AppConfiguracoesEmail", "Não foi encontrado nenhum evento com o id informado.");
        }
    }

    public class DTOConfiguracaoEmail
    {
        public String EnderecoEmail { get; set; }

        public String UsuarioEmail { get; set; }

        public String SenhaEmail { get; set; }

        public String ServidorEmail { get; set; }

        public int? PortaServidor { get; set; }

        public TipoSegurancaEmail? TipoSeguranca { get; set; }
    }

}
