import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { WebServiceBase } from './webservice-base';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { DTWAutenticacao, DTWDadosAutenticacao } from '../seguranca/objetos';

@Injectable()
export class WebServiceAutenticacao extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "autenticacao/");
  }

  autenticar(dadosAutenticar: DTWDadosAutenticacao): Observable<DTWAutenticacao> {
    return this.executarPut('autenticar', dadosAutenticar);
  };

  desautenticar(): Observable<any> {
    return this.executarDelete('desautenticar/');
  }
}
