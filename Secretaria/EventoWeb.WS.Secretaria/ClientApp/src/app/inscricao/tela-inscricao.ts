import { Component, OnInit, ViewChild, Directive } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  DTOInscricaoCompleta, DTOPagamento, DTOInscricaoAtualizacao, DTOInscricaoDadosPessoais
} from './objetos';
import { DTOEventoCompletoInscricao, Periodo } from '../evento/objetos';
import { WebServiceInscricoes } from '../webservices/webservice-inscricoes';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { CompFormInscricao, ResultadoAtualizacaoInscricao } from './comp-form-inscricao';
import { WebServiceEventos } from '../webservices/webservice-eventos';

@Directive()
export abstract class ATelaInscricao {
  inscricao: DTOInscricaoAtualizacao;
  evento: DTOEventoCompletoInscricao;
  naoEhIncompleta: boolean = false;

  @ViewChild("formInscricao")
  formInscricao: CompFormInscricao;

  constructor(protected navegadorUrl: Router) { }

  clicarVoltar(): void {
    this.navegadorUrl.navigate(['evento/' + this.evento.Id.toString() + '/inscricoes']);
  }

  protected obterAtualizacao(): ResultadoAtualizacaoInscricao {
    return this.formInscricao.gerarAtualizacaoInscricao();
  }
}


@Component({
  selector: 'tela-inscricao',
  templateUrl: './tela-inscricao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricao extends ATelaInscricao implements OnInit {

  inscricaoCompleta: DTOInscricaoCompleta;
  NaoEhIncompleta: boolean = false;

  constructor(private mensageria: Alertas, private rotaAtual: ActivatedRoute, protected override navegadorUrl: Router, private wsInscricoes: WebServiceInscricoes) {
    super(navegadorUrl);
  }

  ngOnInit(): void {
    this.inscricao = new DTOInscricaoCompleta();
    this.evento = new DTOEventoCompletoInscricao();

    let dlg = this.mensageria.alertarProcessamento("Carregando dados...");

    this.rotaAtual.parent.params
      .subscribe(
        (parametrosUrlPai) => {
          let idEvento = parametrosUrlPai["id"];

          this.rotaAtual.params
            .subscribe(
              (parametrosUrl) => {
                let idInscricao = parametrosUrl["idInscricao"];

                this.wsInscricoes.obterInscricaoCompleta(idEvento, idInscricao)
                  .subscribe(
                    (dadosInscricao) => {
                      dlg.close();
                      if (dadosInscricao != null) {
                        this.inscricao = dadosInscricao;
                        this.inscricaoCompleta = dadosInscricao;
                        this.evento = dadosInscricao.Evento;                        

                        this.NaoEhIncompleta = false;
                      }
                      else
                        this.clicarVoltar();
                    },
                    (erro) => {
                      dlg.close();
                      this.mensageria.alertarErro(erro);
                    }
                  );
              }
            );
        }
      );
  }

  clicarAceitar(): void {

    let resultado = this.obterAtualizacao();
    if (resultado.valido) {
      let atualizacao = resultado.inscricaoAtualizar;
      let dlg = this.mensageria.alertarProcessamento("Registrando aceitação...");

      this.wsInscricoes.aceitar(this.evento.Id, this.inscricaoCompleta.Id, atualizacao)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  clicarAtualizar(): void {
    let resultado = this.obterAtualizacao();
    if (resultado.valido) {
      let atualizacao = resultado.inscricaoAtualizar;
      let dlg = this.mensageria.alertarProcessamento("Atualizando...");

      this.wsInscricoes.atualizar(this.evento.Id, this.inscricaoCompleta.Id, atualizacao)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  public clicarRejeitar(): void {

    this.mensageria.alertarConfirmacao("Deseja rejeitar esta Inscrição?", "")
      .subscribe(
        (botaoPressionado) => {
          if (botaoPressionado == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Registrando rejeição...");
            this.wsInscricoes.rejeitar(this.evento.Id, this.inscricaoCompleta.Id)
              .subscribe(
                (retorno) => {
                  dlg.close();
                  this.clicarVoltar();
                },
                (erro) => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                }
              );
          }
        });
  }
}

@Component({
  selector: 'tela-inscricao-inclusao',
  templateUrl: './tela-inscricao-inclusao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricaoInclusao extends ATelaInscricao implements OnInit {

  constructor(private mensageria: Alertas, private rotaAtual: ActivatedRoute, protected override navegadorUrl: Router,
    private wsInscricoes: WebServiceInscricoes, private wsEventos: WebServiceEventos) {
    super(navegadorUrl);
  }

  ngOnInit(): void {
    this.inscricao = new DTOInscricaoAtualizacao();
    this.inscricao.DadosPessoais = new DTOInscricaoDadosPessoais();
    this.inscricao.DadosPessoais.DataNascimento = null;
    this.inscricao.DadosPessoais.EhDiabetico = false;
    this.inscricao.DadosPessoais.EhVegetariano = false;
    this.inscricao.DadosPessoais.UsaAdocanteDiariamente = false;
    this.inscricao.Pagamento = new DTOPagamento();
    this.inscricao.Sarais = [];
    this.inscricao.PrimeiroEncontro = false;
    this.inscricao.DormeEvento = true;

    this.evento = new DTOEventoCompletoInscricao();
    this.evento.PeriodoInscricao = new Periodo();
    this.evento.PeriodoRealizacao = new Periodo();

    this.naoEhIncompleta = false;

    let dlg = this.mensageria.alertarProcessamento("Carregando dados...");

    this.rotaAtual.parent.params
      .subscribe(
        (parametrosUrl) => {
          let idEvento = parametrosUrl["id"];

          this.wsEventos.obterParaInscricao(idEvento)
            .subscribe(
              (evento) => {
                dlg.close(); 
                if (evento == null)
                  this.clicarVoltar();
                else {
                  this.evento = evento;
                }
              },
              (erro) => {
                dlg.close();
                this.mensageria.alertarErro(erro);
              }
            );

        }
      );
  }

  clicarSalvar(): void {
    let resultado = this.obterAtualizacao();
    if (resultado.valido) {
      let dlg = this.mensageria.alertarProcessamento("Atualizando...");
      this.wsInscricoes.incluir(this.evento.Id, resultado.inscricaoAtualizar)
        .subscribe(
          (retorno) => {
            dlg.close();
            this.clicarVoltar();
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }
}
