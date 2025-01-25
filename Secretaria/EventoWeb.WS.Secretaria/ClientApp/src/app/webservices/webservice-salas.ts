import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOSalaEstudo } from "../sala-estudo/objetos";
import { DTOId } from "../evento/objetos";

@Injectable()
export class WebServiceSalas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "salas/");
  }

  obterTodas(idEvento: number): Observable<DTOSalaEstudo[]> {
    return this.executarGet<DTOSalaEstudo[]>('evento/' + idEvento.toString() + '/listarTodas');
  };

  obterId(idEvento: number, id: number): Observable<DTOSalaEstudo> {
    return this.executarGet<DTOSalaEstudo>('evento/' + idEvento.toString() + '/obter/' + id);
  };

  incluir(idEvento: number, sala: DTOSalaEstudo): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', sala);
  }

  atualizar(idEvento: number, id: number, sala: DTOSalaEstudo): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + id, sala);
  }

  excluir(idEvento: number, id: number): Observable<any> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + id);
  }
}
