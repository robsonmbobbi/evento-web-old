import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTOEventoMinimo } from './objetos';
import { ServicoDlgFormEvento } from './dlg-form-evento';

import { Router, ActivatedRoute } from '@angular/router';
import { OperacoesImagem } from '../util/util-imagem';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';

@Component({
  selector: 'tela-lista-eventos',
  templateUrl: './tela-lista-eventos.html',
  styleUrls: ["./tela-lista-eventos.scss"]
})
export class TelaListaEventos implements OnInit {

  eventos!: DTOEventoMinimo[];

  constructor(public wsEventos: WebServiceEventos,
    public alertas: Alertas,
    public srv: ServicoDlgFormEvento,
    public router: Router,
    public route: ActivatedRoute) { }

  ngOnInit(): void {

    this.eventos = [];

    let dlg = this.alertas.alertarProcessamento("Buscando eventos...");

    this.wsEventos.obterTodos()
      .subscribe(eventosRetornados => {

        this.eventos = eventosRetornados;

        dlg.close();
      },
        erro => {
          dlg.close();
          this.alertas.alertarErro(erro);
        });
  }

  clicarNovo(): void {
    this.srv.apresentarDlgInclusao()
      .subscribe(retorno => {
        if (retorno != null)
          this.router.navigate(['evento', retorno.Id], { relativeTo: this.route });
      });
  }

  obterImagem(evento: DTOEventoMinimo): string {
    return OperacoesImagem.obterImagemOuSemImagem(evento.Logotipo);
  }

  clicarExcluir(evento: DTOEventoMinimo): void {
    this.alertas.alertarConfirmacao("Você deseja realmente excluir este evento?", "")
      .subscribe((retorno) => {
        if (retorno == CaixaMensagemResposta.Sim) {
          let dlg = this.alertas.alertarProcessamento("Processando exclusão...");
          this.wsEventos.excluir(evento.Id)
            .subscribe((resposta) => {

              this.eventos = this.eventos.filter(x => x.Id != evento.Id);
              dlg.close();
            },
              (erro) => {
                dlg.close();
                this.alertas.alertarErro(erro);
              });
        }
      });
  }
}
