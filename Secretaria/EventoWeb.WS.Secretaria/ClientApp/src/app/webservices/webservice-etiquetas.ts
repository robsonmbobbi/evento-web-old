import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { CrachaInscrito } from "../etiquetas/objetos";

@Injectable()
export class WebServiceEtiquetas extends WebServiceBase {

  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "etiquetas/");
  }

  obterTodos(idEvento: number): Observable<CrachaInscrito[]> {
    return this.executarGet<CrachaInscrito[]>('evento/' + idEvento.toString() + '/listagem');
  };

  gerarEtiquetasCracha(inscritos: CrachaInscrito[]): Observable<Blob> {
    return this.executarPutBlob('geracao-cracha', inscritos);
  };

  gerarEtiquetasCaderno(inscritos: CrachaInscrito[]): Observable<Blob> {
    return this.executarPutBlob('geracao-caderno', inscritos);
  }
}
