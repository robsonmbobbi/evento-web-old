import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DTOPagamento, EnumPagamento } from '../objetos';
import { Observable, forkJoin } from 'rxjs';

@Component({
    selector: 'comp-pagamento',
    templateUrl: './comp-pagamento.html'
})
export class ComponentePagamento {

    private mValor: DTOPagamento;
    private mArquivosBin: File[] = [];

    @Input()
    desabilitar: boolean;

    @Input()
    set valor(param: DTOPagamento) {

        if (param != null) {
            if (this.mValor != param) {
                this.mValor = param;
                this.mArquivosBin = [];

                if (this.mValor.ComprovantesBase64 != null && this.mValor.ComprovantesBase64.length > 0) {
                    let indice = 1;
                    for (let arquivo64 of this.mValor.ComprovantesBase64) {
                        this.mArquivosBin.push(new File([this.dataURItoBlob(arquivo64.substring(arquivo64.indexOf(",") + 1))], indice.toString() + ".jpeg", { type: 'image/jpeg' }));
                        indice++;
                    }
                }
            }
        }
        else {
            this.mValor = new DTOPagamento();
            this.mValor.Forma = EnumPagamento.Comprovante;
            this.mValor.ComprovantesBase64 = [];
            this.mArquivosBin = [];
        }
    }
    @Output()
    valorChange: EventEmitter<DTOPagamento> = new EventEmitter<DTOPagamento>();

    @Input()
    valorInscricao: number;

    @Input()
    valorInscCrianca: number;

    @Input()
    totalCriancas: number;

    opcoes: string[] = ["Enviar Comprovante", "Comprovante esta em outra inscrição", "Outros"];

    get opcaoEscolhida(): string {

        switch (this.mValor.Forma) {
            case EnumPagamento.Comprovante: return this.opcoes[0];
            case EnumPagamento.ComprovanteOutraInscricao: return this.opcoes[1];
            case EnumPagamento.Outro: return this.opcoes[2];
        }
    }

    set opcaoEscolhida(valor: string) {
        if (valor != this.opcaoEscolhida) {
            if (valor == this.opcoes[0])
                this.mValor.Forma = EnumPagamento.Comprovante;
            else if (valor == this.opcoes[1])
                this.mValor.Forma = EnumPagamento.ComprovanteOutraInscricao;
            else if (valor == this.opcoes[2])
                this.mValor.Forma = EnumPagamento.Outro;

            this.mValor.ComprovantesBase64 = [];
            this.mArquivosBin = [];

            this.valorChange.emit(this.mValor);
        }
    }

    set comprovantes(param: any[]) {

        if (param != null) {
            let arquivosValidos = param.filter(x => x.type == "image/jpeg" || x.type == "image/jpg" || x.type == "image/pdf");
            if (arquivosValidos.length != this.mArquivosBin.length) {
                if (arquivosValidos.length == 0) {
                    this.mValor.ComprovantesBase64 = [];
                    this.mArquivosBin = [];

                    this.valorChange.emit(this.mValor);
                }
                else {

                    this.mArquivosBin = arquivosValidos;
                    this.mValor.ComprovantesBase64 = [];

                    let observadores: Observable<string>[] = [];
                    for (let arquivo of arquivosValidos) {
                        observadores.push(this.readFileAsDataURL(arquivo));
                    }

                    forkJoin(observadores)
                        .subscribe(x => this.valorChange.emit(this.mValor));
                }
            }
        }
        else if (param == null && this.mArquivosBin.length != 0) {
            this.mValor.ComprovantesBase64 = [];
            this.mArquivosBin = [];

            this.valorChange.emit(this.mValor);
        }
    }

    private readFileAsDataURL(file): Observable<string> {

        let result_base64 = new Observable<string>((resolve) => {
            let fileReader = new FileReader();
            fileReader.onload = (e: any) => {
                this.mValor.ComprovantesBase64.push(<string>e.target.result);
                resolve.next(<string>e.target.result);
                resolve.complete();
            };

            fileReader.readAsDataURL(file);
        });

        return result_base64;
    }

    get comprovantes(): any[] {

        return this.mArquivosBin;
    }

    set observacoes(param: string) {

        if (this.opcaoEscolhida != this.opcoes[0] && (param == null || param.trim().length == 0)) {
            this.mValor.Observacao = "";
            this.valorChange.emit(this.mValor);
        }
        else {
            this.mValor.Observacao = param;
            this.valorChange.emit(this.mValor);
        }
    }

    get observacoes(): string {
        return this.mValor.Observacao;
    }

    private dataURItoBlob(dataURI: string): Blob {
        const byteString = window.atob(dataURI);
        const arrayBuffer = new ArrayBuffer(byteString.length);
        const int8Array = new Uint8Array(arrayBuffer);
        for (let i = 0; i < byteString.length; i++) {
            int8Array[i] = byteString.charCodeAt(i);
        }
        const blob = new Blob([int8Array], { type: 'image/jpeg' });
        return blob;
    }

    public obterImgComprovantes(): string[] {
        if (this.mValor != null && this.mValor.ComprovantesBase64 != null) {
            return this.mValor.ComprovantesBase64;
        }
        else
            return [];
    }
}
