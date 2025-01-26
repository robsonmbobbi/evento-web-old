using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorInscricao
    {
        public static DTOInscricaoCompletaAdulto Converter(this InscricaoParticipante inscricao)
        {
            var dto = new DTOInscricaoCompletaAdulto();
            dto.Converter(inscricao);
            return dto;
        }

        public static DTOInscricaoCompletaAdultoCodigo ConverterComCodigo(this InscricaoParticipante inscricao)
        {
            var dto = new DTOInscricaoCompletaAdultoCodigo();
            dto.Converter(inscricao);
            return dto;
        }

        private static ADTOInscricaoCompletaAdulto<TSarau> Converter<TSarau>(this ADTOInscricaoCompletaAdulto<TSarau> dto, 
            InscricaoParticipante inscricao)
            where TSarau: DTOSarau
        {
            dto.ConverterDTOAtualizacao(inscricao);
            dto.ConverterDTOCompleta(inscricao);

            dto.CentroEspirita = inscricao.InstituicoesEspiritasFrequenta;
            dto.TipoInscricao = inscricao.Tipo;

            dto.Departamento = ((AtividadeInscricaoDepartamento)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoDepartamento))?.Converter();

            dto.Oficina = ((AtividadeInscricaoOficinas)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinas))?.Converter();
            if(dto.Oficina == null)
                dto.Oficina = ((AtividadeInscricaoOficinaSemEscolha)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinaSemEscolha))?.Converter();
            if (dto.Oficina == null)
                dto.Oficina = ((AtividadeInscricaoOficinasCoordenacao)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinasCoordenacao))?.Converter();

            dto.SalasEstudo = ((AtividadeInscricaoSalaEstudo)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudo))?.Converter();
            if (dto.SalasEstudo == null)
                dto.SalasEstudo = ((AtividadeInscricaoSalaEstudoOrdemEscolha)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoOrdemEscolha))?.Converter();
            if (dto.SalasEstudo == null)
                dto.SalasEstudo = ((AtividadeInscricaoSalaEstudoCoordenacao)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoCoordenacao))?.Converter();


            return dto;
        }

        private static ADTOInscricaoAtualizacao<TSarau> ConverterDTOAtualizacao<TSarau>(this ADTOInscricaoAtualizacao<TSarau> dto,
            Inscricao inscricao)
            where TSarau : DTOSarau
        {
            dto.DadosPessoais = new DTOInscricaoDadosPessoais
            {
                AlimentosAlergia = inscricao.Pessoa.AlergiaAlimentos,
                Cidade = inscricao.Pessoa.Endereco.Cidade,
                DataNascimento = inscricao.Pessoa.DataNascimento,
                EhDiabetico = inscricao.Pessoa.EhDiabetico,
                EhVegetariano = inscricao.Pessoa.EhVegetariano,
                Email = inscricao.Pessoa.Email,
                Nome = inscricao.Pessoa.Nome,
                Sexo = inscricao.Pessoa.Sexo,
                Uf = inscricao.Pessoa.Endereco.UF,
                UsaAdocanteDiariamente = inscricao.Pessoa.UsaAdocanteDiariamente,
                Celular = inscricao.Pessoa.Celular,
            };
            dto.NomeCracha = inscricao.NomeCracha;
            dto.Observacoes = inscricao.Observacoes;
            dto.DormeEvento = inscricao.DormeEvento;

           if (inscricao.Pagamento != null)
            {
                dto.Pagamento = new DTOPagamento
                {
                    Comprovantes = inscricao.Pagamento.Comprovantes?.Select(x => new DTOComprovantePagamento
                    {
                        Base64 = Convert.ToBase64String(x.ArquivoComprovante.Arquivo),
                        TipoArquivo = x.ArquivoComprovante.Tipo
                    }).ToList(),
                    Forma = inscricao.Pagamento.Forma,
                    Observacao = inscricao.Pagamento.Observacao
                };
            }

            return dto;
        }

        private static IDTOInscricaoCompleta ConverterDTOCompleta(this IDTOInscricaoCompleta dto,
            Inscricao inscricao)
        {
            dto.Evento = inscricao.Evento.ConverterParaInsOnLine();
            dto.Id = inscricao.Id;
            dto.Situacao = inscricao.Situacao;

            return dto;
        }

        public static DTOInscricaoSimplificada ConverterSimplificada(this Inscricao inscricao)
        {
            return new DTOInscricaoSimplificada
            {
                Id = inscricao.Id,
                IdEvento = inscricao.Evento.Id,
                Nome = inscricao.Pessoa.Nome,
                Cidade = inscricao.Pessoa.Endereco.Cidade,
                UF = inscricao.Pessoa.Endereco.UF
            };
        }

        public static DTOBasicoInscricao ConverterBasico(this Inscricao inscricao)
        {
            var dto = new DTOBasicoInscricao();
            dto.ConverterBasico(inscricao);
            return dto;
        }

        public static DTOBasicoInscricaoResp ConverterBasicoResp(this Inscricao inscricao)
        {
            var dto = new DTOBasicoInscricaoResp();
            dto.ConverterBasico(inscricao);

            dto.Responsaveis = null;

            if (inscricao is InscricaoInfantil insInfantil)
            {
                dto.Responsaveis = new List<DTOInscricaoSimplificada>
                {
                    insInfantil.InscricaoResponsavel1.ConverterSimplificada()
                };

                if (insInfantil.InscricaoResponsavel2 != null)
                    dto.Responsaveis.Add(insInfantil.InscricaoResponsavel2.ConverterSimplificada());
            }

            return dto;

        }

        private static void ConverterBasico(this DTOBasicoInscricao dto, Inscricao inscricao)
        {
            var gerarDescricaoTipo = new Func<InscricaoParticipante, string>((insc) =>
            {
                switch (insc.Tipo)
                {
                    case EnumTipoParticipante.Participante: return "Participante";
                    case EnumTipoParticipante.ParticipanteTrabalhador: return "Participante/Trabalhador";
                    case EnumTipoParticipante.Trabalhador: return "Trabalhador";
                    default: return "";
                }
            });

            dto.Email = inscricao.Pessoa.Email;
            dto.IdEvento = inscricao.Evento.Id;
            dto.IdInscricao = inscricao.Id;
            dto.NomeEvento = inscricao.Evento.Nome;
            dto.NomeInscrito = inscricao.Pessoa.Nome;
            dto.Cidade = inscricao.Pessoa.Endereco.Cidade;
            dto.DataNascimento = inscricao.Pessoa.DataNascimento;
            dto.Situacao = inscricao.Situacao;
            dto.Tipo = (inscricao is InscricaoInfantil ? "Infantil" : gerarDescricaoTipo((InscricaoParticipante)inscricao));
            dto.UF = inscricao.Pessoa.Endereco.UF;
        }

        public static DTOInscricaoCompletaInfantil Converter(this InscricaoInfantil inscricao)
        {
            var crianca = new DTOInscricaoCompletaInfantil();
            crianca.Converter(inscricao);

            return crianca;
        }

        public static DTOInscricaoCompletaInfantilCodigo ConverterComCodigo(this InscricaoInfantil inscricao)
        {
            var crianca = new DTOInscricaoCompletaInfantilCodigo();
            crianca.Converter(inscricao);

            return crianca;
        }

        private static ADTOInscricaoCompletaInfantil<TSarau> Converter<TSarau>(this ADTOInscricaoCompletaInfantil<TSarau> dto,
            InscricaoInfantil inscricao)
            where TSarau : DTOSarau
        {
            dto.ConverterDTOAtualizacao(inscricao);
            dto.ConverterDTOCompleta(inscricao);

            dto.Responsavel1 = inscricao.InscricaoResponsavel1.ConverterSimplificada();
            dto.Responsavel2 = inscricao.InscricaoResponsavel2?.ConverterSimplificada();

            return dto;
        }

        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudo atividade)
        {
            return new DTOInscricaoSalaEstudo();
        }
        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudoCoordenacao atividade)
        {
            return new DTOInscricaoSalaEstudo
            {
                Coordenador = atividade.SalaEscolhida.Converter(),
                EscolhidasParticipante = null
            };
        }
        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudoOrdemEscolha atividade)
        {
            return new DTOInscricaoSalaEstudo
            {
                Coordenador = null,
                EscolhidasParticipante = atividade.Salas.Select(x => x.Converter()).ToList()
            };
        }
        public static DTOInscricaoOficina Converter(this AtividadeInscricaoOficinas atividade)
        {
            return new DTOInscricaoOficina
            {
                Coordenador = null,
                EscolhidasParticipante = atividade.Oficinas.Select(x => x.Converter()).ToList()
            };
        }

        public static DTOInscricaoOficina Converter(this AtividadeInscricaoOficinaSemEscolha atividade)
        {
            return new DTOInscricaoOficina();
        }

        public static DTOInscricaoOficina Converter(this AtividadeInscricaoOficinasCoordenacao atividade)
        {
            return new DTOInscricaoOficina
            {
                Coordenador = atividade.OficinaEscolhida.Converter(),
                EscolhidasParticipante = null
            };
        }
        public static DTOInscricaoDepartamento Converter(this AtividadeInscricaoDepartamento atividade)
        {
            var dtoDep = atividade.DepartamentoEscolhido.Converter();

            return new DTOInscricaoDepartamento
            {
                Coordenador = atividade.EhCoordenacao ? dtoDep : null,
                Participante = !atividade.EhCoordenacao ? dtoDep : null,
            };
        }
    }
}
