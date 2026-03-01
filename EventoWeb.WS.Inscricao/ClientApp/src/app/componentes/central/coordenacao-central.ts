import { Injectable } from "@angular/core";
import { ServicoProcessamentoErros } from "./servico-processamento-erros";
import { Alertas } from "../alertas-dlg/alertas";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";

@Injectable()
export class CoordenacaoCentral {

    private mProcessamentoErro: ServicoProcessamentoErros;
    private mAlertas: Alertas;
    private mFormatacoes: Formatacoes;
    private mAutorizacoesInscricao: AutorizacoesInscricao;
    private mSexos: string[] = ["Masculino", "Feminino"];
    private mEstadosFederacao: string[] = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];
    private mTiposInscricao: string[] = ['Participante', 'Participante/Trabalhador', 'Trabalhador'];


    public get ProcessamentoErro(): ServicoProcessamentoErros { return this.mProcessamentoErro; }
    public get Alertas(): Alertas { return this.mAlertas; }
    public get Formatacoes(): Formatacoes { return this.mFormatacoes; }
    public get AutorizacoesInscricao(): AutorizacoesInscricao { return this.mAutorizacoesInscricao; }
    public get Sexos(): string[] { return this.mSexos; }
    public get EstadosFederacao(): string[] { return this.mEstadosFederacao; }
    public get TiposInscricao(): string[] { return this.mTiposInscricao; }

    constructor(private dialogo: MatDialog, private roteador: Router) {

        this.mAlertas = new Alertas(dialogo);
        this.mProcessamentoErro = new ServicoProcessamentoErros(this.mAlertas, roteador);
        this.mFormatacoes = new Formatacoes();
        this.mAutorizacoesInscricao = new AutorizacoesInscricao();
    }
}

export class Formatacoes {

    public get FormatoDataHora(): string {
        return "dd/MM/yyyy HH:mm";
    }

    public get FormatoData(): string {
        return "dd/MM/yyyy";
    }

    public get FormatoHora(): string {
        return "HH:mm";
    }

    public get FormatoDiaMesJuntos(): string {
        return "dd \\de MMMM \\de yyyy";
    }

    public get FormatoNumerico15por4(): string {
        return "###,###,###,###,##0.0000";
    }

    public get FormatoNumerico15por2(): string {
        return "###,###,###,###,##0.00";
    }

    public get FormatoPrecisaoCasasDecimais(): string {
        return "0.################";
    }

    public get FormatoInteiro6Casas(): string {
        return "000000";
    }
}

export class AutorizacoesInscricao {

    private NOME_STORANGE: string = "autorizacoes_inscricoes";
    private autorizacoes: DadosAutorizacao[];

    constructor() {
        let jsonString = localStorage.getItem(this.NOME_STORANGE);
        if (jsonString != null && jsonString.trim().length > 0) {
            this.autorizacoes = JSON.parse(jsonString);
        }
        else
            this.autorizacoes = [];
    }

    public adicionar(idInscricao: number, autorizacao: string): void {
        if (this.autorizacoes.findIndex(x => x.IdInscricao == idInscricao) == -1) {
            this.autorizacoes.push({ Autorizacao: autorizacao, IdInscricao: idInscricao });
            localStorage.setItem(this.NOME_STORANGE, JSON.stringify(this.autorizacoes));
        }
        else
            throw new Error("Já existe uma autenticação para essa inscrição.");
    }

    public remover(idInscricao: number): void {
        if (this.autorizacoes.findIndex(x => x.IdInscricao == idInscricao) != -1) {
            this.autorizacoes = this.autorizacoes.filter(x => x.IdInscricao != idInscricao);
            localStorage.setItem(this.NOME_STORANGE, JSON.stringify(this.autorizacoes));
        }
    }

    public obterAutorizacao(idInscricao: number): string {
        let autorizacao = this.autorizacoes.find(x => x.IdInscricao == idInscricao);
        if (autorizacao == null)
            return null;
        else
            return autorizacao.Autorizacao;
    }

    public removerTudo(): void {
        this.autorizacoes = [];
        localStorage.setItem(this.NOME_STORANGE, JSON.stringify(this.autorizacoes));
    }
}

class DadosAutorizacao {
    public IdInscricao: number;
    public Autorizacao: string;
}
