export enum TipoSegurancaEmail { SSL, Nenhuma }

export class DTOConfiguracaoEmail {
  public EnderecoEmail: string = "";

  public UsuarioEmail: string = ""

  public SenhaEmail: string = ""

  public ServidorEmail: string = ""

  public PortaServidor: number = 0

  public TipoSeguranca: TipoSegurancaEmail | null = null;
}
