import { DTOBasicoInscricaoResp } from '../inscricao/objetos';
import { EnumSexoQuarto } from '../quartos/objetos';

export class DTODivisaoQuarto {
  Id: number = 0;
  Nome: string = "";
  EhFamilia: boolean = false;
  Sexo: EnumSexoQuarto = EnumSexoQuarto.Misto;
  Capacidade: number | null = null;
  Coordenadores: DTOBasicoInscricaoResp[] = [];
  Participantes: DTOBasicoInscricaoResp[] = [];
}
