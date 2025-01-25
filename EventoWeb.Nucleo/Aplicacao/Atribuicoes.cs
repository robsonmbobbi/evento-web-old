using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public static class AtribuicaoInscricao
    {
        public static void AtribuirDados(this InscricaoParticipante inscParticipante, DTOInscricaoAtualizacaoAdulto dtoInscricao)
        {
            inscParticipante.AtribuirDadosComum(dtoInscricao);

            inscParticipante.InstituicoesEspiritasFrequenta = dtoInscricao.CentroEspirita;
            inscParticipante.Tipo = dtoInscricao.TipoInscricao;
        }

        public static void AtribuirDados(this Pessoa pessoa, DTOInscricaoDadosPessoais dadosPessoais)
        {
            pessoa.AlergiaAlimentos = dadosPessoais.AlimentosAlergia;
            pessoa.Celular = dadosPessoais.Celular;
            pessoa.DataNascimento = dadosPessoais.DataNascimento;
            pessoa.EhDiabetico = dadosPessoais.EhDiabetico;
            pessoa.EhVegetariano = dadosPessoais.EhVegetariano;
            pessoa.Email = dadosPessoais.Email;
            pessoa.Endereco.Cidade = dadosPessoais.Cidade;
            pessoa.Endereco.UF = dadosPessoais.Uf;
            pessoa.Nome = dadosPessoais.Nome;
            pessoa.Sexo = dadosPessoais.Sexo;
            pessoa.UsaAdocanteDiariamente = dadosPessoais.UsaAdocanteDiariamente;
        }

        public static void AtribuirAtividadeOficina(this InscricaoParticipante inscParticipante, DTOInscricaoOficina dto, AOficinas repOficinas)
        {
            if (dto != null)
            {
                if (dto.Coordenador != null)
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoOficinasCoordenacao(
                            inscParticipante,
                            repOficinas.ObterPorId(inscParticipante.Evento.Id, dto.Coordenador.Id)));
                else if (dto.EscolhidasParticipante != null && dto.EscolhidasParticipante.Count > 0)
                {
                    var oficinas = repOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id);
                    var escolhas = new OficinasEscolhidas(inscParticipante.Evento);
                    foreach (var dtoOficina in dto.EscolhidasParticipante)
                    {
                        if (escolhas.Oficinas.Count() == 0)
                            escolhas.DefinirPrimeiraPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        else
                            escolhas.DefinirProximaPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                    }

                    var gestaoOficinas = new GestaoOficinasEscolhidas(
                        repOficinas,
                        escolhas);

                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoOficinas(
                            inscParticipante,
                            gestaoOficinas));
                }
                else
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoOficinaSemEscolha(inscParticipante));
            }
        }

        public static void AtribuirAtividadeSalaEstudo(this InscricaoParticipante inscParticipante, DTOInscricaoSalaEstudo dto, ASalasEstudo repSalas)
        {
            if (dto != null)
            {
                if (dto.Coordenador != null)
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoSalaEstudoCoordenacao(
                            inscParticipante,
                            repSalas.ObterPorId(inscParticipante.Evento.Id, dto.Coordenador.Id)));
                else if (dto.EscolhidasParticipante != null && dto.EscolhidasParticipante.Count > 0)
                {
                    var salas = repSalas.ListarTodasPorEvento(inscParticipante.Evento.Id);
                    var escolhas = new SalasEstudoEscolhidas(inscParticipante.Evento);
                    foreach (var dtoOficina in dto.EscolhidasParticipante)
                    {
                        if (escolhas.Salas.Count() == 0)
                            escolhas.DefinirPrimeiraPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        else
                            escolhas.DefinirProximaPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                    }

                    var gestaoOficinas = new GestaoSalasEstudoEscolhidas(
                        repSalas,
                        escolhas);

                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoSalaEstudoOrdemEscolha(
                            inscParticipante,
                            gestaoOficinas));
                }
                else
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoSalaEstudo(inscParticipante));
            }
        }

        public static void AtribuirAtividadeDepartamento(this InscricaoParticipante inscParticipante, DTOInscricaoDepartamento dto, ADepartamentos repDepartamento)
        {
            if (dto != null)
            {
                Departamento departamento = null;
                bool ehCoordenador = false;
                if (dto.Coordenador != null)
                {
                    departamento = repDepartamento.ObterPorId(dto.Coordenador.Id);
                    ehCoordenador = true;
                }
                else if (dto.Participante != null)
                    departamento = repDepartamento.ObterPorId(dto.Participante.Id);

                if (departamento == null)
                    throw new ExcecaoAplicacao("AtribuicaoInscricao", "As informações para departamento estão incompletas");

                inscParticipante.AdicionarAtividade(
                    new AtividadeInscricaoDepartamento(inscParticipante, departamento) { EhCoordenacao = ehCoordenador });
            }
        }

        public static void AtribuirDados(this InscricaoInfantil inscricao, DTOInscricaoAtualizacaoInfantil dto)
        {
            inscricao.AtribuirDadosComum(dto);
        }

        private static void AtribuirDadosComum<TSarau>(this Inscricao inscricao, ADTOInscricaoAtualizacao<TSarau> dtoInscricao)
            where TSarau: DTOSarau
        {
            inscricao.Pessoa.AtribuirDados(dtoInscricao.DadosPessoais);
            inscricao.NomeCracha = dtoInscricao.NomeCracha;
            inscricao.Observacoes = dtoInscricao.Observacoes;
            inscricao.DormeEvento = dtoInscricao.DormeEvento;

            inscricao.Pagamento.AtribuirFormaPagamento(dtoInscricao.Pagamento.Forma.Value,
                dtoInscricao.Pagamento.ComprovantesBase64
                        .Select(x => new ArquivoBinario(Convert.FromBase64String(x), EnumTipoArquivoBinario.ImagemJPEG)));
            inscricao.Pagamento.Observacao = dtoInscricao.Pagamento.Observacao;

            inscricao.Pessoa.AtribuirDados(dtoInscricao.DadosPessoais);
        }
    }
}
