import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { WebServiceSalas } from "../webservices/webservice-salas";
import { DTOSalaEstudo } from "./objetos";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { DialogosSala } from './dlg-form-sala';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto, EnumModeloDivisaoSalasEstudo } from '../evento/objetos';


@Component({
  selector: 'tela-listagem-salas',
  styleUrls: ['./tela-listagem-salas.scss'],
  templateUrl: './tela-listagem-salas.html'
})
export class TelaListagemSalas implements OnInit {

  salaSelecionada: DTOSalaEstudo | null = null;
  salas: DTOSalaEstudo[] = [];

  private m_Evento!: DTOEventoCompleto;

  constructor(private wsSalas: WebServiceSalas, private mensageria: Alertas, private roteador: ActivatedRoute,
    private dlgsSala: DialogosSala, private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando salas...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsSalas.obterTodas(this.m_Evento.Id)
      .subscribe(
        salas => {
          this.salas = salas;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.dlgsSala.apresentarDlgFormDialogoInclusao(this.m_Evento.Id, this.PodeUsarFaixaEtaria())
      .subscribe(
        (novaSala) => {
          if (novaSala != null)
            this.salas.push(novaSala);
        }
      );
  }

  clicarEditar(sala: DTOSalaEstudo): void {
    this.dlgsSala.apresentarDlgFormDialogoEdicao(this.m_Evento.Id, sala.Id, this.PodeUsarFaixaEtaria())
      .subscribe(
        (salaAlterada) => {
          if (salaAlterada != null) {
            let indice = this.salas.findIndex(x => x.Id == salaAlterada.Id);
            if (indice != -1) {
              this.salas[indice] = salaAlterada;
            }
          }            
        }
      );
  }

  private PodeUsarFaixaEtaria(): boolean {
    return this.m_Evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorIdadeCidade;
  }

  clicarExcluir(sala: DTOSalaEstudo): void {
    this.mensageria.alertarConfirmacao("Você quer excluir esta sala mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de sala...");
            this.wsSalas.excluir(this.m_Evento.Id, sala.Id)
              .subscribe(
                exclusao => {
                  this.salas = this.salas.filter(x => x.Id != sala.Id);
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
