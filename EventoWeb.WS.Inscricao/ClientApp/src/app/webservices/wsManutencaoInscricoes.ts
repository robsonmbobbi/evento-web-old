import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOInscricaoCompleta, DTOSarau, DTOInscricaoSimplificada, DTOInscricaoCompletaInfantil } from '../inscricao/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';

@Injectable()
export class WsManutencaoInscricoes {

  constructor(private clienteWs: ClienteWs, private coordenacao: CoordenacaoCentral) { }

  obterInscricaoCompleta(idInscricao: number): Observable<DTOInscricaoCompleta> {

    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/obterInscricao/' + idInscricao.toString();
    this.clienteWs.TokenAutorizacao = this.coordenacao.AutorizacoesInscricao.obterAutorizacao(idInscricao);
    return this.clienteWs.executarGet("");
  }

  obterSarau(idEvento: number, codigo: string): Observable<DTOSarau> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/evento/' + idEvento.toString() + '/obterSarau/' + codigo;
    return this.clienteWs.executarGet("");
  }

  obterInscricaoAdultoSimplificadaPorCodigo(idEvento: number, codigo: string): Observable<DTOInscricaoSimplificada> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/evento/' + idEvento.toString() + '/obter-inscricao-adulto/' + codigo;
    return this.clienteWs.executarGet("");
  }

  obterInscricaoCompletaInfantil(idInscricao: number): Observable<DTOInscricaoCompletaInfantil> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'manutencaoinscricoes/obterInscricaoInfantil/' + idInscricao.toString();
    this.clienteWs.TokenAutorizacao = this.coordenacao.AutorizacoesInscricao.obterAutorizacao(idInscricao);
    return this.clienteWs.executarGet("");
  }
}
