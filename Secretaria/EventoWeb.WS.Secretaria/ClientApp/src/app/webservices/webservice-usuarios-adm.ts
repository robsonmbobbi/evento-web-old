import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOAlteracaoSenhaWS, DTOUsuario, DTOUsuarioInclusao } from "../usuarios/objetos";

@Injectable()
export class WebServiceUsuariosAdm extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "usuariosadm/");
  }

  listarTodos(): Observable<DTOUsuario[]> {
    return this.executarGet<DTOUsuario[]>('listar-todos');
  };

  obter(login: string): Observable<DTOUsuario> {
    return this.executarGet<DTOUsuario>('obter/' + login);
  };

  incluir(usuario: DTOUsuarioInclusao): Observable<void> {
    return this.executarPost('incluir', usuario);
  }

  atualizar(usuario: DTOUsuario): Observable<void> {
    return this.executarPut('atualizar/', usuario);
  }

  excluir(login: string): Observable<void> {
    return this.executarDelete('excluir/' + login);
  }

  atualizarSenha(login: string, dadosAlteracaoSenha: DTOAlteracaoSenhaWS): Observable<void> {
    return this.executarPut('atualizar-senha/' + login, dadosAlteracaoSenha);
  }
}
