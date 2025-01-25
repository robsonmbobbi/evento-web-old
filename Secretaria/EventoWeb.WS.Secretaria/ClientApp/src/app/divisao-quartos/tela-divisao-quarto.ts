import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTODivisaoQuarto } from './objetos';
import { DTOBasicoInscricaoResp } from '../inscricao/objetos';
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { WebServiceRelatorios } from '../webservices/webservice-relatorios';
import { WebServiceDivisaoQuartos } from '../webservices/webservice-divisao-quartos';
import { EnumSexoQuarto } from '../quartos/objetos';

@Component({
  selector: 'tela-divisao-quarto',
  styleUrls: ['./tela-divisao-quarto.scss'],
  templateUrl: './tela-divisao-quarto.html'
})
export class TelaDivisaoQuarto implements OnInit {

  private evento!: DTOEventoCompleto;
  divisoesQuartos: DTODivisaoQuarto[] = [];
  inscricoesNaoDistribuidas!: DTODivisaoQuarto;
  sexos: string[] = ["Masculino", "Feminino", "Misto"];

  divisaoSelecionada: DTODivisaoQuarto | null = null;

  constructor(private wsDivisao: WebServiceDivisaoQuartos, private mensageria: Alertas,
    private srvEventoSelecionado: ServicoEventoSelecionado, private wsRelatorios: WebServiceRelatorios) {
    this.inscricoesNaoDistribuidas = {
      Id: 0,
      Nome: "",
      Capacidade: null,
      EhFamilia: false,
      Sexo: EnumSexoQuarto.Misto,
      Coordenadores: [],
      Participantes: []
    };
  }

  ngOnInit(): void {

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    this.clicarAtualizar();
  }

  clicarAtualizar(): void {

    let dlg = this.mensageria.alertarProcessamento("Buscando as divisões de quartos existentes...");

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
    this.mensageria.alertarConfirmacao("Deseja excluir a divisão atual?", "Ao realizar isso, nenhum quarto terá participantes!!")
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

  private processarRetornoDivisao(divisoes: DTODivisaoQuarto[]) {

    this.inscricoesNaoDistribuidas = divisoes.find(x => x.Id == 0) ?? new DTODivisaoQuarto();

    this.divisoesQuartos = divisoes.filter(x => x.Id != 0);   
  }

  private selecionarPrimeiraDivisao(): void {
    this.divisaoSelecionada = null;

    if (this.divisoesQuartos.length > 0)
      this.divisaoSelecionada = this.divisoesQuartos[0];
  }

  clicarImprimir(): void {
    let dlg = this.mensageria.alertarProcessamento("Gerando relatório da divisão...");

    this.wsRelatorios.obterDivisaoQuartos(this.evento.Id)
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

  clicarSelecionarQuarto(divisao: DTODivisaoQuarto): void {
    this.divisaoSelecionada = divisao;
  }

  calcularIdade(inscricao: DTOBasicoInscricaoResp): number {

    let dataAtual = new Date(this.evento.PeriodoRealizacao.DataInicial);
    let dataNascimento = new Date(inscricao.DataNascimento);

    let idade = (dataAtual.getFullYear() - dataNascimento.getFullYear() - 1) +
      (((dataAtual.getMonth() > dataNascimento.getMonth()) ||
        ((dataAtual.getMonth() == dataNascimento.getMonth()) &&
          (dataAtual.getDate() >= dataNascimento.getDate()))) ? 1 : 0);

    return idade;
  }

  clicarMoverParaQuarto(ev: any): void {
    
    this.mensageria.alertarConfirmacao("Você está certo de mudar a quarto deste participante?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Movendo participante...");

            this.wsDivisao.moverInscricaoQuarto(this.evento.Id, this.divisaoSelecionada!.Id, ev.quarto.Id, ev.inscricao.IdInscricao)
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

  clicarIncluirNoQuarto(ev: any): void {
    this.mensageria.alertarConfirmacao("Você está certo de incluir este participante no quarto " + ev.quarto.Nome + "?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Incluindo participante...");

            this.wsDivisao.incluirInscricaoQuarto(this.evento.Id, ev.quarto.Id, ev.inscricao.IdInscricao)
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

  clicarRemoverDoQuarto(inscricao: DTOBasicoInscricaoResp): void {
    this.mensageria.alertarConfirmacao("Você está certo de remover este participante do quarto?", "Essa inscrição será listada nas inscrições sem quarto")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Removendo participante...");

            this.wsDivisao.removerInscricaoQuarto(this.evento.Id, this.divisaoSelecionada!.Id, inscricao.IdInscricao)
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

  clicarTornarCoordenador(inscricao: DTOBasicoInscricaoResp): void {
    this.mensageria.alertarConfirmacao("Você está certo de torná-lo coordenador do quarto?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Alterando participante...");

            this.wsDivisao.definirSeEhCoordenador(this.evento.Id, this.divisaoSelecionada!.Id, inscricao.IdInscricao, true)
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

  clicarTirarCoordenador(inscricao: DTOBasicoInscricaoResp): void {
    this.mensageria.alertarConfirmacao("Você está certo de tirá-lo da coordenação do quarto?", "")
      .subscribe(
        (botaoEscolhido) => {
          if (botaoEscolhido == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Alterando participante...");

            this.wsDivisao.definirSeEhCoordenador(this.evento.Id, this.divisaoSelecionada!.Id, inscricao.IdInscricao, false)
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
      this.divisaoSelecionada = this.divisoesQuartos.find(x => x.Id == id) ?? new DTODivisaoQuarto();

      if (this.divisaoSelecionada == null)
        this.selecionarPrimeiraDivisao();
    }
  }

  gerarTextoResponsaveis(inscricao: DTOBasicoInscricaoResp): string {

    let responsaveis = "";
    for (let indice = 0; indice < inscricao.Responsaveis.length; indice++) {
      if (indice > 0)
        responsaveis += ", ";

      responsaveis += inscricao.Responsaveis[indice].Nome + " - " + inscricao.Responsaveis[indice].Cidade + '/' + inscricao.Responsaveis[indice].UF;
    }
    return responsaveis;
  }
}
