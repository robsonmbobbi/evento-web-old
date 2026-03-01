import { Component, OnInit } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { DTOEventoCompleto } from './objetos';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';
import { ServicoDlgFormEvento } from './dlg-form-evento';

@Component({
  selector: 'tela-gestao-evento',
  styleUrls: ['./tela-gestao-evento.scss'],
  templateUrl: './tela-gestao-evento.html'
})
export class TelaGestaoEvento implements OnInit {

  evento: DTOEventoCompleto = new DTOEventoCompleto();

  constructor(private wsEventos: WebServiceEventos, private mensageria: Alertas, private roteador: ActivatedRoute,
    private wsRelatorios: WebServiceRelatorios, private dialogosEvento: ServicoDlgFormEvento) { }

  ngOnInit(): void {
    this.roteador.params.subscribe(parametros => {
      let dlg = this.mensageria.alertarProcessamento("Buscando dados do evento");

      this.wsEventos.obterId(+parametros["id"])
        .subscribe(evento => {
          this.evento = evento;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
    });
  }

  clicarEditar(): void {
    this.dialogosEvento.apresentarDlgAlteracao(this.evento.Id)
      .subscribe(
        eventoAtualizado => {
          if (eventoAtualizado != null)
            this.evento = eventoAtualizado;
        }
      );
  }

  clicarInscritosDepartamentos(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório inscritos departamentos...");

    this.wsRelatorios.obterInscritosDepartamentos(this.evento.Id)
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
}
