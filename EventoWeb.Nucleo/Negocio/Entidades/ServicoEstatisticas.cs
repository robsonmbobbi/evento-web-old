using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class EstatisticaTipoInscricao
    {
        public int Criancas { get; set; }
        public int Participantes { get; set; }
        public int ParticipantesTrabalhadores { get; set; }
        public int Trabalhadores { get; set; }

        public int CriancasPresentes { get; set; }
        public int ParticipantesPresentes { get; set; }
        public int ParticipantesTrabalhadoresPresentes { get; set; }
        public int TrabalhadoresPresentes { get; set; }
    }

    public class EstatisticaSexo
    {
        public int Homens { get; set; }
        public int Mulheres { get; set; }

        public int HomensPresentes { get; set; }
        public int MulheresPresentes { get; set; }
    }

    public class EstatisticaVegetariano
    {
        public int Sao { get; set; }
        public int NaoSao { get; set; }

        public int SaoPresentes { get; set; }
        public int NaoSaoPresentes { get; set; }
    }

    public class EstatisticaAdocante
    {
        public int Usam { get; set; }
        public int NaoUsam { get; set; }

        public int UsamPresentes { get; set; }
        public int NaoUsamPresentes { get; set; }
    }

    public class EstatisticaDiabeticos
    {
        public int Sao { get; set; }
        public int NaoSao { get; set; }

        public int SaoPresentes { get; set; }
        public int NaoSaoPresentes { get; set; }
    }

    public class EstatisticaEvangelizacao
    {
        public int NumeroMeninas { get; set; }
        public int NumeroMeninos { get; set; }
        public int NumeroCriancas0a3Anos { get; set; }
        public int NumeroCriancas4a6Anos { get; set; }
        public int NumeroCriancas7a9Anos { get; set; }
        public int NumeroCriancas10a12Anos { get; set; }

        public int NumeroMeninasPresentes { get; set; }
        public int NumeroMeninosPresentes { get; set; }
        public int NumeroCriancas0a3AnosPresentes { get; set; }
        public int NumeroCriancas4a6AnosPresentes { get; set; }
        public int NumeroCriancas7a9AnosPresentes { get; set; }
        public int NumeroCriancas10a12AnosPresentes { get; set; }
    }

    public class EstatisticaCidades
    {
        public String Cidade { get; set; }
        public int NumeroInscricoes { get; set; }
    }

    public class EstatisticaGeral
    {
        public int TotalInscricoes { get; set; }
        public int TotalInscricoesPresentes { get; set; }
        public int TotalInscricoesNaoDormem { get; set; }
        public int TotalInscricoesNaoDormemPresentes { get; set; }
        public EstatisticaTipoInscricao TiposInscricao { get; set; }
        public EstatisticaSexo Sexo { get; set; }
        public EstatisticaVegetariano Vegetarianos { get; set; }
        public EstatisticaAdocante UsamAdocante { get; set; }
        public EstatisticaDiabeticos Diabeticos { get; set; }
        public EstatisticaEvangelizacao Evangelizacao { get; set; }
        public IEnumerable<String> Alergias { get; set; }
        public IEnumerable<EstatisticaCidades> InscritosCidade { get; set; }
    }

    public class ServicoEstatisticas
    {
        private int mIdEvento;
        private AInscricoes mRepositorio;

        public ServicoEstatisticas(AInscricoes repositorio, int idEvento)
        {
            mRepositorio = repositorio;
            mIdEvento = idEvento;
        }

        public EstatisticaGeral GerarEstatisticas()
        {
            var inscricoes = mRepositorio.ListarTodasPorEventoESituacao(mIdEvento, EnumSituacaoInscricao.Aceita);
            var estatistica = new EstatisticaGeral();
            estatistica.TotalInscricoes = inscricoes.Count;
            estatistica.TotalInscricoesPresentes = inscricoes.Count(x => x.ConfirmadoNoEvento);

            estatistica.TotalInscricoesNaoDormem = inscricoes.Count(x => !x.DormeEvento);
            estatistica.TotalInscricoesNaoDormemPresentes = inscricoes.Count(x => x.ConfirmadoNoEvento && !x.DormeEvento);

            estatistica.TiposInscricao = GerarEstatisticasTiposInscricao(inscricoes);
            estatistica.Sexo = GerarEstatisticasSexo(inscricoes);
            estatistica.Vegetarianos = GerarEstatisticasVegetarianos(inscricoes);
            estatistica.UsamAdocante = GerarEstatisticasUsamAdocante(inscricoes);
            estatistica.Diabeticos = GerarEstatisticasDiabeticos(inscricoes);
            estatistica.Alergias = GerarEstatisticasAlergias(inscricoes);
            estatistica.Evangelizacao = GerarEstatisticasEvangelizacao(inscricoes);
            estatistica.InscritosCidade = GerarEstatisticasInscritosCidade(inscricoes);

            return estatistica;
        }

        private EstatisticaTipoInscricao GerarEstatisticasTiposInscricao(IList<Inscricao> inscricoes)
        {
            var inscricoesPorTipo = inscricoes.GroupBy(x => x.GetType());
            var estatistica = new EstatisticaTipoInscricao();
            foreach(var tipo in inscricoesPorTipo)
            {
                if (tipo.Key == typeof(InscricaoInfantil))
                {
                    estatistica.Criancas = tipo.Count();
                    estatistica.CriancasPresentes = tipo.Count(x => x.ConfirmadoNoEvento);
                }
                else if (tipo.Key == typeof(InscricaoParticipante))
                {
                    estatistica.Participantes = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.Participante);
                    estatistica.ParticipantesTrabalhadores = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.ParticipanteTrabalhador);
                    estatistica.Trabalhadores = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.Trabalhador);

                    estatistica.ParticipantesPresentes = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.Participante && x.ConfirmadoNoEvento);
                    estatistica.ParticipantesTrabalhadoresPresentes = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.ParticipanteTrabalhador && x.ConfirmadoNoEvento);
                    estatistica.TrabalhadoresPresentes = tipo.Count(x => ((InscricaoParticipante)x).Tipo == EnumTipoParticipante.Trabalhador && x.ConfirmadoNoEvento);
                }
            }

            return estatistica;
        }

        private EstatisticaSexo GerarEstatisticasSexo(IList<Inscricao> inscricoes)
        {
            var inscricoesPorSexo = inscricoes.GroupBy(x => x.Pessoa.Sexo);
            var estatistica = new EstatisticaSexo();
            foreach (var sexo in inscricoesPorSexo)
            {
                if (sexo.Key == SexoPessoa.Feminino)
                {
                    estatistica.Mulheres = sexo.Count();
                    estatistica.MulheresPresentes = sexo.Count(x => x.ConfirmadoNoEvento);
                }
                else if (sexo.Key == SexoPessoa.Masculino)
                {
                    estatistica.Homens = sexo.Count();
                    estatistica.HomensPresentes = sexo.Count(x => x.ConfirmadoNoEvento);
                }
            }

            return estatistica;
        }

        private EstatisticaVegetariano GerarEstatisticasVegetarianos(IList<Inscricao> inscricoes)
        {
            var grupo = inscricoes.GroupBy(x => x.Pessoa.EhVegetariano);
            var estatistica = new EstatisticaVegetariano();
            foreach (var item in grupo)
            {
                if (item.Key)
                {
                    estatistica.Sao = item.Count();
                    estatistica.SaoPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
                else
                {
                    estatistica.NaoSao = item.Count();
                    estatistica.NaoSaoPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
            }

            return estatistica;
        }

        private EstatisticaAdocante GerarEstatisticasUsamAdocante(IList<Inscricao> inscricoes)
        {
            var grupo = inscricoes.GroupBy(x => x.Pessoa.UsaAdocanteDiariamente);
            var estatistica = new EstatisticaAdocante();
            foreach (var item in grupo)
            {
                if (item.Key)
                {
                    estatistica.Usam = item.Count();
                    estatistica.UsamPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
                else
                {
                    estatistica.NaoUsam = item.Count();
                    estatistica.NaoUsamPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
            }

            return estatistica;
        }

        private EstatisticaDiabeticos GerarEstatisticasDiabeticos(IList<Inscricao> inscricoes)
        {
            var grupo = inscricoes.GroupBy(x => x.Pessoa.EhDiabetico);
            var estatistica = new EstatisticaDiabeticos();
            foreach (var item in grupo)
            {
                if (item.Key)
                {
                    estatistica.Sao = item.Count();
                    estatistica.SaoPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
                else
                {
                    estatistica.NaoSao = item.Count();
                    estatistica.NaoSaoPresentes = item.Count(x => x.ConfirmadoNoEvento);
                }
            }

            return estatistica;
        }

        private IEnumerable<string> GerarEstatisticasAlergias(IList<Inscricao> inscricoes)
        {
            return inscricoes
                .Where(x => !String.IsNullOrWhiteSpace(x.Pessoa.AlergiaAlimentos))
                .GroupBy(x => x.Pessoa.AlergiaAlimentos.ToUpper())
                .Select(x => x.Key)
                .OrderBy(x => x);
        }

        private EstatisticaEvangelizacao GerarEstatisticasEvangelizacao(IList<Inscricao> inscricoes)
        {
            var criancas = inscricoes
                .Where(x => x is InscricaoInfantil);

            var estatistica = new EstatisticaEvangelizacao();

            var lista = criancas.Where(x => x.Pessoa.Sexo == SexoPessoa.Feminino);
            estatistica.NumeroMeninas = lista.Count();
            estatistica.NumeroMeninasPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            lista = criancas.Where(x => x.Pessoa.Sexo == SexoPessoa.Masculino);
            estatistica.NumeroMeninos = lista.Count();
            estatistica.NumeroMeninosPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            lista = criancas.Where(x => x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) >= 0 && x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) <= 3);
            estatistica.NumeroCriancas0a3Anos = lista.Count();
            estatistica.NumeroCriancas0a3AnosPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            lista = criancas.Where(x => x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) >= 4 && x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) <= 6);
            estatistica.NumeroCriancas4a6Anos = lista.Count();
            estatistica.NumeroCriancas4a6AnosPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            lista = criancas.Where(x => x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) >= 7 && x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) <= 9);
            estatistica.NumeroCriancas7a9Anos = lista.Count();
            estatistica.NumeroCriancas7a9AnosPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            lista = criancas.Where(x => x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) >= 10 && x.Pessoa.CalcularIdadeEmAnos(x.Evento.PeriodoRealizacaoEvento.DataInicial) <= 12);
            estatistica.NumeroCriancas10a12Anos = lista.Count();
            estatistica.NumeroCriancas10a12AnosPresentes = lista.Count(x => x.ConfirmadoNoEvento);

            return estatistica;
        }

        private IEnumerable<EstatisticaCidades> GerarEstatisticasInscritosCidade(IList<Inscricao> inscricoes)
        {
            var grupo = inscricoes.GroupBy(x => x.Pessoa.Endereco.Cidade.ToUpper());
            var lista = new List<EstatisticaCidades>();
            foreach(var cidade in grupo)
            {
                var estatistica = new EstatisticaCidades();
                estatistica.Cidade = cidade.Key;
                estatistica.NumeroInscricoes = cidade.Count();
                lista.Add(estatistica);
            }

            return lista.OrderBy(x=>x.Cidade);
        }
    }
}
