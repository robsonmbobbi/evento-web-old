import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTOOficina } from './objetos';
import { WebServiceOficinas } from '../webservices/webservice-oficinas';
import { DialogosOficina } from './dlg-form-oficina';


@Component({
  selector: 'tela-listagem-oficinas',
  styleUrls: ['./tela-listagem-oficinas.scss'],
  templateUrl: './tela-listagem-oficinas.html'
})
export class TelaListagemOficinas implements OnInit {
 
  oficinas: DTOOficina[] = [];

  private m_Evento!: DTOEventoCompleto;

  constructor(private wsoficinas: WebServiceOficinas, private mensageria: Alertas, private roteador: ActivatedRoute,
    private dialogosOficina: DialogosOficina, private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando oficinas...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsoficinas.obterTodas(this.m_Evento.Id)
      .subscribe(
        oficinas => {
          this.oficinas = oficinas;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.dialogosOficina.apresentarDlgFormDialogoInclusao(this.m_Evento.Id)
      .subscribe(
        (novaoficina) => {
          if (novaoficina != null)
            this.oficinas.push(novaoficina);
        }
      );
  }

  clicarEditar(oficina: DTOOficina): void {
    this.dialogosOficina.apresentarDlgFormDialogoEdicao(this.m_Evento.Id, oficina.Id)
      .subscribe(
        (oficinaAlterada) => {
          if (oficinaAlterada != null) {
            let indice = this.oficinas.findIndex(x => x.Id == oficinaAlterada.Id);
            if (indice != -1) {
              this.oficinas[indice] = oficinaAlterada;
            }
          }            
        }
      );
  }

  clicarExcluir(oficina: DTOOficina): void {
    this.mensageria.alertarConfirmacao("Você quer excluir esta oficina mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de oficina...");
            this.wsoficinas.excluir(this.m_Evento.Id, oficina.Id)
              .subscribe(
                exclusao => {
                  this.oficinas = this.oficinas.filter(x => x.Id != oficina.Id);
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
