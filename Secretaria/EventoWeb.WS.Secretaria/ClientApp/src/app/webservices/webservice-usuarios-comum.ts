import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOAlteracaoSenhaWS, DTOUsuario, DTOUsuarioInclusao } from "../usuarios/objetos";

@Injectable()
export class WebServiceUsuariosComum extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "usuarioscomum/");
  }

  obter(): Observable<DTOUsuario> {
    return this.executarGet<DTOUsuario>('obter');
  };

  atualizar(usuario: DTOUsuario): Observable<void> {
    return this.executarPut('atualizar', usuario);
  }

  atualizarSenha(dadosAlteracaoSenha: DTOAlteracaoSenhaWS): Observable<void> {
    return this.executarPut('atualizar-senha/', dadosAlteracaoSenha);
  }
}
