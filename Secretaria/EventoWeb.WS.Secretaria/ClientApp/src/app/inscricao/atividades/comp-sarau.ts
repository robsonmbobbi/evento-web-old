import { Component, EventEmitter, Input, Output, ViewChild, Inject, Injectable } from '@angular/core';
import { DTOInscricaoSimplificada } from '../objetos';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';
import { Alertas } from '../../componentes/alertas-dlg/alertas';
import { CaixaMensagemResposta } from '../../componentes/alertas-dlg/caixa-mensagem-dlg';
import { DTOSarau } from '../../sarais/objetos';


@Component({
  selector: 'comp-sarau',
  templateUrl: './comp-sarau.html'
})
export class ComponenteSarau {

  @Input()
  valor: DTOSarau[];
  @Output()
  valorChange: EventEmitter<DTOSarau[]> = new EventEmitter<DTOSarau[]>();

  @Input()
  inscrito: DTOInscricaoSimplificada;

  @Input()
  desabilitar: boolean;

  constructor(private mensageria: Alertas, private DlgsSarau: DialogosInscricaoSarau) { }

  clicarCriar(): void {
    this.DlgsSarau.apresentarDlgForm({ desabilitar: this.desabilitar, sarau: null })
      .subscribe(
        (sarauCriado) => {
          if (sarauCriado != null) {
            sarauCriado.Participantes = [];
            sarauCriado.Participantes.push(this.inscrito);

            this.valor.push(sarauCriado);
            this.valorChange.emit(this.valor);
          }
        }
      );
  }

  editar(sarau: DTOSarau): void {
    this.DlgsSarau.apresentarDlgForm({ desabilitar: this.desabilitar, sarau: sarau })
      .subscribe(
        (sarauAlterado) => {
          if (sarauAlterado != null) {
            let indice = this.valor.findIndex(x => x.Id == sarauAlterado.Id);
            if (indice != -1) {
              this.valor[indice] = sarauAlterado;
              this.valorChange.emit(this.valor);
            }
          }
        }
      );
  }

  excluir(sarau: DTOSarau): void {

    this.mensageria.alertarConfirmacao("Deseja excluir este sarau?", "Ao removê-lo, este sarau também será excluido dos outros participantes")
      .subscribe(
        (botaoPressionado) => {
          if (botaoPressionado == CaixaMensagemResposta.Sim) {
            this.valor = this.valor.filter(x => x.Id != sarau.Id);
            this.valorChange.emit(this.valor);
          }
        });
  }

  gerarTextoParticipantes(sarau: DTOSarau): string {

    let pessoas: string = "";
    for (let inscricao of sarau.Participantes) {

      if (pessoas != "")
        pessoas = pessoas + ", ";

      if (inscricao.Id == this.inscrito.Id)
        pessoas = pessoas + "Você";
      else
        pessoas = pessoas + inscricao.Nome;
    }

    return pessoas;
  }
}

@Component({
  selector: 'dlg-sarau-codigo',
  templateUrl: './dlg-sarau-codigo.html'
})
export class DlgSarauCodigo {

  codigo: string;

  @ViewChild("grupoValidacao")
  grupoValidacao: DxValidationGroupComponent;

  constructor(private dialogRef: MatDialogRef<DlgSarauCodigo>) { }

  clicarOK(): void {
    let validacao = this.grupoValidacao.instance.validate();
    if (validacao.isValid)
      this.dialogRef.close(this.codigo);
  }

  clicarCancelar(): void {
    this.dialogRef.close();
  }
}

@Component({
  selector: 'dlg-sarau-form',
  templateUrl: './dlg-sarau-form.html'
})
export class DlgSarauFormulario {

  @ViewChild("grupoValidacao")
  grupoValidacao: DxValidationGroupComponent;

  sarau: DTOSarau;
  tituloDlg: string;
  desabilitar: boolean;

  constructor(private dialogRef: MatDialogRef<DlgSarauFormulario>,
    @Inject(MAT_DIALOG_DATA) public data: TransfDlgFormSarau) {

    this.tituloDlg = "Nova Apresentação";
    this.desabilitar = data.desabilitar;

    if (data.sarau == null)
      this.sarau = new DTOSarau();
    else {
      this.sarau = data.sarau;
      this.tituloDlg = "Alteração de Apresentação";
    }
  }

  clicarOK(): void {
    let validacao = this.grupoValidacao.instance.validate();
    if (validacao.isValid)
      this.dialogRef.close(this.sarau);
  }

  clicarCancelar(): void {
    this.dialogRef.close();
  }
}

class TransfDlgFormSarau {
  sarau: DTOSarau;
  desabilitar: boolean;
}

@Injectable()
export class DialogosInscricaoSarau {
  constructor(private srvDialog: MatDialog) { }

  apresentarDlgCodigo(): Observable<string> {
    const dlg = this.srvDialog.open(DlgSarauCodigo);
    return dlg.afterClosed();
  }

  apresentarDlgForm(trans: TransfDlgFormSarau): Observable<DTOSarau> {
    const dlg = this.srvDialog.open(DlgSarauFormulario, { data: trans });
    return dlg.afterClosed();
  }
}
