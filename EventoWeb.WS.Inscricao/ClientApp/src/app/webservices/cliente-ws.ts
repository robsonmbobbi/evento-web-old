import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

@Injectable()
export class ClienteWs {

    public URLWs: string;
    public TokenAutorizacao: string;

    constructor(public http: HttpClient) { }

    public executarGet<T>(parametros: string): Observable<T> {
        let opRequisicao = this.gerarCabecalho();

        return this.http
            .get<T>(this.URLWs +
                this.gerarParametros(parametros), { headers: opRequisicao })
            .pipe(catchError(erro => this.ProcessarErro(erro)));
    }

    public executarPost(parametros: string, dados: any): any {
        let opRequisicao = this.gerarCabecalho();

        return this.http
            .post(this.URLWs + this.gerarParametros(parametros),
                dados,
                { headers: opRequisicao })
            .pipe(catchError(erro => this.ProcessarErro(erro)));
    }

    public executarPut(parametros: string, dados: any): any {
        let opRequisicao = this.gerarCabecalho();

        return this.http
            .put(this.URLWs + this.gerarParametros(parametros),
                dados, { headers: opRequisicao })
            .pipe(catchError(erro => this.ProcessarErro(erro)));
    }

    public executarPutBlob(parametros: string, dados: any): any {
        let opRequisicao = this.gerarCabecalho();

        return this.http
            .put(this.URLWs + this.gerarParametros(parametros),
                dados,
                { headers: opRequisicao, responseType: 'blob' })
            .pipe(catchError(erro => this.ProcessarErro(erro)));
    }

    public executarDelete(parametros: string) {
        let opRequisicao = this.gerarCabecalho();

        return this.http
            .delete(this.URLWs + this.gerarParametros(parametros),
                { headers: opRequisicao })
            //.map(this.ExtrairDados)
            .pipe(catchError(erro => this.ProcessarErro(erro)));
    }

    protected gerarParametros(parametros: string): string {
        return (parametros != null && parametros.length > 0 ? parametros : '');
    }

    protected gerarCabecalho(): HttpHeaders {

        let opRequisicao = new HttpHeaders();
        opRequisicao = opRequisicao.append('Content-Type', 'application/json');
        opRequisicao = opRequisicao.append('Accept', 'application/json');

        if (this.TokenAutorizacao != null && this.TokenAutorizacao.length > 0)
            opRequisicao = opRequisicao.append('Authorization', 'Bearer ' + this.TokenAutorizacao);

        return opRequisicao;
    }

    protected ProcessarErro(erro: HttpErrorResponse): Observable<never> {

        if (erro.error instanceof Blob && erro.error.type === "application/json") {
            let blobComoTextoNotificacao = new Observable<string>(notificador => {
                const leitorArquivo = new FileReader();
                leitorArquivo.onload = (e) => {
                    let responseText = (<any>e.target).result;

                    notificador.next(responseText);
                    notificador.complete();
                }
                const errMsg = leitorArquivo.readAsText(erro.error, 'utf-8');
            });

            return blobComoTextoNotificacao
                .pipe(switchMap(errMsgJsonComoTexto => {
                    return throwError(JSON.parse(errMsgJsonComoTexto));
                }));
        }
        else if (erro.error != null && !(erro.error instanceof Blob)) {
            return throwError(erro.error);
        }
        else
            return throwError(erro.message || "Erro no Servidor");
    }
}
