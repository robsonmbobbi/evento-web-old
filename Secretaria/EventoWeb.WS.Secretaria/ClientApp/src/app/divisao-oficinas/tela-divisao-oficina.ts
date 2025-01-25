import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTOBasicoInscricao } from '../inscricao/objetos';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';
import { DTODivisaoOficina } from './objetos';
import { WebServiceDivisaoOficinas } from '../webservices/webservice-divisao-oficinas';

@Component({
  selector: 'tela-divisao-oficina',
  styleUrls: ['./tela-divisao-oficina.scss'],
  templateUrl: './tela-divisao-oficina.html'
})
export class TelaDivisaoOficina implements OnInit {

  private evento!: DTOEventoCompleto;
  divisoesOficinas: DTODivisaoOficina[] = [];
  inscricoesNaoDistribuidas: DTODivisaoOficina | null = null;

  divisaoSelecionada: DTODivisaoOficina | null = null;

  constructor(private wsDivisao: WebServiceDivisaoOficinas, private mensageria: Alertas,
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

    let dlg = this.mensageria.alertarProcessamento("Buscando as divisões de oficinas existentes...");

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
    this.mensageria.alertarConfirmacao("Deseja excluir a divisão atual?", "Ao realizar isso, nenhuma oficina terá participantes!!")
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

  private processarRetornoDivisao(divisoes: DTODivisaoOficina[]) {

    this.inscricoesNaoDistribuidas = divisoes.find(x => x.Id == 0) ?? null;

    this.divisoesOficinas = divisoes.filter(x => x.Id != 0);   
  }

  private selecionarPrimeiraDivisao(): void {
    if (this.divisoesOficinas.length > 0)
      this.divisaoSelecionada = this.divisoesOficinas[0];
    else
      this.divisaoSelecionada = null;
  }

  clicarImprimir(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório da divisão...");

    this.wsRelatorios.obterDivisaoOficinas(this.evento.Id)
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

  clicarSelecionarOficina(divisao: DTODivisaoOficina): void {
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

  clicarMoverParaOficina(ev: any): void {
    
    this.mensageria.alertarConfirmacao("Você está certo de mudar a oficina deste participante?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Movendo participante...");

            this.wsDivisao.moverInscricaoOficinas(this.evento.Id, this.divisaoSelecionada!.Id, ev.oficina.Id, ev.inscricao.IdInscricao)
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

  clicarIncluirNaOficina(ev: any): void {
    this.mensageria.alertarConfirmacao("Você está certo de incluir este participante na oficina " + ev.oficina.Nome + "?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Incluindo participante...");

            this.wsDivisao.incluirInscricaoOficina(this.evento.Id, ev.oficina.Id, ev.inscricao.IdInscricao)
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

  clicarRemoverDaOficina(inscricao: DTOBasicoInscricao): void {
    this.mensageria.alertarConfirmacao("Você está certo de remover este participante da oficina?", "Essa inscrição será listada nas inscrições sem oficina")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Removendo participante...");

            this.wsDivisao.removerInscricaoOficina(this.evento.Id, this.divisaoSelecionada!.Id, inscricao.IdInscricao)
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
      this.divisaoSelecionada = this.divisoesOficinas.find(x => x.Id == id) ?? null;

      if (this.divisaoSelecionada == null)
        this.selecionarPrimeiraDivisao();
    }
  }
}
