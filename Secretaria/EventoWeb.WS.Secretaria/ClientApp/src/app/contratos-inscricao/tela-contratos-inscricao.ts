import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTOContratoInscricao } from './objetos';
import { WebServiceContratosInscricao } from '../webservices/webservice-contratos-inscricao';

@Component({
  selector: 'tela-contratos-inscricao',
  styleUrls: ['./tela-contratos-inscricao.scss'],
  templateUrl: './tela-contratos-inscricao.html'
})
export class TelaContratosInscricao implements OnInit {

  private evento!: DTOEventoCompleto;
  contrato!: DTOContratoInscricao;

  constructor(private wsContratos: WebServiceContratosInscricao, private mensageria: Alertas,
    private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {

    this.contrato = new DTOContratoInscricao();

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    let dlg = this.mensageria.alertarProcessamento("Buscando contrato...");

    this.wsContratos.obter(this.evento.Id)
      .subscribe(
        (contratoRetornado) => {
          if (contratoRetornado != null)
            this.contrato = contratoRetornado;
          dlg.close();
        },
        (erro) => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }

  clicarSalvar(): void {
    if (this.contrato.Regulamento == null || this.contrato.Regulamento.trim().length == 0)
      this.mensageria.alertarAtencao("Você não informou o regulamento", "");
    else if (this.contrato.InstrucoesPagamento == null || this.contrato.InstrucoesPagamento.trim().length == 0)
      this.mensageria.alertarAtencao("Você não informou as instruções de pagamento", "");
    else if (this.contrato.PassoAPassoInscricao == null || this.contrato.PassoAPassoInscricao.trim().length == 0)
      this.mensageria.alertarAtencao("Você não informou o passo-a-passo da inscrição", "");
    else {
      let dlg = this.mensageria.alertarProcessamento("Salvando contrato...");
      if (this.contrato.Id == null) {
        this.wsContratos.incluir(this.evento.Id, this.contrato)
          .subscribe(
            (id) => {
              this.contrato.Id = id.Id;
              dlg.close();
              this.mensageria.alertarInformacao("Contrato salvo com sucesso!!!", "");
            },
            (erro) => {
              dlg.close();
              this.mensageria.alertarErro(erro);
            }
          );
      }
      else {
        this.wsContratos.atualizar(this.evento.Id, this.contrato)
          .subscribe(
            () => {
              dlg.close();
              this.mensageria.alertarInformacao("Contrato salvo com sucesso!!!", "");
            },
            (erro) => {
              dlg.close();
              this.mensageria.alertarErro(erro);
            }
          );
      }
    }
  }
}
