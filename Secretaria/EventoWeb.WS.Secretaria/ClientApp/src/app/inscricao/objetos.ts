import { DTOEventoCompletoInscricao } from "../evento/objetos";
import { DTOSalaEstudo } from "../sala-estudo/objetos";
import { DTOOficina } from "../oficinas/objetos";
import { DTODepartamento } from "../departamentos/objetos";
import { DTOSarau } from '../sarais/objetos';

export enum EnumSituacaoInscricao { Incompleta, Pendente, Aceita, Rejeitada }
export enum EnumSexo { Masculino, Feminino }
export enum EnumTipoInscricao { Participante, ParticipanteTrabalhador, Trabalhador }

export enum EnumApresentacaoAtividades { ApenasParticipante, PodeEscolher }

export class DTOBasicoInscricao {
  IdInscricao: number;
  NomeInscrito: string;
  IdEvento: number;
  NomeEvento: string;
  Email: string;
  Cidade: string;
  UF: string;
  Situacao: EnumSituacaoInscricao;
  DataNascimento: Date;
  Tipo: string;
}

export class DTOBasicoInscricaoResp extends DTOBasicoInscricao {
  Responsaveis: DTOInscricaoSimplificada[];
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
  Evento: DTOEventoCompletoInscricao;
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

export class DTOInscricaoSimplificada {
  Id: number;
  IdEvento: number;
  Nome: string;
  Cidade: string;
  UF: string;
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
  Evento: DTOEventoCompletoInscricao;
  Situacao: EnumSituacaoInscricao;
}

export enum EnumPagamento { Comprovante, ComprovanteOutraInscricao, Outro }

export enum EnumTipoArquivoBinario { PDF, ImagemPNG, ImagemJPEG }

export class DTOComprovantePagamento {
  Base64: string;
  TipoArquivo: EnumTipoArquivoBinario;
}

export class DTOPagamento {

  Forma: EnumPagamento;
  Comprovantes: DTOComprovantePagamento[];
  Observacao: string;
}
