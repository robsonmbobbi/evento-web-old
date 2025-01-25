import { DTOId } from "../evento/objetos";
import { DTOInscricaoSimplificada } from '../inscricao/objetos';

export class DTOSarau extends DTOId {
 
  Tipo!: string;
  DuracaoMin!: number;
  Participantes!: DTOInscricaoSimplificada[];
}
