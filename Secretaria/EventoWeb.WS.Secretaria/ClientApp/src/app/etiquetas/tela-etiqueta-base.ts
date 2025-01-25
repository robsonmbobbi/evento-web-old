import { OnInit, Directive } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { CrachaInscrito } from './objetos';
import { WebServiceEtiquetas } from '../webservices/webservice-etiquetas';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { DTOEventoCompleto } from '../evento/objetos';

@Directive()
export abstract class TelaEtiquetaBase implements OnInit {

  public inscricoes: CrachaInscrito[] = [];
  public inscricoesSelecionadas: number[] = [];
  public evento!: DTOEventoCompleto;
  private m_IdEvento!: number;

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

    this.wsEtiquetas.obterTodos(this.m_IdEvento)
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
