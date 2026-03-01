import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { DTODadosConfirmacao, DTOBasicoInscricao, DTOAcessoInscricao, DTOEnvioCodigoAcessoInscricao, DTOInscricaoAtualizacao, DTOInscricaoAtualizacaoInfantil } from '../inscricao/objetos';
import { ClienteWs } from './cliente-ws';
import { ConfiguracaoSistemaService } from '../configuracao-sistema-service';

@Injectable()
export class WsInscricoes {

  constructor(private clienteWs: ClienteWs) { }

  public obterBasicoInscricao(idInscricao: number): Observable<DTOBasicoInscricao> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/basicoPorId/' + idInscricao.toString();
    return this.clienteWs.executarGet("");
  }

  public enviarCodigoAcesso(identificacao: string): Observable<DTOEnvioCodigoAcessoInscricao> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/enviarCodigo/' + identificacao.toString();
    return this.clienteWs.executarPut("", null);
  }

  public criar(idEvento: number, inscricao: DTOInscricaoAtualizacao): Observable<DTODadosConfirmacao> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/criar/' + idEvento.toString();
    return this.clienteWs.executarPost("", inscricao);
  }

  public validarAcessoInscricao(IdInscricao: number, codigo: string): Observable<DTOAcessoInscricao> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/validarCodigo/' + IdInscricao.toString();
    return this.clienteWs.executarPut("", "'" + codigo + "'");
  }

  enviarCodigoValidacaoEmail(idEvento: number, identificacao: string, email: string, celular: string): Observable<void> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/enviarCodigoEmail/' + idEvento;
    return this.clienteWs.executarPut("", "{Identificacao:'" + identificacao + "', Email:'" + email + "', Whatsapp: '" + celular + "'}");
  }

  validarCodigoEmail(identificacao: string, codigo: string): Observable<boolean> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/validarCodigoEmail/';
    return this.clienteWs.executarPut("", "{Identificacao:'" + identificacao + "', Codigo:'" + codigo + "'}");
  }

  criarInfantil(idEvento: number, inscricao: DTOInscricaoAtualizacaoInfantil): Observable<void> {
    this.clienteWs.URLWs = ConfiguracaoSistemaService.configuracao.urlBaseWs + 'inscricoes/criar-infantil/' + idEvento.toString();
    return this.clienteWs.executarPost("", inscricao);
  }
}
