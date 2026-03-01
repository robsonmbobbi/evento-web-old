import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { WebServiceSalas } from "../webservices/webservice-salas";
import { DTOSalaEstudo } from "./objetos";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';

@Component({
  selector: 'dlg-form-sala',
  styleUrls: ['./dlg-form-sala.scss'],
  templateUrl: './dlg-form-sala.html'
})
export class DlgFormSala implements OnInit {    

  sala!: DTOSalaEstudo;
  idSala: number | null;
  idEvento: number;
  usaFaixaEtaria!: boolean;
  podeUsarFaixaEtaria: boolean;
  titulo!: string;

  constructor(public dialogRef: MatDialogRef<DlgFormSala>, private alertas: Alertas,
    private wsSalas: WebServiceSalas, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.podeUsarFaixaEtaria = false;
    this.idSala = null;
    this.idEvento = 0;

    if (data != null) {
      this.idSala = data.idSala || null;
      this.podeUsarFaixaEtaria = data.podeUsarFaixaEtaria || false;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.usaFaixaEtaria = false;
    this.titulo = "Inclusão de Sala de Estudo";

    if (this.idSala == null) {
      this.sala = new DTOSalaEstudo();
      this.sala.DeveSerParNumeroTotalParticipantes = false;
    }      
    else {
      this.titulo = "Atualização de Sala de Estudo";
      let dlg = this.alertas.alertarProcessamento("Buscando dados da Sala...");
      this.wsSalas.obterId(this.idEvento, this.idSala)
        .subscribe(
          sala => {
            this.sala = sala;
            this.usaFaixaEtaria = this.sala.IdadeMaxima != null && this.sala.IdadeMinima != null;
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
    if (this.sala.Nome == null || this.sala.Nome.trim().length == 0)
      this.alertas.alertarAtencao('Informe o nome da sala.', "");
    else if (this.podeUsarFaixaEtaria && this.usaFaixaEtaria && (this.sala.IdadeMinima == null || this.sala.IdadeMaxima == null))
      this.alertas.alertarAtencao('Você precisa informar a faixa etária inicial e final', '');
    else if (this.podeUsarFaixaEtaria && this.usaFaixaEtaria && this.sala.IdadeMinima > this.sala.IdadeMaxima)
      this.alertas.alertarAtencao('A idade mínima deve ser menor ou igual a máxima', '');
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando Sala, aguarde...");

      let servico: any;

      if (this.idSala == null)
        servico = this.wsSalas.incluir(this.idEvento, this.sala);
      else
        servico = this.wsSalas.atualizar(this.idEvento, this.idSala, this.sala);

      servico
        .subscribe(
          (retorno: any) => {            
            dlg.close();

            if (this.idSala == null)
              this.sala.Id = retorno.Id;

            this.dialogRef.close(this.sala || null);
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
export class DialogosSala {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number, podeUsarFaixaEtaria: boolean): Observable<DTOSalaEstudo> {
    const dlg = this.srvDialog.open(DlgFormSala, { data: { idEvento: idEvento, idSala: null, podeUsarFaixaEtaria: podeUsarFaixaEtaria } });
    return dlg.afterClosed();
  }

  apresentarDlgFormDialogoEdicao(idEvento: number, idSala: number, podeUsarFaixaEtaria: boolean): Observable<DTOSalaEstudo> {
    const dlg = this.srvDialog.open(DlgFormSala, { data: { idEvento: idEvento, idSala: idSala, podeUsarFaixaEtaria: podeUsarFaixaEtaria } });
    return dlg.afterClosed();
  }
}
