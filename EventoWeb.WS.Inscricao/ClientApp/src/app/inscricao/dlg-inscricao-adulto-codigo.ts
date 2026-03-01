import { Component, ViewChild, Injectable, Inject } from '@angular/core';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';
import { DialogoContrato } from '../contrato/dlg-contrato';

@Component({
  selector: 'dlg-inscricao-adulto-codigo',
  templateUrl: './dlg-inscricao-adulto-codigo.html'
})
export class DlgInscricaoAdultoCodigo {

  codigo: string;
  private idEvento: number;

  @ViewChild("grupoValidacao")
  grupoValidacao: DxValidationGroupComponent;

  constructor(private dialogRef: MatDialogRef<DlgInscricaoAdultoCodigo>, private dlgContrato: DialogoContrato, @Inject(MAT_DIALOG_DATA) public data: any) {
    if (data != null) {
      this.idEvento = data.idEvento;
    }
  }

  clicarOK(): void {
    let validacao = this.grupoValidacao.instance.validate();
    if (validacao.isValid)
      this.dialogRef.close(this.codigo);
  }

  clicarCancelar(): void {
    this.dialogRef.close();
  }

  clicarRegulamento(): void {
    this.dlgContrato.apresentarDlgFormDialogoInclusao(this.idEvento);
  }
}

@Injectable()
export class DialogosInscricao {
  constructor(private srvDialog: MatDialog) { }

  apresentarDlgCodigo(idEvento: number): Observable<string> {
    const dlg = this.srvDialog.open(DlgInscricaoAdultoCodigo, { data: { idEvento: idEvento }, width: '70vw' });
    return dlg.afterClosed();
  }
}
