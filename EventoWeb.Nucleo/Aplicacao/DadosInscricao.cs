using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public enum EnumApresentacaoAtividades { ApenasParticipante, PodeEscolher }
    public enum EnumResultadoEnvio { InscricaoNaoEncontrada, EventoEncerradoInscricao, InscricaoOK, IdentificacaoInvalida }

    public class DTODadosConfirmacao
    {
        public int IdInscricao { get; set; }
        public string EnderecoEmail { get; set; }
    }

    public class DTOBasicoInscricao
    {
        public int IdInscricao { get; set; }
        public string NomeInscrito { get; set; }
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public EnumSituacaoInscricao Situacao { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Tipo { get; set; }
    }

    public class DTOBasicoInscricaoResp : DTOBasicoInscricao
    {
        public IList<DTOInscricaoSimplificada> Responsaveis { get; set; }
    }

    public class DTOAcessoInscricao
    {
        public int IdInscricao { get; set; }
        public string Autorizacao { get; set; }
    }

    public class DTOEnvioCodigoAcessoInscricao
    {
        public EnumResultadoEnvio Resultado { get; set; }
        public int? IdInscricao { get; set; }
    }

    public class DTOInscricaoDadosPessoais
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public SexoPessoa Sexo { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public Boolean EhVegetariano { get; set; }
        public Boolean UsaAdocanteDiariamente { get; set; }
        public Boolean EhDiabetico { get; set; }
        public string CarnesNaoCome { get; set; }
        public string AlimentosAlergia { get; set; }
        public string MedicamentosUsa { get; set; }
        public string Celular { get; set; }
        public string TelefoneFixo { get; set; }
    }

    public abstract class ADTOInscricaoAtualizacao<TSarau>
        where TSarau : DTOSarau
    {
        public DTOInscricaoDadosPessoais DadosPessoais { get; set; }
        public string NomeCracha { get; set; }
        public bool PrimeiroEncontro { get; set; }
        public string Observacoes { get; set; }
        public IList<TSarau> Sarais { get; set; }
        public DTOPagamento Pagamento { get; set; }
        public bool DormeEvento { get; set; }
    }

    public interface IDTOInscricaoCompleta
    {
        int Id { get; set; }
        DTOEventoCompletoInscricao Evento { get; set; }
        EnumSituacaoInscricao Situacao { get; set; }
    }

    public abstract class ADTOInscricaoAtualizacaoAdulto<TSarau> : ADTOInscricaoAtualizacao<TSarau>
        where TSarau : DTOSarau
    {
        public EnumTipoParticipante TipoInscricao { get; set; }
        public string CentroEspirita { get; set; }
        public string TempoEspirita { get; set; }
        public string NomeResponsavelCentro { get; set; }
        public string TelefoneResponsavelCentro { get; set; }
        public string NomeResponsavelLegal { get; set; }
        public string TelefoneResponsavelLegal { get; set; }
        public DTOInscricaoOficina Oficina { get; set; }
        public DTOInscricaoSalaEstudo SalasEstudo { get; set; }
        public DTOInscricaoDepartamento Departamento { get; set; }
    }

    public class DTOInscricaoAtualizacaoAdulto : ADTOInscricaoAtualizacaoAdulto<DTOSarau>
    {
    }

    public abstract class ADTOInscricaoCompletaAdulto<TSarau> : ADTOInscricaoAtualizacaoAdulto<TSarau>, IDTOInscricaoCompleta
        where TSarau : DTOSarau
    {
        public int Id { get; set; }
        public DTOEventoCompletoInscricao Evento { get; set; }
        public EnumSituacaoInscricao Situacao { get; set; }
    }

    public class DTOInscricaoCompletaAdulto : ADTOInscricaoCompletaAdulto<DTOSarau>
    {
    }

    public class DTOInscricaoCompletaAdultoCodigo : ADTOInscricaoCompletaAdulto<DTOSarauCodigo>
    {
        public string Codigo { get; set; } 
    }

    public class DTOInscricaoOficina
    {
        public DTOOficina Coordenador { get; set; }
        public IList<DTOOficina> EscolhidasParticipante { get; set; }
    }

    public class DTOInscricaoSalaEstudo
    {
        public DTOSalaEstudo Coordenador { get; set; }
        public IList<DTOSalaEstudo> EscolhidasParticipante { get; set; }
    }

    public class DTOInscricaoDepartamento
    {
        public DTODepartamento Coordenador { get; set; }
        public DTODepartamento Participante { get; set; }
    }

    public class DTOSarau
    {
        public int? Id { get; set; }
        public string Tipo { get; set; }
        public int DuracaoMin { get; set; }
        public IList<DTOInscricaoSimplificada> Participantes { get; set; }
    }

    public class DTOSarauCodigo : DTOSarau
    {
        public string Codigo { get; set; }
    }

    public class DTOInscricaoSimplificada
    {
        public int Id { get; set; }
        public int IdEvento { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }

    public abstract class ADTOInscricaoAtualizacaoInfantil<TSarau> : ADTOInscricaoAtualizacao<TSarau> where TSarau : DTOSarau
    {
        public DTOInscricaoSimplificada Responsavel1 { get; set; }
        public DTOInscricaoSimplificada Responsavel2 { get; set; }
    }

    public class DTOInscricaoAtualizacaoInfantil: ADTOInscricaoAtualizacaoInfantil<DTOSarau>
    {
    }

    public abstract class ADTOInscricaoCompletaInfantil<TSarau> : ADTOInscricaoAtualizacaoInfantil<TSarau>, IDTOInscricaoCompleta
        where TSarau : DTOSarau
    {
        public int Id { get; set; }
        public DTOEventoCompletoInscricao Evento { get; set; }
        public EnumSituacaoInscricao Situacao { get; set; }
    }

    public class DTOInscricaoCompletaInfantil : ADTOInscricaoCompletaInfantil<DTOSarau>
    {
    }

    public class DTOInscricaoCompletaInfantilCodigo : ADTOInscricaoCompletaInfantil<DTOSarauCodigo>
    {
        public string Codigo { get; set; }
    }

    public class DTOComprovantePagamento
    {
        public string Base64 { get; set; }
        public EnumTipoArquivoBinario TipoArquivo { get; set; }
    }

    public class DTOPagamento
    {
        public EnumPagamento? Forma { get; set; }
        public IList<DTOComprovantePagamento> Comprovantes { get; set; }
        public string Observacao { get; set; }
    }
}
