import { Component, Inject, Injectable, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Observable } from 'rxjs';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';

@Component({
  selector: 'dlg-validacao-email',
  styleUrls: ['./dlg-validacao-email.scss'],
  templateUrl: './dlg-validacao-email.html'
})
export class DlgValidacaoEmail {

  celular: string;
  nome: string;
  codigo: string;
  private identificacao: string;

  @ViewChild("grupoValidacao")
  grupoValidacao: DxValidationGroupComponent;

  constructor(public dialogRef: MatDialogRef<DlgValidacaoEmail>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private wsInscricao: WsInscricoes, private coordenacao: CoordenacaoCentral) {

    if (data != null) {
      this.identificacao = data.identificacao;
      this.celular = data.celular;
      this.nome = data.nome;
    }
  }

  clicarValidar(): void {
    if (this.grupoValidacao.instance.validate().isValid) {
      let dlg = this.coordenacao.Alertas.alertarProcessamento("Validando código...");

      this.wsInscricao.validarCodigoEmail(this.identificacao, this.codigo)
        .subscribe(
          (codigoEhvalido) => {
            dlg.close();
            if (codigoEhvalido)
              this.dialogRef.close(true);
            else
              this.coordenacao.Alertas.alertarAtencao("Código informado não é valido!!", "");
          },
          (erro) => {
            dlg.close();
            this.coordenacao.ProcessamentoErro.processar(erro);
          }
        );
    }
  }

  clicarCancelar(): void {
    this.dialogRef.close(false);
  }
}

@Injectable()
export class DialogoValidacaoEmail {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlg(celular: string, nome: string, identificacao: string): Observable<boolean> {
    const dlg = this.srvDialog.open(DlgValidacaoEmail, { data: { celular: celular, nome: nome, identificacao: identificacao }, width: "95vw", disableClose: true });
    return dlg.afterClosed();
  }
}
