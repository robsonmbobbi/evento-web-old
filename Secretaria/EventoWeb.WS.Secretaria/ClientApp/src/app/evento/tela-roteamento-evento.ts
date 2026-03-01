import { Component, OnInit, OnDestroy, Injectable } from '@angular/core';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { DTOEventoCompleto } from './objetos';
import { Subscription } from 'rxjs';

@Component({
  selector: 'tela-roteamento-evento',
  templateUrl: './tela-roteamento-evento.html'
})
export class TelaRoteamentoEvento implements OnInit, OnDestroy {

  evento: DTOEventoCompleto = new DTOEventoCompleto();
  parametrosPag!: Subscription;

  constructor(private wsEventos: WebServiceEventos, private mensageria: Alertas,
    private roteador: ActivatedRoute, private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {
    this.parametrosPag = this.roteador.params.subscribe(parametros => {
      let dlg = this.mensageria.alertarProcessamento("Buscando dados do evento");

      this.wsEventos.obterId(+parametros["id"])
        .subscribe(evento => {
          this.evento = evento;
          this.srvEventoSelecionado.EventoSelecionado = this.evento;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
          })
    });
  }

  ngOnDestroy(): void {
    this.parametrosPag.unsubscribe();
  }
}

@Injectable()
export class ServicoEventoSelecionado {
  private m_Evento!: DTOEventoCompleto;

  get EventoSelecionado(): DTOEventoCompleto {
    return this.m_Evento;
  }

  set EventoSelecionado(valor: DTOEventoCompleto) {
    this.m_Evento = valor;
  }
}
