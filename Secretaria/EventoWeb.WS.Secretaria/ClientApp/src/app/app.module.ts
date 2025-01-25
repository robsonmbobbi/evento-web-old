import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID, APP_INITIALIZER, Injectable } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MAT_DATE_LOCALE } from '@angular/material/core';

import { FlexLayoutModule } from "@angular/flex-layout";

import { RichTextEditorAllModule } from '@syncfusion/ej2-angular-richtexteditor';

import {
  DxDataGridModule,
  DxTextBoxModule,
  DxNumberBoxModule,
  DxDateBoxModule,
  DxDropDownBoxModule,
  DxListModule,
  DxRadioGroupModule,
  DxCheckBoxModule,
  DxPopoverModule,
  DxTextAreaModule,
  DxValidatorModule,
  DxValidationGroupModule,
  DxSelectBoxModule,
  DxFileUploaderModule,
  DxGalleryModule
} from 'devextreme-angular';

import { locale, loadMessages } from 'devextreme/localization';

import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { DlgEmProcessamento, Alertas } from './componentes/alertas-dlg/alertas';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';
import { MenuUsuario } from './componentes/menu-usuario/menu-usuario';

import { WebServiceAutenticacao } from './webservices/webservice-autenticacao';
import { WebServiceEventos } from './webservices/webservice-eventos';

import { PermissaoAcessoRota } from './seguranca/permissao-acesso-rota';
import { GestaoAutenticacao } from './seguranca/gestao-autenticacao';

import { TelaLogin } from './login/tela-login';
import { TelaPrincipal } from './tela-principal';

import { TelaListaEventos } from './evento/tela-lista-eventos';
import { DlgFormEventoInclusao, ServicoDlgFormEvento, DlgFormEventoAlteracao } from './evento/dlg-form-evento';
import { TelaGestaoEvento } from './evento/tela-gestao-evento';

import { ConfiguracaoSistemaService, Configuracao } from './configuracao-sistema-service';
import { TelaListagemSalas } from './sala-estudo/tela-listagem-salas';
import { TelaRoteamentoEvento, ServicoEventoSelecionado } from './evento/tela-roteamento-evento';
import { WebServiceSalas } from './webservices/webservice-salas';
import { TelaListagemInscricoes } from './inscricao/tela-lista-inscricoes';
import { WebServiceInscricoes } from './webservices/webservice-inscricoes';
import { TelaInscricao, TelaInscricaoInclusao } from './inscricao/tela-inscricao';
import { ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador, ComponenteOficinaParticipanteSemEscolha } from './inscricao/atividades/comp-oficinas';
import { ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador } from './inscricao/atividades/comp-salas';
import { ComponenteDepartamentos } from './inscricao/atividades/comp-departamentos';
import { ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario, DialogosInscricaoSarau } from './inscricao/atividades/comp-sarau';
import { ComponentePagamento } from './inscricao/pagamento/comp-pagamento';
import { Observable } from 'rxjs';
import { DialogosSala, DlgFormSala } from './sala-estudo/dlg-form-sala';
import { WebServiceDivisaoSalas } from './webservices/webservice-divisao-salas';
import { TelaDivisaoSala } from './divisao-salas-estudo/tela-divisao-sala';
import { WebServiceRelatorios } from './webservices/webservice-relatorios';
import { WebServiceOficinas } from './webservices/webservice-oficinas';
import { WebServiceQuartos } from './webservices/webservice-quartos';
import { WebServiceDivisaoOficinas } from './webservices/webservice-divisao-oficinas';
import { WebServiceDivisaoQuartos } from './webservices/webservice-divisao-quartos';
import { TelaListagemOficinas } from './oficinas/tela-listagem-oficinas';
import { DlgFormOficina, DialogosOficina } from './oficinas/dlg-form-oficina';
import { TelaListagemQuartos } from './quartos/tela-listagem-quartos';
import { DlgFormQuarto, DialogosQuarto } from './quartos/dlg-form-quarto';
import { TelaDivisaoOficina } from './divisao-oficinas/tela-divisao-oficina';
import { TelaDivisaoQuarto } from './divisao-quartos/tela-divisao-quarto';
import { WebServiceEstatisticas } from './webservices/webservice-estatisticas';
import { TelaEstatisticas } from './estatisticas/tela-estatisticas';
import { TelaContratosInscricao } from './contratos-inscricao/tela-contratos-inscricao';
import { WebServiceContratosInscricao } from './webservices/webservice-contratos-inscricao';
import { TelaInscricaoInfantil, TelaInscricaoInfantilInclusao } from './inscricao/tela-inscricao-infantil';
import { CompFormInscricao } from './inscricao/comp-form-inscricao';
import { CompFormInscricaoInfantil } from './inscricao/comp-form-inscricao-infantil';
import { DlgSelecaoInscricaoAdulto, DialogosInscricao } from './inscricao/dlg-selecao-inscricao-adulto';
import { TelaListagemDepartamentos } from './departamentos/tela-listagem-departamentos';
import { DlgFormDepartamento, DialogosDepartamentos } from './departamentos/dlg-form-departamento';
import { WebServiceDepartamentos } from './webservices/webservice-departamentos';
import { TelaListagemSarais } from './sarais/tela-listagem-sarais';
import { DlgFormSarau, DialogosSarau } from './sarais/dlg-form-sarau';
import { WebServiceSarais } from './webservices/webservice-sarais';
import { TelaMensagensEmailInscricao } from './mensagens-email-inscricao/tela-mensagens-email-inscricao';
import { WebServiceMensagensInscricao } from './webservices/webservice-mensagens-email-inscricao';
import { TelaConfiguracaoEmail } from './configuracao-email/tela-configuracao-email';
import { WebServiceConfiguracaoEmail } from './webservices/webservice-configuracao-email';
import { WebServiceEtiquetas } from './webservices/webservice-etiquetas';
import { TelaEtiquetaCaderno } from './etiquetas/tela-etiqueta-caderno';
import { TelaEtiquetaCracha } from './etiquetas/tela-etiqueta-cracha';
import { WebServiceUsuariosAdm } from './webservices/webservice-usuarios-adm';
import { WebServiceUsuariosComum } from './webservices/webservice-usuarios-comum';
import { DialogoSenhaComum, DlgFormSenhaComum } from './usuarios/senha-comum/dlg-form-senha-comum';
import { DialogoSenhaAdmin, DlgAlteracaoSenhaAdmin, DlgFormSenhaAdmin } from './usuarios/senha-admin/dlg-form-senha-admin';
import { DialogoAlteracaoUsuarioComum, DlgFormAlteracaoUsuarioComum } from './usuarios/alteracao-comum/dlg-form-alteracao-usuario-comum';
import { TelaListagemUsuarios } from './usuarios/listagem/tela-listagem-usuarios';
import { TelaManutencaoUsuario } from './usuarios/cadastro/tela-manutencao-usuario';
import { FormUsuario } from './usuarios/cadastro/form-usuario';
import { TelaRoteamentoUsuario } from './usuarios/roteamento/tela-roteamento-usuario';

declare function require(url: string): any;

registerLocaleData(localePt);

//let ptMessages = require("devextreme/localization/messages/pt.json");
//loadMessages(ptMessages);
locale('pt');

@Injectable()
export class AppLoadService {

  constructor(private http: HttpClient) { }

  initializeApp(): Promise<any> {

    let controle = new Observable((observador) => {
      this.http.get<Configuracao>('assets/configuracao.json')
        .subscribe(
          (cnf) => {
            ConfiguracaoSistemaService.configuracao = cnf;
            observador.complete();
          },
          (erro) => observador.error(erro)
        );
    })
      .toPromise();

    return controle;
  }
}

export function init_app(appLoadService: AppLoadService) {
  return () => appLoadService.initializeApp();
}

@NgModule({
  declarations: [
    CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaPrincipal,
    TelaLogin,
    TelaListaEventos, DlgFormEventoInclusao, DlgFormEventoAlteracao, TelaGestaoEvento, TelaRoteamentoEvento,
    TelaListagemSalas, DlgFormSala,
    TelaListagemInscricoes,
    TelaInscricao, TelaInscricaoInclusao, TelaInscricaoInfantil, TelaInscricaoInfantilInclusao, CompFormInscricao, CompFormInscricaoInfantil,
    ComponenteOficinas, ComponenteOficinaParticipante, ComponenteOficinaCoordenador, ComponenteOficinaParticipanteSemEscolha,
    ComponenteSalas, ComponenteSalasParticipanteComEscolha, ComponenteSalasParticipanteSemEscolha, ComponenteSalaCoordenador,
    ComponenteDepartamentos, ComponenteSarau, DlgSarauCodigo, DlgSarauFormulario, DlgSelecaoInscricaoAdulto,
    ComponentePagamento,
    TelaDivisaoSala, TelaDivisaoOficina, TelaDivisaoQuarto,
    TelaListagemOficinas, DlgFormOficina,
    TelaListagemQuartos, DlgFormQuarto,
    TelaEstatisticas, TelaContratosInscricao,
    TelaListagemDepartamentos, DlgFormDepartamento,
    TelaListagemSarais, DlgFormSarau,
    TelaMensagensEmailInscricao, TelaConfiguracaoEmail,
    TelaEtiquetaCaderno, TelaEtiquetaCracha,
    DlgFormSenhaComum, DlgAlteracaoSenhaAdmin, DlgFormSenhaAdmin, DlgFormAlteracaoUsuarioComum,
    TelaRoteamentoUsuario, TelaListagemUsuarios, TelaManutencaoUsuario, FormUsuario
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatIconModule,
    MatMenuModule,
    MatButtonModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonToggleModule,
    MatSidenavModule,
    MatTabsModule,
    MatCardModule,
    MatDividerModule,
    FlexLayoutModule,
    DxDataGridModule,
    DxTextBoxModule,
    DxNumberBoxModule,
    DxDateBoxModule,
    DxDropDownBoxModule,
    DxListModule,
    DxRadioGroupModule,
    DxCheckBoxModule,
    DxPopoverModule,
    DxTextAreaModule,
    DxValidatorModule,
    DxValidationGroupModule,
    DxSelectBoxModule,
    DxFileUploaderModule,
    DxGalleryModule,    
    RichTextEditorAllModule,
    RouterModule.forRoot([
      { path: '', component: TelaListaEventos, canActivate: [PermissaoAcessoRota] },
      { path: 'login', component: TelaLogin },
      {
        path: 'usuarios', component: TelaRoteamentoUsuario, canActivate: [PermissaoAcessoRota],
        children: [
          { path: '', component: TelaListagemUsuarios },
          { path: 'incluir', component: TelaManutencaoUsuario },
          { path: 'editar/:login', component: TelaManutencaoUsuario },
        ]
      },
      {
        path: 'evento/:id', component: TelaRoteamentoEvento, canActivate: [PermissaoAcessoRota],
        children: [
          { path: '', component: TelaGestaoEvento, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes', component: TelaListagemInscricoes, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes/:idInscricao/editar', component: TelaInscricao, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes/incluir', component: TelaInscricaoInclusao, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes/:idInscricao/editar-infantil', component: TelaInscricaoInfantil, canActivate: [PermissaoAcessoRota] },
          { path: 'inscricoes/incluir-infantil', component: TelaInscricaoInfantilInclusao, canActivate: [PermissaoAcessoRota] },
          { path: 'salas', component: TelaListagemSalas, canActivate: [PermissaoAcessoRota] },
          { path: 'divisao-salas', component: TelaDivisaoSala, canActivate: [PermissaoAcessoRota] },
          { path: 'oficinas', component: TelaListagemOficinas, canActivate: [PermissaoAcessoRota] },
          { path: 'divisao-oficinas', component: TelaDivisaoOficina, canActivate: [PermissaoAcessoRota] },
          { path: 'quartos', component: TelaListagemQuartos, canActivate: [PermissaoAcessoRota] },
          { path: 'divisao-quartos', component: TelaDivisaoQuarto, canActivate: [PermissaoAcessoRota] },
          { path: 'estatisticas', component: TelaEstatisticas, canActivate: [PermissaoAcessoRota] },
          { path: 'contratos', component: TelaContratosInscricao, canActivate: [PermissaoAcessoRota] },
          { path: 'departamentos', component: TelaListagemDepartamentos, canActivate: [PermissaoAcessoRota] },
          { path: 'sarau', component: TelaListagemSarais, canActivate: [PermissaoAcessoRota] },
          { path: 'mensagens-inscricao', component: TelaMensagensEmailInscricao, canActivate: [PermissaoAcessoRota] },
          { path: 'configuracao-email', component: TelaConfiguracaoEmail, canActivate: [PermissaoAcessoRota] },
          { path: 'etiquetas-caderno', component: TelaEtiquetaCaderno, canActivate: [PermissaoAcessoRota] },
          { path: 'etiquetas-cracha', component: TelaEtiquetaCracha, canActivate: [PermissaoAcessoRota] },
        ]
      },
      { path: '** ', redirectTo: '' }
    ], { enableTracing: false })
  ],
  /*entryComponents: [CaixaMensagemDlg, DlgEmProcessamento, MenuUsuario, LayoutGeral,
    TelaListaEventos, DlgFormEventoInclusao, DlgFormEventoAlteracao, TelaRoteamentoEvento,
    TelaListagemSalas, DlgFormSala, DlgFormOficina, DlgFormQuarto,
    DlgSarauCodigo, DlgSarauFormulario, DlgSelecaoInscricaoAdulto, DlgFormDepartamento, DlgFormSarau,
    DlgFormSenhaComum, DlgAlteracaoSenhaAdmin, DlgFormAlteracaoUsuarioComum, DlgFormSenhaAdmin,
    TelaListagemUsuarios, TelaManutencaoUsuario, FormUsuario, TelaRoteamentoUsuario,
  ],*/
  providers: [
    AppLoadService,
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    { provide: APP_INITIALIZER, useFactory: init_app, deps: [AppLoadService], multi: true },
    Alertas, PermissaoAcessoRota, GestaoAutenticacao, ServicoDlgFormEvento,
    WebServiceAutenticacao, WebServiceEventos, WebServiceSalas, WebServiceInscricoes, ServicoEventoSelecionado,
    WebServiceDivisaoSalas, WebServiceRelatorios, WebServiceOficinas, WebServiceQuartos, WebServiceDivisaoOficinas, WebServiceDivisaoQuartos,
    DialogosInscricaoSarau, DialogosSala, DialogosOficina, DialogosQuarto, DialogosInscricao, DialogosDepartamentos,
    WebServiceEstatisticas, WebServiceContratosInscricao, WebServiceDepartamentos, WebServiceSarais, DialogosSarau,
    WebServiceMensagensInscricao, WebServiceConfiguracaoEmail, WebServiceEtiquetas, WebServiceUsuariosAdm, WebServiceUsuariosComum,
    DialogoSenhaComum, DialogoSenhaAdmin, DialogoAlteracaoUsuarioComum],
  bootstrap: [TelaPrincipal]
})
export class AppModule {
}
