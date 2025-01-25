import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { WebServiceEstatisticas } from '../webservices/webservice-estatisticas';
import { DTOEstatisticaGeral } from './objetos';
import { DTOEventoCompleto } from '../evento/objetos';

@Component({
  selector: 'tela-estatisticas',
  styleUrls: ['./tela-estatisticas.scss'],
  templateUrl: './tela-estatisticas.html'
})
export class TelaEstatisticas implements OnInit {

  estatistica!: DTOEstatisticaGeral;
  opcoes: string[] = ["Geral","Evangelização","Por Cidade","Carnes não comem","Medicamentos","Alergias"];

  private m_Evento!: DTOEventoCompleto;
  private m_OpcaoEscolhida: string = this.opcoes[0];

  constructor(private wsEstatistica: WebServiceEstatisticas, private mensageria: Alertas, private roteador: ActivatedRoute,
    private srvEventoSelecionado: ServicoEventoSelecionado) { }

  public set opcaoEscolhida(valor: string[]) {
    this.m_OpcaoEscolhida = valor[0];
    this.mensageria.alertarAtencao("teste", this.m_OpcaoEscolhida);
    // faz alguma coisa com isso
  }

  public get opcaoEscolhida(): string[] {
    return [this.m_OpcaoEscolhida];
  }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando estatísticas...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsEstatistica.obter(this.m_Evento.Id)
      .subscribe(
        estatistica => {
          this.estatistica = estatistica;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }
}
