import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { WsInscricoes } from '../webservices/wsInscricoes';
import { DTOBasicoInscricao } from './objetos';

@Component({
    selector: 'tela-codigo-inscricao',
    templateUrl: './tela-codigo-inscricao.html',
    styleUrls: ['./tela-codigo-inscricao.scss']
})
export class TelaCodigoInscricao implements OnInit {

    dadosTela: DadosTela;

    constructor(private rotaAtual: ActivatedRoute, private coordenacao: CoordenacaoCentral,
        private wsInscricoes: WsInscricoes, private navegadorUrl: Router) { }

    ngOnInit(): void {
        this.coordenacao.AutorizacoesInscricao.removerTudo();

        this.dadosTela = new DadosTela();
        this.dadosTela.codigo = "";
        this.dadosTela.dadosInscricao = new DTOBasicoInscricao();
        this.dadosTela.inscricaoEncontrada = false;

        let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando dados...");

        this.rotaAtual.params
            .subscribe(
                (parametrosUrl) => {
                    let idInscricao = parametrosUrl["idinscricao"];

                    this.wsInscricoes.obterBasicoInscricao(idInscricao)
                        .subscribe(
                            (dadosInscricao) => {
                                dlg.close();
                                if (dadosInscricao != null) {
                                    this.dadosTela.dadosInscricao = dadosInscricao;
                                    this.dadosTela.inscricaoEncontrada = true;
                                }
                            },
                            (erro) => {
                                dlg.close();
                                this.coordenacao.ProcessamentoErro.processar(erro);
                            }
                        );
                }
            );
    }

    @ViewChild("grupoValidacao")
    grupoValidacao: DxValidationGroupComponent;

    public clicarContinuar(): void {

        let resultadoValidacao = this.grupoValidacao.instance.validate();
        if (!resultadoValidacao.isValid)
            this.coordenacao.Alertas.alertarAtencao("Nâo deu para continuar!!", "Ops, acho que alguns dados informados não estão legais!");
        else {
            let dlg = this.coordenacao.Alertas.alertarProcessamento("Acessando...");

            this.wsInscricoes.validarAcessoInscricao(this.dadosTela.dadosInscricao.IdInscricao, this.dadosTela.codigo)
                .subscribe(
                    (autenticacao) => {
                        dlg.close();
                        this.coordenacao.AutorizacoesInscricao.adicionar(autenticacao.IdInscricao, autenticacao.Autorizacao);
                        this.navegadorUrl.navigate(['inscricao/' + autenticacao.IdInscricao]);
                    },
                    (erro) => {
                        dlg.close();
                        this.coordenacao.ProcessamentoErro.processar(erro);
                    }
                );
        }
    }
}

class DadosTela {
    codigo: string;
    inscricaoEncontrada: boolean;
    dadosInscricao: DTOBasicoInscricao;
}
