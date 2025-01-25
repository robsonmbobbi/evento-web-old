import { DTOId } from "../evento/objetos";

export class DTOContratoInscricao extends DTOId
{
  Regulamento: string = "";
  InstrucoesPagamento: string = "";
  PassoAPassoInscricao: string = "";
}
