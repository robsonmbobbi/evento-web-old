import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DTOOficina } from './objetos';
import { WebServiceOficinas } from '../webservices/webservice-oficinas';

@Component({
  selector: 'dlg-form-oficina',
  styleUrls: ['./dlg-form-oficina.scss'],
  templateUrl: './dlg-form-oficina.html'
})
export class DlgFormOficina implements OnInit {    

  oficina!: DTOOficina;
  idOficina: number | null;
  idEvento: number;
  titulo!: string;

  constructor(public dialogRef: MatDialogRef<DlgFormOficina>, private alertas: Alertas,
    private wsOficinas: WebServiceOficinas, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.idOficina = null;
    this.idEvento = 0;

    if (data != null) {
      this.idOficina = data.idOficina || null;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.titulo = "Inclusão de oficina";

    if (this.idOficina == null) {
      this.oficina = new DTOOficina();
      this.oficina.DeveSerParNumeroTotalParticipantes = false;
    }      
    else {
      this.titulo = "Atualização de oficina";
      let dlg = this.alertas.alertarProcessamento("Buscando dados da oficina...");
      this.wsOficinas.obterId(this.idEvento, this.idOficina)
        .subscribe(
          oficina => {
            this.oficina = oficina;
            dlg.close();
          },
          erro => {
            dlg.close();
            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  clicarSalvar(): void {
    if (this.oficina.Nome == null || this.oficina.Nome.trim().length == 0)
      this.alertas.alertarAtencao('Informe o nome da oficina.', "");
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando oficina, aguarde...");

      let servico: any;

      if (this.idOficina == null)
        servico = this.wsOficinas.incluir(this.idEvento, this.oficina);
      else
        servico = this.wsOficinas.atualizar(this.idEvento, this.idOficina, this.oficina);

      servico
        .subscribe(
          (retorno: any) => {            
            dlg.close();

            if (this.idOficina == null)
              this.oficina.Id = retorno.Id;

            this.dialogRef.close(this.oficina || null);
          },
          (erro: any) => {
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
export class DialogosOficina {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number): Observable<DTOOficina> {
    const dlg = this.srvDialog.open(DlgFormOficina, { data: { idEvento: idEvento, idOficina: null } });
    return dlg.afterClosed();
  }

  apresentarDlgFormDialogoEdicao(idEvento: number, idOficina: number): Observable<DTOOficina> {
    const dlg = this.srvDialog.open(DlgFormOficina, { data: { idEvento: idEvento, idOficina: idOficina } });
    return dlg.afterClosed();
  }
}
