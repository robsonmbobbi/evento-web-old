import { DTOId } from "../evento/objetos";

export enum EnumSexoQuarto { Masculino, Feminino, Misto }

export class DTOQuarto extends DTOId {
  public Nome!: string;
  public EhFamilia!: boolean;
  public Capacidade!: number;
  public Sexo!: EnumSexoQuarto;
}
