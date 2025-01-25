import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOId } from "../evento/objetos";
import { DTOQuarto } from '../quartos/objetos';

@Injectable()
export class WebServiceQuartos extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "quartos/");
  }

  obterTodos(idEvento: number): Observable<DTOQuarto[]> {
    return this.executarGet<DTOQuarto[]>('evento/' + idEvento.toString() + '/listarTodos');
  };

  obterId(idEvento: number, id: number): Observable<DTOQuarto> {
    return this.executarGet<DTOQuarto>('evento/' + idEvento.toString() + '/obter/' + id);
  };

  incluir(idEvento: number, sala: DTOQuarto): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', sala);
  }

  atualizar(idEvento: number, id: number, sala: DTOQuarto): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + id, sala);
  }

  excluir(idEvento: number, id: number): Observable<any> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + id);
  }
}
