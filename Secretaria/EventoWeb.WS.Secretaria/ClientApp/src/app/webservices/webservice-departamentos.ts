import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOId } from "../evento/objetos";
import { DTODepartamento } from '../departamentos/objetos';

@Injectable()
export class WebServiceDepartamentos extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "departamentos/");
  }

  obterTodos(idEvento: number): Observable<DTODepartamento[]> {
    return this.executarGet<DTODepartamento[]>('evento/' + idEvento.toString() + '/listarTodos');
  };

  obterId(idEvento: number, id: number): Observable<DTODepartamento> {
    return this.executarGet<DTODepartamento>('evento/' + idEvento.toString() + '/obter/' + id);
  };

  incluir(idEvento: number, sala: DTODepartamento): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', sala);
  }

  atualizar(idEvento: number, id: number, sala: DTODepartamento): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + id, sala);
  }

  excluir(idEvento: number, id: number): Observable<any> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + id);
  }
}
