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

import { RichTextEditorModule } from '@syncfusion/ej2-angular-richtexteditor';

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
import { TelaPrincipal } from './tela-principal';
import { Observable } from 'rxjs';

import { Configuracao, ConfiguracaoSistemaService } from './configuracao-sistema-service';
import { CaixaMensagemDlg } from './componentes/alertas-dlg/caixa-mensagem-dlg';
import { DlgEmProcessamento } from './componentes/alertas-dlg/alertas';
import { MenuUsuario } from './componentes/menu-usuario/menu-usuario';
import { LayoutGeral } from './componentes/layout-geral/layout-geral';

declare function require(url: string): any;

registerLocaleData(localePt);

let ptMessages = require("devextreme/localization/messages/pt.json");
loadMessages(ptMessages);
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
    TelaPrincipal
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
    RichTextEditorModule,
    RouterModule.forRoot([
      /*{ path: '', component: TelaListaEventos, canActivate: [PermissaoAcessoRota] },
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
      { path: '** ', redirectTo: '' }*/
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
  ],
  bootstrap: [TelaPrincipal]
})
export class AppModule {
}
