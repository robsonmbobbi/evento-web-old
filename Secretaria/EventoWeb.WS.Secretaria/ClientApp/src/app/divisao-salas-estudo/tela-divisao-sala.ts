import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ActivatedRoute } from '@angular/router';
import { WebServiceDivisaoSalas } from '../webservices/webservice-divisao-salas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTODivisaoSalaEstudo } from './objetos';
import { DTOBasicoInscricao } from '../inscricao/objetos';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';

@Component({
  selector: 'tela-divisao-sala',
  styleUrls: ['./tela-divisao-sala.scss'],
  templateUrl: './tela-divisao-sala.html'
})
export class TelaDivisaoSala implements OnInit {

  private evento!: DTOEventoCompleto;
  divisoesSalas: DTODivisaoSalaEstudo[] = [];
  inscricoesNaoDistribuidas!: DTODivisaoSalaEstudo;

  divisaoSelecionada: DTODivisaoSalaEstudo | null = null;

  constructor(private wsDivisao: WebServiceDivisaoSalas, private mensageria: Alertas,
    private srvEventoSelecionado: ServicoEventoSelecionado, private wsRelatorios: WebServiceRelatorios) {
    this.inscricoesNaoDistribuidas = {
      Id: 0,
      Nome: "",
      Coordenadores: [],
      Participantes: []
    };
  }

  ngOnInit(): void {

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    this.clicarAtualizar();
  }

  clicarAtualizar(): void {

    let dlg = this.mensageria.alertarProcessamento("Buscando as divisões de salas existentes...");

    this.wsDivisao.obterTodas(this.evento.Id)
      .subscribe(
        divisoes => {
          this.processarRetornoDivisao(divisoes);
          this.selecionarPrimeiraDivisao();
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarDividir(): void {
    this.mensageria.alertarConfirmacao("Deseja realizar a divisão automática?", "Ao realizar isso, as divisões existentes serão perdidas!!")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Realizando divisão...");

            this.wsDivisao.realizarDivisaoAutomatica(this.evento.Id)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
                  this.selecionarPrimeiraDivisao();
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

  clicarRemover(): void {
    this.mensageria.alertarConfirmacao("Deseja excluir a divisão atual?", "Ao realizar isso, nenhuma sala terá participantes!!")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Excluindo divisão...");

            this.wsDivisao.removerTudo(this.evento.Id)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
                  this.selecionarPrimeiraDivisao();
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

  private processarRetornoDivisao(divisoes: DTODivisaoSalaEstudo[]) {

    this.inscricoesNaoDistribuidas = divisoes.find(x => x.Id == 0) ?? new DTODivisaoSalaEstudo();

    this.divisoesSalas = divisoes.filter(x => x.Id != 0);   
  }

  private selecionarPrimeiraDivisao(): void {
    this.divisaoSelecionada = null;

    if (this.divisoesSalas.length > 0)
      this.divisaoSelecionada = this.divisoesSalas[0];
  }

  clicarImprimir(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório da divisão...");

    this.wsRelatorios.obterDivisaoSalas(this.evento.Id)
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

  clicarSelecionarSala(divisao: DTODivisaoSalaEstudo): void {
    this.divisaoSelecionada = divisao;
  }

  calcularIdade(inscricao: DTOBasicoInscricao): number {

    let dataAtual = new Date(this.evento.PeriodoRealizacao.DataInicial);
    let dataNascimento = new Date(inscricao.DataNascimento);

    let idade = (dataAtual.getFullYear() - dataNascimento.getFullYear() - 1) +
      (((dataAtual.getMonth() > dataNascimento.getMonth()) ||
        ((dataAtual.getMonth() == dataNascimento.getMonth()) &&
          (dataAtual.getDate() >= dataNascimento.getDate()))) ? 1 : 0);

    return idade;
  }

  clicarMoverParaSala(ev: any): void {
    
    this.mensageria.alertarConfirmacao("Você está certo de mudar a sala de estudo deste participante?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Movendo participante...");

            this.wsDivisao.moverInscricaoSalas(this.evento.Id, this.divisaoSelecionada!.Id, ev.sala.Id, ev.inscricao.IdInscricao)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
                  this.atualizarDivisaoSelecionada();
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

  clicarIncluirNaSala(ev: any): void {
    this.mensageria.alertarConfirmacao("Você está certo de incluir este participante na sala " + ev.sala.Nome + "?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Incluindo participante...");

            this.wsDivisao.incluirInscricaoSala(this.evento.Id, ev.sala.Id, ev.inscricao.IdInscricao)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
                  this.atualizarDivisaoSelecionada();
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

  clicarRemoverDaSala(inscricao: DTOBasicoInscricao): void {
    this.mensageria.alertarConfirmacao("Você está certo de remover este participante da sala de estudo?", "Essa inscrição será listada nas inscrições sem sala de estudo")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Removendo participante...");

            this.wsDivisao.removerInscricaoSala(this.evento.Id, this.divisaoSelecionada!.Id, inscricao.IdInscricao)
              .subscribe(
                divisoes => {
                  this.processarRetornoDivisao(divisoes);
                  this.atualizarDivisaoSelecionada();
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

  private atualizarDivisaoSelecionada() {
    if (this.divisaoSelecionada != null) {
      let id = this.divisaoSelecionada.Id;
      this.divisaoSelecionada = null;
      this.divisaoSelecionada = this.divisoesSalas.find(x => x.Id == id) ?? new DTODivisaoSalaEstudo();

      if (this.divisaoSelecionada == null)
        this.selecionarPrimeiraDivisao();
    }
  }
}
