export class DTOUsuario {
  Login: string | undefined | null;
  Nome: string | undefined | null;
  EhAdministrador: boolean | undefined | null;
}

export class DTOUsuarioInclusao extends DTOUsuario
{
  Senha: string | undefined | null;
  RepeticaoSenha: string | undefined | null;
}

export class DTOAlteracaoSenhaWS {
  NovaSenha: string | undefined | null;
  NovaSenhaRepetida: string | undefined | null;
}

export class DTOAlteracaoSenhaComumWS extends DTOAlteracaoSenhaWS
{
  SenhaAtual: string | undefined | null;
}
