import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOId } from "../evento/objetos";
import { DTOOficina } from '../oficinas/objetos';

@Injectable()
export class WebServiceOficinas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "oficinas/");
  }

  obterTodas(idEvento: number): Observable<DTOOficina[]> {
    return this.executarGet<DTOOficina[]>('evento/' + idEvento.toString() + '/listarTodas');
  };

  obterId(idEvento: number, id: number): Observable<DTOOficina> {
    return this.executarGet<DTOOficina>('evento/' + idEvento.toString() + '/obter/' + id);
  };

  incluir(idEvento: number, oficina: DTOOficina): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', oficina);
  }

  atualizar(idEvento: number, id: number, oficina: DTOOficina): Observable<void> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + id, oficina);
  }

  excluir(idEvento: number, id: number): Observable<void> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + id);
  }
}
