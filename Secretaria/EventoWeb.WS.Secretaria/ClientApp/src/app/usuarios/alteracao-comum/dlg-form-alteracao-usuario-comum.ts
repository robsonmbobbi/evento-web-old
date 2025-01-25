import { Component, Injectable, ViewChild, OnInit } from "@angular/core";
import { MatDialogRef, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DxValidationGroupComponent } from "devextreme-angular/ui/validation-group";
import { DTOAlteracaoSenhaWS, DTOUsuario } from "../objetos";
import { WebServiceUsuariosComum } from "../../webservices/webservice-usuarios-comum";

@Component({
  selector: 'dlg-form-alteracao-usuario-comum',
  styleUrls: ['./dlg-form-alteracao-usuario-comum.scss'],
  templateUrl: './dlg-form-alteracao-usuario-comum.html'
})
export class DlgFormAlteracaoUsuarioComum implements OnInit {    

  usuario!: DTOUsuario;

  @ViewChild("grupoValidacao", { static: true })
  grupoValidacao?: DxValidationGroupComponent;

  constructor(
    public dialogRef: MatDialogRef<DlgFormAlteracaoUsuarioComum>,
    private alertas: Alertas,
    private wsUsuarios: WebServiceUsuariosComum) {
  }

  ngOnInit(): void {
    this.usuario = {
      EhAdministrador: false,
      Login: "",
      Nome: ""
    };

    let dlg = this.alertas.alertarProcessamento("Buscando dados do usuário, aguarde...");

    this.wsUsuarios.obter()
      .subscribe(
        usuario => {
          if (usuario != null)
            this.usuario = usuario;

          dlg.close();
        },
        erro => {
          dlg.close();
          this.alertas.alertarErro(erro);
        }
    );
  }

  clicarSalvar(): void {
    if (this.grupoValidacao?.instance.validate().isValid) {
      let dlg = this.alertas.alertarProcessamento("Alterando usuário, aguarde...");

      this.wsUsuarios.atualizar(this.usuario!)
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
    this.dialogRef.close();
  }  
}

@Injectable()
export class DialogoAlteracaoUsuarioComum {

  constructor(private srvDialog: MatDialog) { }

  apresentar(): Observable<void> {
    const dlg = this.srvDialog.open(DlgFormAlteracaoUsuarioComum, { width: '380px' });
    return dlg.afterClosed();
  }
}
