import { Component, Injectable, ViewChild } from "@angular/core";
import { MatDialogRef, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { WebServiceUsuariosComum } from "../../webservices/webservice-usuarios-comum";
import { DxValidationGroupComponent } from "devextreme-angular/ui/validation-group";
import { DTOAlteracaoSenhaComumWS } from "../objetos";

@Component({
  selector: 'dlg-form-senha-comum',
  styleUrls: ['./dlg-form-senha-comum.scss'],
  templateUrl: './dlg-form-senha-comum.html'
})
export class DlgFormSenhaComum {    

  senhaAtual: string = "";
  novaSenha: string = "";
  repeticaoNovaSenha: string = "";

  @ViewChild("grupoValidacao", { static: true })
  grupoValidacao!: DxValidationGroupComponent;

  constructor(
    public dialogRef: MatDialogRef<DlgFormSenhaComum>,
    private alertas: Alertas,
    private wsUsuarios: WebServiceUsuariosComum) {
  }

  comparadorSenha = () => {
    return this.novaSenha;
  };

  clicarSalvar(): void {
    if (this.grupoValidacao.instance.validate().isValid) {
      let dlg = this.alertas.alertarProcessamento("Alterando senha, aguarde...");

      let dto = new DTOAlteracaoSenhaComumWS();
      dto.NovaSenha = this.novaSenha;
      dto.NovaSenhaRepetida = this.repeticaoNovaSenha;
      dto.SenhaAtual = this.senhaAtual;

      this.wsUsuarios.atualizarSenha(dto)
        .subscribe(
          () => {            
            dlg.close();
            this.dialogRef.close();
          },
          erro => {
            dlg.close();
            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  clicarCancelar() {
    this.dialogRef.close(null);
  }  
}

@Injectable()
export class DialogoSenhaComum {

  constructor(private srvDialog: MatDialog) { }

  apresentar(): Observable<void> {
    const dlg = this.srvDialog.open(DlgFormSenhaComum, { width: '380px' });
    return dlg.afterClosed();
  }
}
