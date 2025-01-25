import { Injectable, Component, ViewContainerRef, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CaixaMensagem, CaixaMensagemBotoes, CaixaMensagemEstilos, CaixaMensagemResposta } from './caixa-mensagem-dlg';
import { Observable } from 'rxjs';

@Injectable()
export class Alertas {

    constructor(public dialogo: MatDialog) {
  }

  alertarErro(mensagem: any): void {        
    if (typeof mensagem == "string")
      this.alertar('Erro', mensagem, "", CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
    else
      this.alertar('Erro', mensagem.MensagemErro, "", CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
  }

  alertar(titulo: string, mensagem: string, informacao: string, botoes: CaixaMensagemBotoes, estilo: CaixaMensagemEstilos): Observable<any> {
    return CaixaMensagem.apresentar(this.dialogo, mensagem, titulo, informacao,
      botoes, false, estilo, "350px");
  }

  alertarAtencao(mensagem: string, detalhes: string): void {
    this.alertar('Atenção', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
  }

  alertarInformacao(mensagem: string, detalhes: string): void {
    this.alertar('Informação', mensagem, detalhes, CaixaMensagemBotoes.Ok, CaixaMensagemEstilos.Completo);
  }

  alertarConfirmacao(mensagem: string, detalhes: string): Observable<CaixaMensagemResposta> {
    return this.alertar('Confirmação', mensagem, detalhes, CaixaMensagemBotoes.SimNao, CaixaMensagemEstilos.Completo);
  }

  alertarProcessamento(mensagem: string): MatDialogRef<DlgEmProcessamento> {

    return this.dialogo.open<DlgEmProcessamento>(DlgEmProcessamento, {
      width: '300px',
      disableClose: true,
      data: {
        texto: mensagem
      }
    });
  }
}

@Component({
  selector: 'dlg-processando',
  templateUrl: './dlg-processando.html',
  styles: [':host { margin: 10px }' ]
})
export class DlgEmProcessamento {

    constructor(public dialogRef: MatDialogRef<DlgEmProcessamento>, @Inject(MAT_DIALOG_DATA) public data: any) { }
}
