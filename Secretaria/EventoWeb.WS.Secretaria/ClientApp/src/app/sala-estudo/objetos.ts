import { DTOId } from "../evento/objetos";

export class DTOSalaEstudo extends DTOId {
  public Nome!: string;
  public DeveSerParNumeroTotalParticipantes!: boolean;
  public IdadeMinima!: number;
  public IdadeMaxima!: number;
}
