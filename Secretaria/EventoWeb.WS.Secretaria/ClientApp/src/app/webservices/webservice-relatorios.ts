import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable()
export class WebServiceRelatorios extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "relatorios/");
  }

  obterDivisaoSalas(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/divisao-salas', null);
  };

  obterDivisaoOficinas(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/divisao-oficinas', null);
  }

  obterDivisaoQuartos(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/divisao-quartos', null);
  }

  obterInscritosDepartamentos(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/inscritos-departamentos', null);
  }

  obterListagemSarau(idEvento: number): Observable<Blob> {
    return this.executarPutBlob('evento/' + idEvento.toString() + '/listagem-sarau', null);
  }
}
