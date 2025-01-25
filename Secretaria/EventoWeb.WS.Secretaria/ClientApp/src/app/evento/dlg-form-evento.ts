import { Component, Injectable, OnInit, Inject } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

import { DTOEventoCompleto, EnumModeloDivisaoSalasEstudo, EnumPublicoEvangelizacao, Periodo, DTOEvento, EnumModeloDivisaoOficinas } from "./objetos";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { WebServiceEventos } from "../webservices/webservice-eventos";
import { OperacoesImagem } from "../util/util-imagem";
import { Observable } from 'rxjs';

export abstract class ADlgFormEvento {

  titulo!: string;

  modelosDivisaoSala: string[] = ["Por Idade e Cidade", "Por Ordem de Escolha definida na Inscrição"];
  publicosEvangelizacao: string[] = ["Todos", "Apenas para Trabalhadores e Participantes/Trabalhadores"];
  modelosDivisaoOficina: string[] = ["Por Ordem de Escolha definida na Inscrição", "Por Idade e Cidade"];

  evento!: DTOEventoCompleto;

  constructor(protected alertas: Alertas) { }

  public set temSalaEstudo(valor: boolean) {
    if (valor)
      this.modeloDivisaoSalaEstudo = this.modelosDivisaoSala[EnumModeloDivisaoSalasEstudo.PorIdadeCidade];
    else
      this.modeloDivisaoSalaEstudo = "";
  }

  public get temSalaEstudo(): boolean {
    return this.evento.ConfiguracaoSalaEstudo != null;
  }

  public set modeloDivisaoSalaEstudo(valor: string) {
    let indice = this.modelosDivisaoSala.findIndex(x => x == valor);
    if (indice != -1)
      this.evento.ConfiguracaoSalaEstudo = indice;
    else
      this.evento.ConfiguracaoSalaEstudo = null;
  }

  public get modeloDivisaoSalaEstudo(): string | null {
    return (this.evento.ConfiguracaoSalaEstudo != null ? this.modelosDivisaoSala[this.evento.ConfiguracaoSalaEstudo] : null);
  }

  public set temEvangelizacao(valor: boolean) {
    if (valor)
      this.publicoEvangelizacao = this.publicosEvangelizacao[EnumPublicoEvangelizacao.Todos];
    else
      this.publicoEvangelizacao = "";
  }

  public get temEvangelizacao(): boolean {
    return this.evento.ConfiguracaoEvangelizacao != null;
  }

  public set publicoEvangelizacao(valor: string) {
    let indice = this.publicosEvangelizacao.findIndex(x => x == valor);
    if (indice != -1)
      this.evento.ConfiguracaoEvangelizacao = indice;
    else
      this.evento.ConfiguracaoEvangelizacao = null;
  }

  public get publicoEvangelizacao(): string | null {
    return (this.evento.ConfiguracaoEvangelizacao != null ? this.publicosEvangelizacao[this.evento.ConfiguracaoEvangelizacao] : null);
  }

  public set temSarau(valor: boolean) {
    if (valor)
      this.evento.ConfiguracaoTempoSarauMin = 0;
    else
      this.evento.ConfiguracaoTempoSarauMin = null;
  }

  public get temSarau(): boolean {
    return this.evento.ConfiguracaoTempoSarauMin != null;
  }

  public set temOficinas(valor: boolean) {
    if (valor)
      this.modeloDivisaoOficina = this.modelosDivisaoOficina[EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao];
    else
      this.modeloDivisaoOficina = "";
  }

  public get temOficinas(): boolean {
    return this.evento.ConfiguracaoOficinas != null;
  }

  public set modeloDivisaoOficina(valor: string) {
    let indice = this.modelosDivisaoOficina.findIndex(x => x == valor);
    if (indice != -1)
      this.evento.ConfiguracaoOficinas = indice;
    else
      this.evento.ConfiguracaoOficinas = null;
  }

  public get modeloDivisaoOficina(): string | null {
    return (this.evento.ConfiguracaoOficinas != null ? this.modelosDivisaoOficina[this.evento.ConfiguracaoOficinas] : null);
  }

  public set temDormitorios(valor: Boolean) {
    this.evento.TemDormitorios = valor;
    if (this.evento.TemDormitorios)
      this.evento.PermiteEscolhaDormirEvento = false;
    else
      this.evento.PermiteEscolhaDormirEvento = null;
  }

  public get temDormitorios(): Boolean {
    return this.evento.TemDormitorios;
  }

  processarArquivoEscolhido(arquivoImagem: any): void {
    let reader = new FileReader();
    reader.addEventListener('load', (event: any) => {
      this.evento.Logotipo = event.target.result.substring(event.target.result.indexOf(",") + 1);
    });
    reader.readAsDataURL(arquivoImagem.files[0]);
  }

  obterImagem(): string {
    return OperacoesImagem.obterImagemOuSemImagem(this.evento.Logotipo);
  }

  clicarLimparImagem(): void {
    this.evento.Logotipo = null;
  }

  abstract clicarSair(): void;

  clicarSalvar(): void {
    if (this.evento.Nome == null || this.evento.Nome.trim().length == 0)
      this.alertas.alertarAtencao("Você não informou o nome do evento", "");
    else if (this.evento.PeriodoRealizacao.DataInicial == null)
      this.alertas.alertarAtencao("Você não informou a data que inicia o evento", "");
    else if (this.evento.PeriodoRealizacao.DataFinal == null)
      this.alertas.alertarAtencao("Você não informou a data que termina o evento", "");
    else if (this.evento.PeriodoInscricao.DataInicial == null)
      this.alertas.alertarAtencao("Você não informou a data que inicia as inscrições", "");
    else if (this.evento.PeriodoInscricao.DataFinal == null)
      this.alertas.alertarAtencao("Você não informou a data que termina as inscrições", "");
    else if (this.evento.ConfiguracaoEvangelizacao != null && this.evento.ConfiguracaoEvangelizacao == null)
      this.alertas.alertarAtencao("Você precisa dizer qual o público da evangelização", "");
    else if (this.evento.ConfiguracaoSalaEstudo != null && this.evento.ConfiguracaoSalaEstudo == null)
      this.alertas.alertarAtencao("Você precisa dizer qual o modelo de divisão da sala de estudo", "");
    else if (this.evento.ConfiguracaoTempoSarauMin != null && (this.evento.ConfiguracaoTempoSarauMin <= 0))
      this.alertas.alertarAtencao("Você precisa dizer o tempo do duração. Esse tempo deve ser maior que zero", "");
    else if (this.evento.IdadeMinima == null || this.evento.IdadeMinima <= 0)
      this.alertas.alertarAtencao("Você precisa dizer a idade mínima do participante em anos", "Esse idade deve ser maior que zero");
    else if (this.evento.ValorInscricaoAdulto == null || this.evento.ValorInscricaoAdulto < 0)
      this.alertas.alertarAtencao("Você precisa dizer o valor da inscrição do participante", "Esse valor não pode ser menor que zero");
    else if (this.evento.ConfiguracaoEvangelizacao != null && (this.evento.ValorInscricaoCrianca == null || this.evento.ValorInscricaoCrianca < 0))
      this.alertas.alertarAtencao("Você precisa dizer o valor da inscrição infantil", "Esse valor não pode ser menor que zero");
    else {
      this.salvarEvento(this.evento);
    }
  }

  protected abstract salvarEvento(evento: DTOEventoCompleto): void;
}

@Component({
  selector: "dlg-form-evento-inclusao",
  templateUrl: "./dlg-form-evento.html",
  styleUrls: ["./dlg-form-evento.scss"]
})
export class DlgFormEventoInclusao extends ADlgFormEvento implements OnInit {

  constructor(
    private dialogRef: MatDialogRef<DlgFormEventoInclusao>, protected override alertas: Alertas,
    private wsEventos: WebServiceEventos) {
    super(alertas);
  }

  ngOnInit(): void {
    this.evento = new DTOEventoCompleto();
    this.evento.PeriodoInscricao = new Periodo();
    this.evento.PeriodoRealizacao = new Periodo();
    this.evento.DataRegistro = new Date();
    this.evento.TemDepartamentalizacao = false;
    this.evento.TemDormitorios = false;
    this.evento.PermiteEscolhaDormirEvento = null;

    this.titulo = "Inclusão de Evento";
  }

  clicarSair(): void {
    this.dialogRef.close(null);
  }

  protected salvarEvento(evento: DTOEventoCompleto): void {
    let dlg = this.alertas.alertarProcessamento("Salvando evento, aguarde...");
    this.wsEventos
      .incluir(this.evento)
      .subscribe(
        (eventoIncluido) => {
          dlg.close();
          this.dialogRef.close(eventoIncluido);
        },
        (retornoErro) => {
          dlg.close();
          this.alertas.alertarErro(retornoErro);
        });
  }
}

@Component({
  selector: "dlg-form-evento-alteracao",
  templateUrl: "./dlg-form-evento.html",
  styleUrls: ["./dlg-form-evento.scss"]
})
export class DlgFormEventoAlteracao extends ADlgFormEvento implements OnInit {

  private idEvento: any;

  constructor(
    private dialogRef: MatDialogRef<DlgFormEventoAlteracao>, protected override alertas: Alertas,
    private wsEventos: WebServiceEventos, @Inject(MAT_DIALOG_DATA) public data: any) {

    super(alertas);

    this.idEvento = data.idEvento;
  }

  ngOnInit(): void {
    let dlg = this.alertas.alertarProcessamento("Buscando evento, aguarde...");

    this.wsEventos.obterId(this.idEvento)
      .subscribe(
        evento => {
          dlg.close();
          if (evento != null)
            this.evento = evento;
          else {
            this.alertas.alertarAtencao("Evento não encontrado com este id", "");
            this.dialogRef.close();
          }
        },
        erro => {
          dlg.close();
          this.alertas.alertarErro(erro);
        }
      );

    this.titulo = "Alteração de Evento";
  }

  clicarSair(): void {
    this.dialogRef.close(null);
  }

  protected salvarEvento(evento: DTOEventoCompleto): void {
    let dlg = this.alertas.alertarProcessamento("Salvando evento, aguarde...");
    this.wsEventos
      .atualizar(this.idEvento, this.evento)
      .subscribe(
        () => {
          dlg.close();
          this.dialogRef.close(this.evento);
        },
        (retornoErro) => {
          dlg.close();
          this.alertas.alertarErro(retornoErro);
        });
  }
}

@Injectable()
export class ServicoDlgFormEvento {

  constructor(public dialog: MatDialog) { }

  apresentarDlgInclusao(): Observable<DTOEventoCompleto> {
    const dialogRef = this.dialog.open(DlgFormEventoInclusao, {
      width: '100vw',
      height: '100vh',
      maxWidth: '100vw',
      maxHeight: '100vh'
    });

    return dialogRef.afterClosed();
  }

  apresentarDlgAlteracao(idEvento: number): Observable<DTOEventoCompleto> {
    const dialogRef = this.dialog.open(DlgFormEventoAlteracao, {
      width: '100vw',
      height: '100vh',
      maxWidth: '100vw',
      maxHeight: '100vh',
      data: {
        idEvento: idEvento
      }
    });

    return dialogRef.afterClosed();
  }
}
