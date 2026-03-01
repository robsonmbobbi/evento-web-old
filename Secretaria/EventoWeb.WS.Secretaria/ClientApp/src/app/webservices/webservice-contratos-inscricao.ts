import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOId } from "../evento/objetos";
import { DTOContratoInscricao } from '../contratos-inscricao/objetos';

@Injectable()
export class WebServiceContratosInscricao extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "contratosinscricao/");
  }

  obter(idEvento: number): Observable<DTOContratoInscricao> {
    return this.executarGet<DTOContratoInscricao>('evento/' + idEvento.toString() + '/obter/');
  };

  incluir(idEvento: number, contrato: DTOContratoInscricao): Observable<DTOId> {
    return this.executarPost('evento/' + idEvento.toString() + '/criar/', contrato);
  }

  atualizar(idEvento: number, contrato: DTOContratoInscricao): Observable<void> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/', contrato);
  }

  excluir(idEvento: number): Observable<void> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/');
  }
}
