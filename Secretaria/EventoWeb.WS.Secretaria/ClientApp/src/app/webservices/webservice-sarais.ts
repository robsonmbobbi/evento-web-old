import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOId } from "../evento/objetos";
import { DTOSarau } from '../sarais/objetos';

@Injectable()
export class WebServiceSarais extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "sarais/");
  }

  obterTodos(idEvento: number): Observable<DTOSarau[]> {
    return this.executarGet<DTOSarau[]>('evento/' + idEvento.toString() + '/listarTodos');
  };

  obterId(idEvento: number, id: number): Observable<DTOSarau> {
    return this.executarGet<DTOSarau>('evento/' + idEvento.toString() + '/obter/' + id);
  };

  incluir(idEvento: number, sarau: DTOSarau): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', sarau);
  }

  atualizar(idEvento: number, id: number, sarau: DTOSarau): Observable<void> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + id, sarau);
  }

  excluir(idEvento: number, id: number): Observable<any> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + id);
  }
}
