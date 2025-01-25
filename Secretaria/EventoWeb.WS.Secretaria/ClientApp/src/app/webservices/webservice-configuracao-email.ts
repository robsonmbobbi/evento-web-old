import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTOConfiguracaoEmail } from '../configuracao-email/objetos';

@Injectable()
export class WebServiceConfiguracaoEmail extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "configuracaoemail/");
  }

  obter(idEvento: number): Observable<DTOConfiguracaoEmail> {
    return this.executarGet<DTOConfiguracaoEmail>('evento/' + idEvento.toString() + '/obter/');
  };

  atualizar(idEvento: number, configuracao: DTOConfiguracaoEmail): Observable<void> {
    return this.executarPost('evento/' + idEvento.toString() + '/salvar/', configuracao);
  }
}
