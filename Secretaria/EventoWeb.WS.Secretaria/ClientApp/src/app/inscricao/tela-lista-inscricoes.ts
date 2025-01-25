import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { EnumSituacaoInscricao, DTOBasicoInscricao } from './objetos';
import { WebServiceInscricoes } from '../webservices/webservice-inscricoes';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { WebServiceEventos } from '../webservices/webservice-eventos';
import { DTOEventoCompleto } from '../evento/objetos';

@Component({
  selector: 'tela-lista-inscricoes',
  templateUrl: './tela-lista-inscricoes.html',
  styleUrls: ["./tela-lista-inscricoes.scss"]
})
export class TelaListagemInscricoes implements OnInit {

  public filtros: string[] = ["Incompleta", "Pendente", "Aceita", "Rejeitada"];
  public inscricoes: DTOBasicoInscricao[] = [];
  public evento!: DTOEventoCompleto;
  private m_FiltroEscolhido: string = "Pendente";
  private m_IdEvento!: number;

  constructor(private wsInscricoes: WebServiceInscricoes,
    private wsEventos: WebServiceEventos,
    private alertas: Alertas,
    private roteador: ActivatedRoute) { }

  set filtroEscolhido(valor: string) {
    if (valor != this.m_FiltroEscolhido) {
      this.m_FiltroEscolhido = valor;

      let dlg = this.alertas.alertarProcessamento("Pesquisando inscrições....");
      this.wsInscricoes.obterTodas(this.m_IdEvento, this.filtros.findIndex(x => x == this.m_FiltroEscolhido))
        .subscribe(
          inscricoesRetornadas => {
            dlg.close();
            this.inscricoes = inscricoesRetornadas.map(x => {
              x.DataNascimento = new Date(x.DataNascimento);
              return x;
            });
          },
          (erro) => {
            dlg.close();

            this.alertas.alertarErro(erro);
          }
        );
    }
  }

  get filtroEscolhido(): string {
    return this.m_FiltroEscolhido;
  }

  ngOnInit(): void {

    this.roteador.parent!.params.subscribe(parametros => {
      this.m_IdEvento = +parametros["id"];
      this.filtroEscolhido = this.filtros[1];

      this.wsEventos.obterId(this.m_IdEvento)
        .subscribe(
          (evento) => {
            this.evento = evento;
          },
          (erro) => this.alertas.alertarProcessamento(erro)
        );
    });
  }

  obterTextoSituacao(situacao: EnumSituacaoInscricao): string {
    switch (situacao) {
      case EnumSituacaoInscricao.Aceita:
        return "Aceita";
      case EnumSituacaoInscricao.Pendente:
        return "Pendente";
      case EnumSituacaoInscricao.Rejeitada:
        return "Rejeitada";
      default:
        return "";
    }
  }

  excluir(inscricao: DTOBasicoInscricao): void {
    this.alertas.alertarConfirmacao("Você deseja realmente excluir esta inscrição?", "")
      .subscribe((retorno) => {
        if (retorno == CaixaMensagemResposta.Sim) {
          let dlg = this.alertas.alertarProcessamento("Processando exclusão...");
          this.wsInscricoes.excluir(inscricao.IdEvento, inscricao.IdInscricao)
            .subscribe((resposta) => {

              this.inscricoes = this.inscricoes.filter(x => x.IdInscricao != inscricao.IdInscricao);
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
