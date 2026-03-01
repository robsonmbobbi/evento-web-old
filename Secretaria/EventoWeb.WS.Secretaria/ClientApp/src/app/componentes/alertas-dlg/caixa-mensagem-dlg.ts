import { Component, Inject } from "@angular/core";
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from "@angular/material/dialog";

// Origem https://github.com/trashvin/messagebox-like-angular-alertbox

@Component({
  selector: "caixa-mensagem-dlg",
  templateUrl: "./caixa-mensagem-dlg.html",
  styleUrls: ["./caixa-mensagem-dlg.scss"]
})
export class CaixaMensagemDlg {
  style: number;
  title: string;
  message: string;
  information: string;
  button: number;

  constructor(
    public dialogRef: MatDialogRef<CaixaMensagemDlg>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.style = data.style || 0;
    this.title = data.title;
    this.message = data.message;
    this.information = data.information;
    this.button = data.button;
    this.dialogRef.disableClose = !data.allow_outside_click || false;

  }

  onOk() {
    this.dialogRef.close(CaixaMensagemResposta.OK);
  }

  onCancel() {
    this.dialogRef.close(CaixaMensagemResposta.Cancelar);
  }

  onYes() {
    this.dialogRef.close(CaixaMensagemResposta.Sim);
  }

  onNo() {
    this.dialogRef.close(CaixaMensagemResposta.Nao);
  }

  onAccept() {
    this.dialogRef.close(CaixaMensagemResposta.Aceito);
  }

  onReject() {
    this.dialogRef.close(CaixaMensagemResposta.Rejeitado);
  }
}

export class CaixaMensagem {
  static apresentar(dialog: MatDialog, message: string, title = "Alerta",
    information = "", button = 0, allow_outside_click = false,
    style = 0, width = "200px") {
    const dialogRef = dialog.open(CaixaMensagemDlg, {
      data: {
        title: title || "Alerta",
        message: message,
        information: information,
        button: button || 0,
        style: style || 0,
        allow_outside_click: allow_outside_click || false
      },
      width: width
    });
    return dialogRef.afterClosed();
  }
}

export enum CaixaMensagemBotoes {
  Ok = 0,
  OkCancelar = 1,
  SimNao = 2,
  AceitarRejeitar = 3
}

export enum CaixaMensagemEstilos {
  Simples = 0,
  Completo = 1
};

export enum CaixaMensagemResposta {
  OK,
  Cancelar,
  Sim,
  Nao,
  Aceito,
  Rejeitado,
}
