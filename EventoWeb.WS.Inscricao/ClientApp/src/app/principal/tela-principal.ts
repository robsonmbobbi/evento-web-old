import { Component, OnInit } from '@angular/core';
import { DTOEventoListagem } from './objetos';
import { WsEventos } from '../webservices/wsEventos';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';

@Component({
    selector: 'tela-principal',
    templateUrl: './tela-principal.html',
    styleUrls: ['./tela-principal.scss']
})
export class TelaPrincipal implements OnInit {

    eventos: DTOEventoListagem[];

    constructor(private wsEventos: WsEventos, private coordenacao: CoordenacaoCentral) { }

    ngOnInit(): void {

        this.coordenacao.AutorizacoesInscricao.removerTudo();

        this.eventos = [];

        let dlg = this.coordenacao.Alertas.alertarProcessamento("Carregando eventos disponíveis...");

        this.wsEventos.listarEventosDisponiveisInscricao()
            .subscribe(
                (eventosRetornados) => {
                    this.eventos = eventosRetornados;
                    dlg.close();
                },
                (excecao) => {
                    dlg.close();
                    this.coordenacao.ProcessamentoErro.processar(excecao);
                }
            );
    }

    public obterImagem(evento: DTOEventoListagem): string {
        if (evento.Logotipo == null || evento.Logotipo.trim().length == 0)
            return 'assets/semimagem.jpg';
        else
            return 'data:image/jpeg;base64,' + evento.Logotipo;
    }
}
