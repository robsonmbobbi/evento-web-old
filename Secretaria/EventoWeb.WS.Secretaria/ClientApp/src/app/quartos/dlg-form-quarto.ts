import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DTOQuarto, EnumSexoQuarto } from './objetos';
import { WebServiceQuartos } from '../webservices/webservice-quartos';

@Component({
  selector: 'dlg-form-quarto',
  styleUrls: ['./dlg-form-quarto.scss'],
  templateUrl: './dlg-form-quarto.html'
})
export class DlgFormQuarto implements OnInit {    

  quarto!: DTOQuarto;
  idQuarto: number | null;
  idEvento: number;
  titulo!: string;

  sexos: string[] = ["Masculino", "Feminino", "Misto"];

  public set sexoEscolhido(valor: string) {
    if (valor == this.sexos[0])
      this.quarto.Sexo = EnumSexoQuarto.Masculino;
    else if (valor == this.sexos[1])
      this.quarto.Sexo = EnumSexoQuarto.Feminino;
    else if (valor == this.sexos[2])
      this.quarto.Sexo = EnumSexoQuarto.Misto;
  }

  public get sexoEscolhido(): string {
    return this.sexos[this.quarto.Sexo];
  }

  constructor(public dialogRef: MatDialogRef<DlgFormQuarto>, private alertas: Alertas,
    private wsQuartos: WebServiceQuartos, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.idQuarto = null;
    this.idEvento = 0;

    if (data != null) {
      this.idQuarto = data.idQuarto || null;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.titulo = "Inclusão de Quarto";

    if (this.idQuarto == null) {
      this.quarto = new DTOQuarto();
      this.quarto.EhFamilia = false;
      this.quarto.Sexo = EnumSexoQuarto.Masculino;
    }      
    else {
      this.titulo = "Atualização de quarto";
      let dlg = this.alertas.alertarProcessamento("Buscando dados do quarto...");
      this.wsQuartos.obterId(this.idEvento, this.idQuarto)
        .subscribe(
          quarto => {
            this.quarto = quarto;
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
    if (this.quarto.Nome == null || this.quarto.Nome.trim().length == 0)
      this.alertas.alertarAtencao('Informe o nome da quarto.', "");
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando quarto, aguarde...");

      let servico: any;

      if (this.idQuarto == null)
        servico = this.wsQuartos.incluir(this.idEvento, this.quarto);
      else
        servico = this.wsQuartos.atualizar(this.idEvento, this.idQuarto, this.quarto);

      servico
        .subscribe(
          (retorno: any) => {            
            dlg.close();

            if (this.idQuarto == null)
              this.quarto.Id = retorno.Id;

            this.dialogRef.close(this.quarto || null);
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
export class DialogosQuarto {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number): Observable<DTOQuarto> {
    const dlg = this.srvDialog.open(DlgFormQuarto, { data: { idEvento: idEvento, idQuarto: null } });
    return dlg.afterClosed();
  }

  apresentarDlgFormDialogoEdicao(idEvento: number, idQuarto: number): Observable<DTOQuarto> {
    const dlg = this.srvDialog.open(DlgFormQuarto, { data: { idEvento: idEvento, idQuarto: idQuarto } });
    return dlg.afterClosed();
  }
}
