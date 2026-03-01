import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTOEventoListagem, DTOContratoInscricao, DTOEventoCompleto } from '../principal/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';

@Injectable()
export class WsEventos {
    

  constructor(private clienteWs: ClienteWs) { }

  public listarEventosDisponiveisInscricao(): Observable<DTOEventoListagem[]> {

    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/disponiveis';
    return this.clienteWs.executarGet("");
  }

  public obter(idEvento: number): Observable<DTOEventoListagem> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/' + idEvento.toString();
    return this.clienteWs.executarGet("");
  }

  public obterContrato(idEvento: number): Observable<DTOContratoInscricao> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/' + idEvento.toString()  + "/contrato";
    return this.clienteWs.executarGet("");
  }

  obterCompleto(idEvento: number): Observable<DTOEventoCompleto> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'eventos/' + idEvento.toString() + "/completo";
    return this.clienteWs.executarGet("");
  }
}
