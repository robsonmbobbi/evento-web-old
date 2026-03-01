import { WebServiceBase } from "./webservice-base";
import { Injectable } from "@angular/core";
import { GestaoAutenticacao } from "../seguranca/gestao-autenticacao";
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { EnumSituacaoInscricao, DTOBasicoInscricao, DTOInscricaoCompleta, DTOInscricaoAtualizacao, DTOInscricaoCompletaInfantil, DTOInscricaoAtualizacaoInfantil } from "../inscricao/objetos";

@Injectable()
export class WebServiceInscricoes extends WebServiceBase {
    
  constructor(http: HttpClient, public override gestorAutenticacao: GestaoAutenticacao) {
    super(http, gestorAutenticacao, "inscricoes/");
  }

  obterTodas(idEvento: number, situacao: EnumSituacaoInscricao): Observable<DTOBasicoInscricao[]> {
    return this.executarGet<DTOBasicoInscricao[]>('evento/' + idEvento.toString() + '/listarTodas?&situacao=' + situacao);
  };

  excluir(idEvento: number, idInscricao: number): Observable<any> {
    return this.executarDelete('evento/' + idEvento.toString() + '/excluir/' + idInscricao.toString());
  }

  obterInscricaoCompleta(idEvento: number, idInscricao: number): Observable<DTOInscricaoCompleta> {
    return this.executarGet<DTOInscricaoCompleta>('evento/' + idEvento.toString() + '/obter/' + idInscricao.toString());
  }

  obterInscricaoCompletaInfantil(idEvento: number, idInscricao: number): Observable<DTOInscricaoCompletaInfantil> {
    return this.executarGet<DTOInscricaoCompletaInfantil>('evento/' + idEvento.toString() + '/obter-infantil/' + idInscricao.toString());
  }

  aceitar(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacao): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/aceitar/' + idInscricao.toString(), atualizacao);
  }

  aceitarInfantil(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacaoInfantil): Observable<void> {
    return this.executarPut('evento/' + idEvento.toString() + '/aceitar-infantil/' + idInscricao.toString(), atualizacao);
  }

  rejeitar(idEvento: number, idInscricao: number): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/rejeitar/' + idInscricao.toString(), null);
  }

  atualizar(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacao): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar/' + idInscricao.toString(), atualizacao);
  }

  atualizarInfantil(idEvento: number, idInscricao: number, atualizacao: DTOInscricaoAtualizacaoInfantil): Observable<any> {
    return this.executarPut('evento/' + idEvento.toString() + '/atualizar-infantil/' + idInscricao.toString(), atualizacao);
  }

  incluir(idEvento: number, inscricao: DTOInscricaoAtualizacao): Observable<void> {
    return this.executarPost('evento/' + idEvento.toString() + '/incluir/', inscricao);
  }

  incluirInfantil(idEvento: number, inscricao: DTOInscricaoAtualizacaoInfantil): Observable<void> {
    return this.executarPost('evento/' + idEvento.toString() + '/incluir-infantil/', inscricao);
  }
}
