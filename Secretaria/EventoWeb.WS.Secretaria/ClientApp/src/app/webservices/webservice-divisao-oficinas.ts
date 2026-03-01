import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTODivisaoOficina } from '../divisao-oficinas/objetos';

@Injectable()
export class WebServiceDivisaoOficinas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "divisaooficinas/");
  }

  obterTodas(idEvento: number): Observable<DTODivisaoOficina[]> {
    return this.executarGet<DTODivisaoOficina[]>('evento/' + idEvento.toString() + '/obter');
  };

  realizarDivisaoAutomatica(idEvento: number): Observable<DTODivisaoOficina[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/divisao-automatica', null);
  };

  removerTudo(idEvento: number, ): Observable<DTODivisaoOficina[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/remover-todas-divisoes/');
  }

  incluirInscricaoOficina(idEvento: number, idOficina: number, idInscricao: number): Observable<DTODivisaoOficina[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/oficina/' + idOficina + '/incluir', null);
  }

  moverInscricaoOficinas(idEvento: number, daIdOficina: number, paraIdOficina: number, idInscricao: number): Observable<DTODivisaoOficina[]> {
    return this.executarPut('evento/' + idEvento.toString() + '/mover-inscricao/' + idInscricao + '/da-oficina/' + daIdOficina + '/para-oficina/' + paraIdOficina, null);
  }

  removerInscricaoOficina(idEvento: number, idOficina: number, idInscricao: number): Observable<DTODivisaoOficina[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/oficina/' + idOficina + '/remover');
  }
}
