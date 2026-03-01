import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { WebServiceMensagensInscricao } from '../webservices/webservice-mensagens-email-inscricao';
import { DTOMensagemEmailInscricao, DTOModeloMensagem } from './objetos';

@Component({
  selector: 'tela-mensagens-email-inscricao',
  styleUrls: ['./tela-mensagens-email-inscricao.scss'],
  templateUrl: './tela-mensagens-email-inscricao.html'
})
export class TelaMensagensEmailInscricao implements OnInit {

  private evento!: DTOEventoCompleto;
  mensagens!: DTOMensagemEmailInscricao;

  constructor(private wsMensagens: WebServiceMensagensInscricao, private mensageria: Alertas,
    private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {

    this.mensagens = new DTOMensagemEmailInscricao();
    this.mensagens.MensagemInscricaoCodigoAcessoAcompanhamento = new DTOModeloMensagem();
    this.mensagens.MensagemInscricaoCodigoAcessoCriacao = new DTOModeloMensagem();
    this.mensagens.MensagemInscricaoConfirmada = new DTOModeloMensagem();
    this.mensagens.MensagemInscricaoRegistradaAdulto = new DTOModeloMensagem();
    this.mensagens.MensagemInscricaoRegistradaInfantil = new DTOModeloMensagem();

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    let dlg = this.mensageria.alertarProcessamento("Buscando mensagens...");

    this.wsMensagens.obter(this.evento.Id)
      .subscribe(
        (mensagemRetornada) => {
          if (mensagemRetornada != null)
            this.mensagens = mensagemRetornada;
          dlg.close();
        },
        (erro) => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }

  clicarSalvar(): void {
    let dlg = this.mensageria.alertarProcessamento("Salvando mensagens...");
    this.wsMensagens.atualizar(this.evento.Id, this.mensagens)
      .subscribe(
        () => {
          dlg.close();
          this.mensageria.alertarInformacao("Mensagens salvas com sucesso!!!", "");
        },
        (erro) => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }
}
