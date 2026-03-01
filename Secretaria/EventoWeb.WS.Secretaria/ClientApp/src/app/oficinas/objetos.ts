import { DTOId } from "../evento/objetos";

export class DTOOficina extends DTOId {
  public Nome!: string;
  public DeveSerParNumeroTotalParticipantes!: boolean;
  public NumeroTotalParticipantes!: number;
}
