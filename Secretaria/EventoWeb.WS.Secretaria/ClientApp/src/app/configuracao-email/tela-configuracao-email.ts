import { Component, OnInit } from '@angular/core';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTOConfiguracaoEmail } from './objetos';
import { WebServiceConfiguracaoEmail } from '../webservices/webservice-configuracao-email';

@Component({
  selector: 'tela-configuracao-email',
  styleUrls: ['./tela-configuracao-email.scss'],
  templateUrl: './tela-configuracao-email.html'
})
export class TelaConfiguracaoEmail implements OnInit {

  private evento!: DTOEventoCompleto;
  configuracao!: DTOConfiguracaoEmail;
  tiposSeguranca: string[] = ["SSL", "Nenhuma"];

  constructor(private wsConfiguracaoEmail: WebServiceConfiguracaoEmail, private mensageria: Alertas,
    private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {

    this.configuracao = new DTOConfiguracaoEmail();

    this.evento = this.srvEventoSelecionado.EventoSelecionado;
    let dlg = this.mensageria.alertarProcessamento("Buscando configuracao...");

    this.wsConfiguracaoEmail.obter(this.evento.Id)
      .subscribe(
        (configuracaoRetornada) => {
          if (configuracaoRetornada != null)
            this.configuracao = configuracaoRetornada;
          dlg.close();
        },
        (erro) => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        }
      );
  }

  clicarSalvar(): void {

    if (this.configuracao.EnderecoEmail == null || this.configuracao.EnderecoEmail.trim().length == 0)
      this.mensageria.alertarAtencao("Você precisa informar o endereço de Email", "");
    else if (this.configuracao.PortaServidor == null || this.configuracao.PortaServidor <= 0)
      this.mensageria.alertarAtencao("Você precisa informar a Porta do Servidor de Email", "A porta deve ser um número maior que zero.");
    else if (this.configuracao.SenhaEmail == null || this.configuracao.SenhaEmail.trim().length == 0)
      this.mensageria.alertarAtencao("Você precisa informar a Senha de acesso de Email", "");
    else if (this.configuracao.ServidorEmail == null || this.configuracao.ServidorEmail.trim().length == 0)
      this.mensageria.alertarAtencao("Você precisa informar a Servidor de acesso de Email", "");
    else if (this.configuracao.TipoSeguranca == null)
      this.mensageria.alertarAtencao("Você precisa informar o Tipo de Segurança usada", "");
    else if (this.configuracao.UsuarioEmail == null || this.configuracao.UsuarioEmail.trim().length == 0)
      this.mensageria.alertarAtencao("Você precisa informar o Usuário", "");
    else {
      let dlg = this.mensageria.alertarProcessamento("Salvando configuração...");
      this.wsConfiguracaoEmail.atualizar(this.evento.Id, this.configuracao)
        .subscribe(
          () => {
            dlg.close();
            this.mensageria.alertarInformacao("Configuração salva com sucesso!!!", "");
          },
          (erro) => {
            dlg.close();
            this.mensageria.alertarErro(erro);
          }
        );
    }
  }

  get tipoSegurancaEscolhida(): string | null {
    return (this.configuracao.TipoSeguranca != null ? this.tiposSeguranca[this.configuracao.TipoSeguranca] : null);
  }

  set tipoSegurancaEscolhida(valor: string) {
    let indice = this.tiposSeguranca.findIndex(x => x == valor);
    if (indice != -1)
      this.configuracao.TipoSeguranca = indice;
    else
      this.configuracao.TipoSeguranca = null;
  }
}
