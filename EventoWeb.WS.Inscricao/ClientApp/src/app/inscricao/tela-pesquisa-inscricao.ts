import { Component, OnInit, ViewChild } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { Router } from '@angular/router';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { EnumResultadoEnvio } from './objetos';

@Component({
    selector: 'tela-pesquisa-inscricao',
    templateUrl: './tela-pesquisa-inscricao.html',
    styleUrls: ['./tela-pesquisa-inscricao.scss']
})
export class TelaPesquisaInscricao implements OnInit {

    identificacao: string;

    constructor(private coordenacao: CoordenacaoCentral,
        private wsInscricoes: WsInscricoes, private navegadorUrl: Router) { }

    ngOnInit(): void {
        this.coordenacao.AutorizacoesInscricao.removerTudo();
        this.identificacao = "";
    }

    @ViewChild("grupoValidacao")
    grupoValidacao: DxValidationGroupComponent;

    public clicarContinuar(): void {

        let resultadoValidacao = this.grupoValidacao.instance.validate();
        if (!resultadoValidacao.isValid)
            this.coordenacao.Alertas.alertarAtencao("Nâo deu para continuar!!", "Ops, acho que alguns dados informados não estão legais!");
        else {
            let dlg = this.coordenacao.Alertas.alertarProcessamento("Pesquisando identificação...");

            this.wsInscricoes.enviarCodigoAcesso(this.identificacao.toUpperCase())
                .subscribe(
                    (envio) => {
                        dlg.close();

                        if (envio.Resultado == EnumResultadoEnvio.InscricaoNaoEncontrada)
                            this.coordenacao.Alertas.alertarAtencao("Problema com a identificação!!", "Ops, não consegui achar nenhuma inscrição com essa identificação!");
                        else if (envio.Resultado == EnumResultadoEnvio.EventoEncerradoInscricao)
                            this.coordenacao.Alertas.alertarAtencao("Problema com a identificação!!", "Ops, você usou uma identificação de inscrição da qual o evento já foi encerrado. Não consigo acessar mais!");
                        else
                            this.navegadorUrl.navigate(['validar/' + envio.IdInscricao]);
                    },
                    (erro) => {
                        dlg.close();
                        this.coordenacao.ProcessamentoErro.processar(erro);
                    }
                );
        }
    }
}
