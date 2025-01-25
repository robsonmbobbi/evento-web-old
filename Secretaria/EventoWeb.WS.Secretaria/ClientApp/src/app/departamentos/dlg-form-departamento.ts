import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DTODepartamento } from './objetos';
import { WebServiceDepartamentos } from '../webservices/webservice-departamentos';

@Component({
  selector: 'dlg-form-departamento',
  styleUrls: ['./dlg-form-departamento.scss'],
  templateUrl: './dlg-form-departamento.html'
})
export class DlgFormDepartamento implements OnInit {    

  departamento!: DTODepartamento;
  idDepartamento: number | null;
  idEvento!: number;
  titulo!: string;

  constructor(public dialogRef: MatDialogRef<DlgFormDepartamento>, private alertas: Alertas,
    private wsDepartamentos: WebServiceDepartamentos, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.idDepartamento = null;
    this.idEvento = 0;

    if (data != null) {
      this.idDepartamento = data.idDepartamento || null;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.titulo = "Inclusão de Departamento";

    if (this.idDepartamento == null) {
      this.departamento = new DTODepartamento();
    }      
    else {
      this.titulo = "Atualização de Departamento";
      let dlg = this.alertas.alertarProcessamento("Buscando dados do departamento...");
      this.wsDepartamentos.obterId(this.idEvento, this.idDepartamento)
        .subscribe(
          departamento => {
            this.departamento = departamento;            
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
    if (this.departamento.Nome == null || this.departamento.Nome.trim().length == 0)
      this.alertas.alertarAtencao('Informe o nome do departamento.', "");
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando Departamento, aguarde...");

      let servico: any;

      if (this.idDepartamento == null)
        servico = this.wsDepartamentos.incluir(this.idEvento, this.departamento);
      else
        servico = this.wsDepartamentos.atualizar(this.idEvento, this.idDepartamento, this.departamento);

      servico
        .subscribe(
            (retorno: any) => {            
            dlg.close();

            if (this.idDepartamento == null)
              this.departamento.Id = retorno.Id;

            this.dialogRef.close(this.departamento || null);
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
export class DialogosDepartamentos {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number): Observable<DTODepartamento> {
    const dlg = this.srvDialog.open(DlgFormDepartamento, { data: { idEvento: idEvento, idDepartamento: null }, width: '70vw' });
    return dlg.afterClosed();
  }

  apresentarDlgFormDialogoEdicao(idEvento: number, idDepartamento: number): Observable<DTODepartamento> {
    const dlg = this.srvDialog.open(DlgFormDepartamento, { data: { idEvento: idEvento, idDepartamento: idDepartamento }, width: '70vw' });
    return dlg.afterClosed();
  }
}
