import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOEstatisticaGeral } from '../estatisticas/objetos';

@Injectable()
export class WebServiceEstatisticas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "estatisticas/");
  }

  obter(idEvento: number): Observable<DTOEstatisticaGeral> {
    return this.executarGet<DTOEstatisticaGeral>('evento/' + idEvento.toString());
  };
}
