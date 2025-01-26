import { Component, OnInit, ViewChild, Directive } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { ActivatedRoute, Router } from '@angular/router';
import { DTOInscricaoCompleta, EnumSituacaoInscricao, DTOInscricaoAtualizacao, DTOInscricaoDadosPessoais, DTOPagamento } from './objetos';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { DTOEventoCompleto, Periodo } from '../principal/objetos';
import { CompFormInscricao } from './comp-form-inscricao';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { WsEventos } from '../webservices/wsEventos';
import { DialogoValidacaoEmail } from './dlg-validacao-email';
import { md5 } from '../componentes/geracao-md5';

@Directive()
export abstract class ATelaInscricao {
  inscricao: DTOInscricaoAtualizacao;
  evento: DTOEventoCompleto;
  naoEhIncompleta: boolean = false;

  @ViewChild("formInscricao")
  formInscricao: CompFormInscricao;

  constructor(public coordenacao: CoordenacaoCentral, protected navegadorUrl: Router) { }

  clicarVoltar(): void {
    this.navegadorUrl.navigate(['']);
  }

  clicarAtualizar(): void {
    let resultado = this.formInscricao.gerarAtualizacaoInscricao();
    if (resultado.valido)
      this.processarAtualizacao(resultado.inscricaoAtualizar);
  }

  protected abstract processarAtualizacao(inscricao: DTOInscricaoAtualizacao);
}

@Component({
  selector: 'tela-inscricao-atualizacao',
  templateUrl: './tela-inscricao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricaoAtualizacao extends ATelaInscricao implements OnInit {

  inscricaoCompleta: DTOInscricaoCompleta;

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, protected navegadorUrl: Router,
    private wsInscricoes: WsManutencaoInscricoes) {
    super(coordenacao, navegadorUrl);
  }

  ngOnInit(): void {
    this.inscricao = new DTOInscricaoCompleta();
    this.evento = new DTOEventoCompleto();

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idInscricao = parametrosUrl["idinscricao"];

          this.wsInscricoes.obterInscricaoCompleta(idInscricao)
            .subscribe(
              (dadosInscricao) => {
                dlg.close();
                if (dadosInscricao != null) {
                  this.inscricao = dadosInscricao;
                  this.inscricaoCompleta = dadosInscricao;
                  this.evento = dadosInscricao.Evento;
                  this.naoEhIncompleta = this.inscricaoCompleta.Situacao != EnumSituacaoInscricao.Incompleta;
                }
                else
                  this.voltar(idInscricao);

              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
        }
      );
  }

  clicarVoltar(): void {
    this.voltar(this.inscricaoCompleta.Id);
  }

  private voltar(idInscricao: number) {
    this.coordenacao.AutorizacoesInscricao.remover(idInscricao);
    super.clicarVoltar();
  }

  protected processarAtualizacao(inscricao: DTOInscricaoAtualizacao) {

    this.voltar(this.inscricaoCompleta.Id);
  }
}

@Component({
  selector: 'tela-inscricao-inclusao',
  templateUrl: './tela-inscricao.html',
  styleUrls: ['./tela-inscricao.scss']
})
export class TelaInscricaoInclusao extends ATelaInscricao implements OnInit {

  constructor(public coordenacao: CoordenacaoCentral, private rotaAtual: ActivatedRoute, protected navegadorUrl: Router,
    private wsInscricoes: WsInscricoes, private wsEventos: WsEventos, private dlgValidacaoEmail: DialogoValidacaoEmail) {
    super(coordenacao, navegadorUrl);
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
    this.inscricao.DormeEvento = true;

    this.evento = new DTOEventoCompleto();
    this.evento.PeriodoInscricao = new Periodo();
    this.evento.PeriodoRealizacao = new Periodo();

    this.naoEhIncompleta = false;

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          let idEvento = parametrosUrl["idevento"];

          this.wsEventos.obterCompleto(idEvento)
            .subscribe(
              (evento) => {
                dlg.close();
                if (evento == null)
                  this.clicarVoltar();
                else
                  this.evento = evento;
              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );

        }
      );
  }

  protected processarAtualizacao(inscricao: DTOInscricaoAtualizacao) {

    let dlgEnvioCodigo = this.coordenacao.Alertas.alertarProcessamento("Enviando código de validação...");

    let identificacao = md5(new Date().toISOString() + inscricao.DadosPessoais.Email + inscricao.DadosPessoais.Nome);

    this.wsInscricoes.enviarCodigoValidacaoEmail(this.evento.Id, identificacao, inscricao.DadosPessoais.Email)
      .subscribe(
        () => {
          dlgEnvioCodigo.close();
          this.dlgValidacaoEmail.apresentarDlg(inscricao.DadosPessoais.Email, inscricao.DadosPessoais.Nome, identificacao)
            .subscribe(
              (validou) => {
                if (validou) {
                  let dlg = this.coordenacao.Alertas.alertarProcessamento("Incluindo inscrição...");

                  this.wsInscricoes.criar(this.evento.Id, inscricao)
                    .subscribe(
                      (retorno) => {
                        dlg.close();
                        this.coordenacao.Alertas.alertarInformacao("Inscrição incluída com sucesso!!",
                          "Agora é aguardar a análise da secretaria do evento para que a sua inscrição seja efetivada!");
                        this.clicarVoltar();
                      },
                      (erro) => {
                        dlg.close();
                        this.coordenacao.ProcessamentoErro.processar(erro);
                      }
                    );
                }
              }
            );
        },
        (erro) => {
          dlgEnvioCodigo.close();
          this.coordenacao.ProcessamentoErro.processar(erro);
        }
      );
  }
}
