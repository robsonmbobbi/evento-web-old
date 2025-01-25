import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOMensagemEmailInscricao } from '../mensagens-email-inscricao/objetos';

@Injectable()
export class WebServiceMensagensInscricao extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "mensagensemailinscricao/");
  }

  obter(idEvento: number): Observable<DTOMensagemEmailInscricao> {
    return this.executarGet<DTOMensagemEmailInscricao>('evento/' + idEvento.toString() + '/obter/');
  };

  atualizar(idEvento: number, contrato: DTOMensagemEmailInscricao): Observable<void> {
    return this.executarPost('evento/' + idEvento.toString() + '/atualizar/', contrato);
  }
}
