import { DTOUsuario } from "../usuarios/objetos";

export class DTWDadosAutenticacao {
  Login: string | undefined | null;
  Senha: string | undefined | null;
}

export class DTWAutenticacao {
  Usuario: DTOUsuario | undefined | null;
  TokenAutenticacao: string | undefined | null;
  Validade: Date | undefined | null;
}
