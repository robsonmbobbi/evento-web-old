import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DialogosQuarto } from './dlg-form-quarto';
import { DTOQuarto } from './objetos';
import { WebServiceQuartos } from '../webservices/webservice-quartos';


@Component({
  selector: 'tela-listagem-quartos',
  styleUrls: ['./tela-listagem-quartos.scss'],
  templateUrl: './tela-listagem-quartos.html'
})
export class TelaListagemQuartos implements OnInit {
 
  quartos: DTOQuarto[] = [];
  sexos: string[] = ["Masculino", "Feminino", "Misto"];

  private m_Evento!: DTOEventoCompleto;

  constructor(private wsQuartos: WebServiceQuartos, private mensageria: Alertas, private roteador: ActivatedRoute,
    private dialogosQuarto: DialogosQuarto, private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando quartos...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsQuartos.obterTodos(this.m_Evento.Id)
      .subscribe(
        quartos => {
          this.quartos = quartos;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.dialogosQuarto.apresentarDlgFormDialogoInclusao(this.m_Evento.Id)
      .subscribe(
        (novaquarto) => {
          if (novaquarto != null)
            this.quartos.push(novaquarto);
        }
      );
  }

  clicarEditar(quarto: DTOQuarto): void {
    this.dialogosQuarto.apresentarDlgFormDialogoEdicao(this.m_Evento.Id, quarto.Id)
      .subscribe(
        (quartoAlterada) => {
          if (quartoAlterada != null) {
            let indice = this.quartos.findIndex(x => x.Id == quartoAlterada.Id);
            if (indice != -1) {
              this.quartos[indice] = quartoAlterada;
            }
          }            
        }
      );
  }

  clicarExcluir(quarto: DTOQuarto): void {
    this.mensageria.alertarConfirmacao("Você quer excluir este quarto mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de quarto...");
            this.wsQuartos.excluir(this.m_Evento.Id, quarto.Id)
              .subscribe(
                exclusao => {
                  this.quartos = this.quartos.filter(x => x.Id != quarto.Id);
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
}
