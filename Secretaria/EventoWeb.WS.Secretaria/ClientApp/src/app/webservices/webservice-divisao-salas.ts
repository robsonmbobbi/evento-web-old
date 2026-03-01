import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { DTODivisaoSalaEstudo } from '../divisao-salas-estudo/objetos';

@Injectable()
export class WebServiceDivisaoSalas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "divisaosalas/");
  }

  obterTodas(idEvento: number): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarGet<DTODivisaoSalaEstudo[]>('evento/' + idEvento.toString() + '/obter');
  };

  realizarDivisaoAutomatica(idEvento: number): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/divisao-automatica', null);
  };

  removerTudo(idEvento: number, ): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/remover-todas-divisoes/');
  }

  incluirInscricaoSala(idEvento: number, idSala: number, idInscricao: number): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarPost('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/sala/' + idSala + '/incluir', null);
  }

  moverInscricaoSalas(idEvento: number, daIdSala: number, paraIdSala: number, idInscricao: number): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarPut('evento/' + idEvento.toString() + '/mover-inscricao/' + idInscricao + '/da-sala/' + daIdSala + '/para-sala/' + paraIdSala, null);
  }

  removerInscricaoSala(idEvento: number, idSala: number, idInscricao: number): Observable<DTODivisaoSalaEstudo[]> {
    return this.executarDelete('evento/' + idEvento.toString() + '/inscricao/' + idInscricao + '/sala/' + idSala + '/remover');
  }
}
