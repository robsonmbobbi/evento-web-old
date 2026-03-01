import { OnInit, Directive } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { CrachaInscrito, EnumFiltroCracha } from './objetos';
import { WebServiceEtiquetas } from '../webservices/webservice-etiquetas';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { DTOEventoCompleto } from '../evento/objetos';

@Directive()
export abstract class TelaEtiquetaBase implements OnInit {

  public inscricoes: CrachaInscrito[] = [];
  public inscricoesSelecionadas: number[] = [];
  public evento!: DTOEventoCompleto;
  public filtros: string[] = ["Participantes e Participantes/Trabalhadores", "Todos os inscritos (exceto crianças)"];
  private m_FiltroEscolhido: string = "Participantes e Participantes/Trabalhadores";
  private m_IdEvento!: number;

  set filtroEscolhido(valor: string) {
    if (valor != this.m_FiltroEscolhido) {
      this.m_FiltroEscolhido = valor;

      this.carregarInscricoes();
    }
  }

  get filtroEscolhido(): string {
    return this.m_FiltroEscolhido;
  }

  constructor(
    public tituloTela: string,
    public iconeFontAwesome: string,
    protected wsEventos: WebServiceEventos,
    protected wsEtiquetas: WebServiceEtiquetas,
    protected alertas: Alertas,
    protected roteador: ActivatedRoute) { }

  ngOnInit(): void {

    this.roteador.parent!.params.subscribe(parametros => {
      this.m_IdEvento = +parametros["id"];

      this.wsEventos.obterId(+parametros["id"])
        .subscribe(evento => {
          this.evento = evento;
          this.carregarInscricoes();
        },
          erro => {
            this.alertas.alertarErro(erro);
          });
    });
  }

  private carregarInscricoes(): void {
    var dlg = this.alertas.alertarProcessamento("Buscando incrições...");

    var filtro = this.filtros.findIndex(x => x == this.m_FiltroEscolhido);

    this.wsEtiquetas.obterTodos(this.m_IdEvento, filtro == 0 ? EnumFiltroCracha.ParticipantesEPartTrab : EnumFiltroCracha.ParticipantesEPartTrabETrabalhadores)
      .subscribe(
        (inscricoes) => {
          this.inscricoes = inscricoes;
          dlg.close();
        },
        (erro) => {
          dlg.close();
          this.alertas.alertarProcessamento(erro);
        }
      );
  }

  public clicarAtualizar(): void {
    this.alertas.alertarConfirmacao("Você deseja atualizar as inscrições?", "Ao executar essa ação, a seleção para impressão poderá ser perdida.")
      .subscribe(
        resposta => {
          if (resposta != null && resposta == CaixaMensagemResposta.Sim)
            this.carregarInscricoes();
        }
      );
  }

  public abstract clicarGerar(): void;
}
