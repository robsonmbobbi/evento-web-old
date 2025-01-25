import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTOSarau } from './objetos';
import { WebServiceSarais } from '../webservices/webservice-sarais';
import { DialogosSarau } from './dlg-form-sarau';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';


@Component({
  selector: 'tela-listagem-sarais',
  styleUrls: ['./tela-listagem-sarais.scss'],
  templateUrl: './tela-listagem-sarais.html'
})
export class TelaListagemSarais implements OnInit {
 
  sarais: DTOSarau[] = [];

  private m_Evento!: DTOEventoCompleto;

  constructor(private wsSarais: WebServiceSarais, private mensageria: Alertas, private roteador: ActivatedRoute,
    private dialogosSarau: DialogosSarau, private srvEventoSelecionado: ServicoEventoSelecionado,
    private wsRelatorios: WebServiceRelatorios) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando sarais...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsSarais.obterTodos(this.m_Evento.Id)
      .subscribe(
        sarais => {
          this.sarais = sarais;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.dialogosSarau.apresentarDlgFormDialogoInclusao(this.m_Evento.Id)
      .subscribe(
        (novoSarau) => {
          if (novoSarau != null)
            this.sarais.push(novoSarau);
        }
      );
  }

  clicarEditar(sarau: DTOSarau): void {
    this.dialogosSarau.apresentarDlgFormDialogoEdicao(this.m_Evento.Id, sarau.Id)
      .subscribe(
        (sarauAlterado) => {
          if (sarauAlterado != null) {
            let indice = this.sarais.findIndex(x => x.Id == sarauAlterado.Id);
            if (indice != -1) {
              this.sarais[indice] = sarauAlterado;
            }
          }            
        }
      );
  }

  clicarExcluir(sarau: DTOSarau): void {
    this.mensageria.alertarConfirmacao("Você quer excluir esta apresentação mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de apresentação...");
            this.wsSarais.excluir(this.m_Evento.Id, sarau.Id)
              .subscribe(
                exclusao => {
                  this.sarais = this.sarais.filter(x => x.Id != sarau.Id);
                  dlg.close();
                },
                erro => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                });
          }
        }
      );
  }

  clicarImprimir(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório...");

    this.wsRelatorios.obterListagemSarau(this.m_Evento.Id)
      .subscribe(
        relatorioGerado => {
          dlg.close();
          window.open(URL.createObjectURL(relatorioGerado), '_blank');
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }

  gerarTextoParticipantes(sarau: DTOSarau): string {

    let pessoas: string = "";
    for (let inscricao of sarau.Participantes) {

      if (pessoas != "")
        pessoas = pessoas + ", ";

      pessoas = pessoas + inscricao.Nome;        
    }

    return pessoas;
  }
}
