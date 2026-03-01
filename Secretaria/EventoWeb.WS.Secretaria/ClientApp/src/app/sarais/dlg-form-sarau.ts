import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { Observable } from 'rxjs';
import { DTOSarau } from './objetos';
import { WebServiceSarais } from '../webservices/webservice-sarais';
import { DTOInscricaoSimplificada } from '../inscricao/objetos';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { DialogosInscricao } from '../inscricao/dlg-selecao-inscricao-adulto';

@Component({
  selector: 'dlg-form-sarau',
  styleUrls: ['./dlg-form-sarau.scss'],
  templateUrl: './dlg-form-sarau.html'
})
export class DlgFormSarau implements OnInit {    

  sarau!: DTOSarau;
  idSarau: number | null;
  idEvento: number;
  titulo!: string;

  constructor(public dialogRef: MatDialogRef<DlgFormSarau>, private alertas: Alertas, private dlgsInscricao: DialogosInscricao,
    private wsSarais: WebServiceSarais, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.idSarau = null;
    this.idEvento = 0;

    if (data != null) {
      this.idSarau = data.idSarau || null;
      this.idEvento = data.idEvento || 0;
    }
  }

  ngOnInit(): void {
    this.titulo = "Inclusão de Sarau";

    if (this.idSarau == null) {
      this.sarau = new DTOSarau();
      this.sarau.Participantes = [];
    }      
    else {
      this.titulo = "Atualização de Sarau";
      let dlg = this.alertas.alertarProcessamento("Buscando dados do sarau...");
      this.wsSarais.obterId(this.idEvento, this.idSarau)
        .subscribe(
          sarau => {
            this.sarau = sarau;
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
    if (this.sarau.Tipo == null || this.sarau.Tipo.trim().length == 0)
      this.alertas.alertarAtencao('Informe o tipo de apresentação.', "");
    else if (this.sarau.DuracaoMin == null || this.sarau.DuracaoMin <= 0)
      this.alertas.alertarAtencao('Informe o tempo de duração da apresentação.', "");
    else if (this.sarau.Participantes == null || this.sarau.Participantes.length <= 0)
      this.alertas.alertarAtencao('Informe as pessoas que participarão.', "");
    else {
      let dlg = this.alertas.alertarProcessamento("Salvando sarau, aguarde...");

      let servico: any;

      if (this.idSarau == null)
        servico = this.wsSarais.incluir(this.idEvento, this.sarau);
      else
        servico = this.wsSarais.atualizar(this.idEvento, this.idSarau, this.sarau);

      servico
        .subscribe(
          (retorno: any) => {            
            dlg.close();

            if (this.idSarau == null)
              this.sarau.Id = retorno.Id;

            this.dialogRef.close(this.sarau || null);
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

  clicarIncluirParticipante(): void {
    this.dlgsInscricao.apresentarDlgPesquisa(this.idEvento)
      .subscribe(
        (inscricaoSelecionada) => {
          if (this.sarau.Participantes.find(x => x.Id == inscricaoSelecionada.IdInscricao) == null)
            this.sarau.Participantes.push({
              Cidade: inscricaoSelecionada.Cidade,
              Id: inscricaoSelecionada.IdInscricao,
              IdEvento: inscricaoSelecionada.IdEvento,
              Nome: inscricaoSelecionada.NomeInscrito,
              UF: inscricaoSelecionada.UF
            });
          else
            this.alertas.alertarAtencao("A inscrição escolhida já é participante da apresentação!!", "");
        }
      );
  }

  clicarExcluirParticipante(inscrito: DTOInscricaoSimplificada): void {
    this.alertas.alertarConfirmacao("Deseja remover este participante?", "")
      .subscribe(
        (escolha) => {
          if (escolha == CaixaMensagemResposta.Sim)
            this.sarau.Participantes = this.sarau.Participantes.filter(x => x.Id != inscrito.Id);
        }
      );
  }
}

@Injectable()
export class DialogosSarau {

  constructor(private srvDialog: MatDialog) { }

  apresentarDlgFormDialogoInclusao(idEvento: number): Observable<DTOSarau> {
    const dlg = this.srvDialog.open(DlgFormSarau, { data: { idEvento: idEvento, idSarau: null }, width: '70vm', height: '70vh' });
    return dlg.afterClosed();
  }

  apresentarDlgFormDialogoEdicao(idEvento: number, idSarau: number): Observable<DTOSarau> {
    const dlg = this.srvDialog.open(DlgFormSarau, { data: { idEvento: idEvento, idSarau: idSarau }, width: '70vm', height: '70vh' });
    return dlg.afterClosed();
  }
}
