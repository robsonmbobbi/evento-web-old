import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DTOComprovantePagamento, DTOPagamento, EnumPagamento, EnumTipoArquivoBinario } from '../objetos';
import { Observable, forkJoin } from 'rxjs';
import { DialogoContrato } from '../../contrato/dlg-contrato';

@Component({
  selector: 'comp-pagamento',
  templateUrl: './comp-pagamento.html'
})
export class ComponentePagamento {

  private mValor: DTOPagamento;
  private mArquivosBin: ComprovanteComArquivo[] = [];
  private mExtensaoArquivo: DadosExtensaoArquivo[] = [
    { Tipo: ".PDF", Mime: "application/pdf" },
    { Tipo: ".PNG", Mime: "image/png" },
    { Tipo: ".JPG", Mime: "image/jpg" },
  ];

  @Input()
  desabilitar: boolean;

  @Input()
  idEvento: number;

  @Input()
  set valor(param: DTOPagamento) {

    if (param != null) {
      if (this.mValor != param) {
        this.mValor = param;
        this.mArquivosBin = [];

        if (this.mValor.Comprovantes != null && this.mValor.Comprovantes.length > 0) {
          let indice = 1;
          for (let comprovante of this.mValor.Comprovantes) {
              this.mArquivosBin.push(
                  {
                      Comprovante: comprovante,
                      Arquivo: new File(
                        [this.dataURItoBlob(
                            comprovante.Base64.substring(comprovante.Base64.indexOf(",") + 1),
                            this.mExtensaoArquivo[comprovante.TipoArquivo].Mime
                          )],
                        indice.toString() + '.' + this.mExtensaoArquivo[comprovante.TipoArquivo].Tipo,
                        { type: this.mExtensaoArquivo[comprovante.TipoArquivo].Mime }
                      )
                  }
              );
              indice++;
          }
        }
      }
    }
    else {
      this.mValor = new DTOPagamento();
      this.mValor.Forma = EnumPagamento.Comprovante;
      this.mValor.Comprovantes = [];
      this.mArquivosBin = [];
    }
  }
  @Output()
  valorChange: EventEmitter<DTOPagamento> = new EventEmitter<DTOPagamento>();

  @Input()
  valorInscricao: number;

  opcoes: string[] = ["Enviar Comprovante", "Comprovante esta em outra inscrição", "Outros"];

  constructor(private dlgContrato: DialogoContrato) { }

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

      this.mValor.Comprovantes = [];
      this.mArquivosBin = [];

      this.valorChange.emit(this.mValor);
    }
  }

  get arquivosComprovantes(): ComprovanteComArquivo[] {

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

  public onFileSelected(event: any) {

    if (event.target.files.length > 0) {
        let arquivosValidos = event.target.files;

        let observadores: Observable<string>[] = [];

        for (let arquivo of arquivosValidos) {
            observadores.push(this.readFileAsDataURL(arquivo));
        }

        forkJoin(observadores)
            .subscribe(x => this.valorChange.emit(this.mValor));
    }
  }

  private readFileAsDataURL(file): Observable<string> {

    let result_base64 = new Observable<string>((resolve) => {
        let fileReader = new FileReader();
        fileReader.onload = (e: any) => {
            let comprovante = new DTOComprovantePagamento();
            comprovante.Base64 = e.target.result;
            if (file.type == "image/jpeg" || file.type == "image/jpg") {
                comprovante.TipoArquivo = EnumTipoArquivoBinario.ImagemJPEG;
            } else if (file.type == "application/pdf") {
                comprovante.TipoArquivo = EnumTipoArquivoBinario.PDF;
            } else {
                comprovante.TipoArquivo = EnumTipoArquivoBinario.ImagemPNG; // Default or other type
            }
            this.mValor.Comprovantes.push(comprovante);
            this.mArquivosBin.push({ Comprovante: comprovante, Arquivo: file });

            resolve.next(<string>e.target.result);
            resolve.complete();
        };

        fileReader.readAsDataURL(file);
    });

    return result_base64;
  }

  private dataURItoBlob(dataURI: string, mimeType: string): Blob {
    const byteString = window.atob(dataURI);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
        int8Array[i] = byteString.charCodeAt(i);
    }
    const blob = new Blob([int8Array], { type: mimeType });
    return blob;
}

  public abrirComprovante(arquivo: File): void {
    let url = URL.createObjectURL(arquivo);
    window.open(url, '_blank');
  }

  public clicarExcluirComprovante(comprovante: ComprovanteComArquivo): void {
      let indice = this.mArquivosBin.indexOf(comprovante);
      this.mArquivosBin.splice(indice, 1);
      this.mValor.Comprovantes.splice(indice, 1);
      this.valorChange.emit(this.mValor);
  }

  public clicarAbrirRegulamento(): void {
    this.dlgContrato.apresentarDlgFormDialogoInclusao(this.idEvento);
  }
}

export class DadosExtensaoArquivo {
  Tipo: string;
  Mime: string;
}

export class ComprovanteComArquivo {
  Comprovante: DTOComprovantePagamento;
  Arquivo: File;
}
