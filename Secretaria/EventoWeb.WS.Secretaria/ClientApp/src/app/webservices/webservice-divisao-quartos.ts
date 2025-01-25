import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTODivisaoQuarto } from '../divisao-quartos/objetos';

@Injectable()
export class WebServiceDivisaoQuartos extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "divisaoQuartos/");
  }

  obterTodas(idEvento: number): Observable<DTODivisaoQuarto[]> {
    return this.executarGet<DTODivisaoQuarto[]>('evento/' + idEvento.toString() + '/obter');
  };

  realizarDivisaoAutomatica(idEvento: number): Observable<DTODivisaoQuarto[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/divisao-automatica', null);
  };

  removerTudo(idEvento: number, ): Observable<DTODivisaoQuarto[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/remover-todas-divisoes/');
  }

  incluirInscricaoQuarto(idEvento: number, idQuarto: number, idInscricao: number): Observable<DTODivisaoQuarto[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/quarto/' + idQuarto + '/incluir', null);
  }

  moverInscricaoQuarto(idEvento: number, daIdQuarto: number, paraIdQuarto: number, idInscricao: number): Observable<DTODivisaoQuarto[]> {
    return this.executarPut('evento/' + idEvento.toString() + '/mover-inscricao/' + idInscricao + '/da-quarto/' + daIdQuarto + '/para-quarto/' + paraIdQuarto, null);
  }

  removerInscricaoQuarto(idEvento: number, idQuarto: number, idInscricao: number): Observable<DTODivisaoQuarto[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/quarto/' + idQuarto + '/remover');
  }

  definirSeEhCoordenador(idEvento: number, idQuarto: number, idInscricao: number, ehCoordenador: boolean): Observable<DTODivisaoQuarto[]> {
    return this.executarPut('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/quarto/' + idQuarto + '/coordenador/' + ehCoordenador, null);
  }
}
