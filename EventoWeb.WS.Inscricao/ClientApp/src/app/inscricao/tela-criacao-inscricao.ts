import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsEventos } from '../webservices/wsEventos';

@Component({
  selector: 'tela-criacao-inscricao',
  templateUrl: './tela-criacao-inscricao.html',
  styleUrls: ['./tela-criacao-inscricao.scss']
})
export class TelaCriacaoInscricao implements OnInit {

  idEvento: number;
  nomeEvento: string;
  eventoEncontrado: boolean;
  permiteInscricaoInfantil: boolean;
  tiposInscricao: string[] = ["Infantil", "Participante e/ou trabalhador"];
  tipoInscricaoEscolhida: string;

  constructor(private rotaAtual: ActivatedRoute, public coordenacao: CoordenacaoCentral,
    private wsEventos: WsEventos, private navegadorUrl: Router) { }

  ngOnInit(): void {

    this.coordenacao.AutorizacoesInscricao.removerTudo();
    this.eventoEncontrado = false;
    this.nomeEvento = "";
    this.idEvento = 0;
    let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

    this.rotaAtual.params
      .subscribe(
        (parametrosUrl) => {
          this.idEvento = parametrosUrl["idevento"];

          this.wsEventos.obter(this.idEvento)
            .subscribe(
              (dadosEvento) => {

                if (dadosEvento != null) {
                  this.nomeEvento = dadosEvento.Nome;
                  this.eventoEncontrado = true;
                  this.permiteInscricaoInfantil = dadosEvento.PermiteInscricaoInfantil;

                  this.tiposInscricao[0] = this.tiposInscricao[0] + " (menores de " + dadosEvento.IdadeMinima + ")";
                }

                dlg.close();
              },
              (erro) => {
                dlg.close();
                this.coordenacao.ProcessamentoErro.processar(erro);
              }
            );
        }
      );
  }

  public clicarContinuar(): void {

    if (this.permiteInscricaoInfantil && (this.tipoInscricaoEscolhida == null || this.tipoInscricaoEscolhida.trim().length == 0))
      this.coordenacao.Alertas.alertarAtencao("Você não escolheu o tipo de inscrição que deseja fazer", "Escolha um tipo de inscrição para podermos continuar!!");
    else {
      if (!this.permiteInscricaoInfantil || (this.permiteInscricaoInfantil && this.tipoInscricaoEscolhida == this.tiposInscricao[1]))
        this.navegadorUrl.navigate(['criar-inscricao/' + this.idEvento]);
      else
        this.navegadorUrl.navigate(['criar-inscricao-infantil/' + this.idEvento]);
    }        
  }
}
