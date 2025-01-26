import { DTOOficina, DTOEventoCompleto, DTOSalaEstudo, DTODepartamento } from '../principal/objetos';

export enum EnumSexo { Masculino, Feminino }
export enum EnumTipoInscricao { Participante, ParticipanteTrabalhador, Trabalhador }
export enum EnumApresentacaoAtividades {
    ApenasParticipante, PodeEscolher
}
export enum EnumSituacaoInscricao { Incompleta, Pendente, Aceita, Rejeitada }

export class DTODadosConfirmacao {
    public IdInscricao: number;
    public EnderecoEmail: string;
}

export class DTOBasicoInscricao {
    public IdInscricao: number;
    public NomeInscrito: string;
    public IdEvento: number;
    public NomeEvento: string;
    public Email: string;
}

export class DTOAcessoInscricao {
    public IdInscricao: number;
    public Autorizacao: string;
}

export enum EnumResultadoEnvio { InscricaoNaoEncontrada, EventoEncerradoInscricao, InscricaoOK }

export class DTOEnvioCodigoAcessoInscricao {
  public Resultado: EnumResultadoEnvio;
  public IdInscricao: number;
}

export class DTOInscricaoDadosPessoais {
    Nome: string;
    DataNascimento: Date;
    Email: string;
    Sexo: EnumSexo;
    Cidade: string;
    Uf: string;
    EhVegetariano: boolean;
    UsaAdocanteDiariamente: boolean;
    EhDiabetico: boolean;
    AlimentosAlergia: string;
    Celular: string;
}

export class DTOInscricaoAtualizacaoInfantil {
  DadosPessoais: DTOInscricaoDadosPessoais;
  NomeCracha: string;
  Observacoes: string;
  DormeEvento: boolean;

  Sarais: DTOSarau[];
  Responsavel1: DTOInscricaoSimplificada;
  Responsavel2: DTOInscricaoSimplificada;

  Pagamento: DTOPagamento;
}

export class DTOInscricaoCompletaInfantil extends DTOInscricaoAtualizacaoInfantil {
  Id: number;
  Evento: DTOEventoCompleto;
  Situacao: EnumSituacaoInscricao;
}

export class DTOInscricaoAtualizacao {
    DadosPessoais: DTOInscricaoDadosPessoais;
    TipoInscricao: EnumTipoInscricao;
    NomeCracha: string;
    CentroEspirita: string;
    Observacoes: string;
    DormeEvento: boolean;


    Oficina: DTOInscricaoOficina;
    SalasEstudo: DTOInscricaoSalaEstudo;
    Departamento: DTOInscricaoDepartamento;
    Sarais: DTOSarau[];

    Pagamento: DTOPagamento;
}

export class DTOInscricaoCompleta extends DTOInscricaoAtualizacao {
    Id: number;
    Evento: DTOEventoCompleto;
    Situacao: EnumSituacaoInscricao;
}

export class DTOInscricaoOficina {
    Coordenador: DTOOficina;
    EscolhidasParticipante: DTOOficina[];
}

export class DTOInscricaoSalaEstudo {
    Coordenador: DTOSalaEstudo;
    EscolhidasParticipante: DTOSalaEstudo[];
}

export class DTOInscricaoDepartamento {
    Coordenador: DTODepartamento;
    Participante: DTODepartamento;
}

export class DTOSarau {
    Id: number;
    Tipo: string;
    DuracaoMin: number;
    Participantes: DTOInscricaoSimplificada[];
}

export class DTOInscricaoSimplificada {
    Id: number;
    IdEvento: number;
    Nome: string;
}

export enum EnumPagamento { Comprovante, ComprovanteOutraInscricao, Outro }

export class DTOPagamento {

    Forma: EnumPagamento;
    ComprovantesBase64: string[];
    Observacao: string;
}
