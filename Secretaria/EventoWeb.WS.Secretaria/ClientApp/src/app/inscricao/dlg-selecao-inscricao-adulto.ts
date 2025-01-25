import { Component, Injectable, Inject } from '@angular/core';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { WebServiceInscricoes } from '../webservices/webservice-inscricoes';
import { DTOBasicoInscricao, EnumSituacaoInscricao } from './objetos';
import { Alertas } from '../componentes/alertas-dlg/alertas';

@Component({
  selector: 'dlg-selecao-inscricao-adulto',
  templateUrl: './dlg-selecao-inscricao-adulto.html'
})
export class DlgSelecaoInscricaoAdulto {
  private m_IdEvento: number;
  private m_FiltroEscolhido: string = null;
  public filtros: string[] = ["Incompleta", "Pendente", "Aceita", "Rejeitada"];
  public inscricoes: DTOBasicoInscricao[] = [];

  constructor(private dialogRef: MatDialogRef<DlgSelecaoInscricaoAdulto>, @Inject(MAT_DIALOG_DATA) public data: any,
    private wsInscricoes: WebServiceInscricoes, private alertas: Alertas,) {
    if (data != null) {
      this.m_IdEvento = data.idEvento;
    }
  }

  set filtroEscolhido(valor: string) {
    if (valor != this.m_FiltroEscolhido) {
      this.m_FiltroEscolhido = valor;

      let dlg = this.alertas.alertarProcessamento("Pesquisando inscrições....");
      this.wsInscricoes.obterTodas(this.m_IdEvento, this.filtros.findIndex(x => x == this.m_FiltroEscolhido))
        .subscribe(
          inscricoesRetornadas => {
            dlg.close();
            this.inscricoes = inscricoesRetornadas.map(x => {
              x.DataNascimento = new Date(x.DataNascimento);
              return x;
            });
          },
          (erro) => {
            dlg.close();

            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  get filtroEscolhido(): string {
    return this.m_FiltroEscolhido;
  }

  obterTextoSituacao(situacao: EnumSituacaoInscricao): string {
    switch (situacao) {
      case EnumSituacaoInscricao.Aceita:
        return "Aceita";
      case EnumSituacaoInscricao.Pendente:
        return "Pendente";
      case EnumSituacaoInscricao.Rejeitada:
        return "Rejeitada";
      default:
        return "";
    }
  }

  clicarSelecionado(inscricao: DTOBasicoInscricao): void {
    this.dialogRef.close(inscricao);
  }
}

@Injectable()
export class DialogosInscricao {
  constructor(private srvDialog: MatDialog) { }

  apresentarDlgPesquisa(idEvento: number): Observable<DTOBasicoInscricao> {
    const dlg = this.srvDialog.open(DlgSelecaoInscricaoAdulto, { data: { idEvento: idEvento }, width: '90vw', height:'90vh' });
    return dlg.afterClosed();
  }
}
