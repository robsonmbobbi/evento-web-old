import { DTOBasicoInscricao } from '../inscricao/objetos';

export class DTODivisaoOficina {
  Id: number = 0;
  Nome: string = "";
  Coordenadores: DTOBasicoInscricao[] = [];
  Participantes: DTOBasicoInscricao[] = [];
}
