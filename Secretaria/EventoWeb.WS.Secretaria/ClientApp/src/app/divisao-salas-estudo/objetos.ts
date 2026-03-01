import { DTOBasicoInscricao } from '../inscricao/objetos';

export class DTODivisaoSalaEstudo {
  Id: number = 0;
  Nome: string = "";
  Coordenadores: DTOBasicoInscricao[] = [];
  Participantes: DTOBasicoInscricao[] = [];
}
