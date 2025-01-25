import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DTOSalaEstudo, EnumModeloDivisaoSalasEstudo } from '../../principal/objetos';
import { EnumApresentacaoAtividades, DTOInscricaoSalaEstudo } from '../objetos';

@Component({
  selector: 'comp-salas',
  templateUrl: './comp-salas.html'
})
export class ComponenteSalas {

  apresentacao: EnumApresentacaoAtividades;
  opcoes: string[] = ["Participante", "Coordenador"];
  _opcaoEscolhida: string;

  @Input()
  desabilitar: boolean;

  @Input()
  salas: DTOSalaEstudo[] = [];

  @Input()
  configuracaoSala: EnumModeloDivisaoSalasEstudo;

  @Input()
  set escolhido(param: DTOInscricaoSalaEstudo) {
    this.mCoordenador = null;
    this.mParticipante = null;

    if (param != null) {
      if (param.Coordenador != null) {
        this._opcaoEscolhida = this.opcoes[1];
        this.mCoordenador = param.Coordenador;
      }
      else {
        this._opcaoEscolhida = this.opcoes[0];
        this.mParticipante = param.EscolhidasParticipante;
      }
    }
  }

  @Output()
  escolhidoChange: EventEmitter<DTOInscricaoSalaEstudo> = new EventEmitter<DTOInscricaoSalaEstudo>();

  @Input()
  set forma(valor: EnumApresentacaoAtividades) {

    this.apresentacao = valor;
    if (valor == EnumApresentacaoAtividades.ApenasParticipante)
      this.opcaoEscolhida = this.opcoes[0];
  }

  set opcaoEscolhida(valor: string) {
    if (valor != this._opcaoEscolhida) {
      this._opcaoEscolhida = valor;

      if (valor == this.opcoes[0]) {
        if (this.configuracaoSala == EnumModeloDivisaoSalasEstudo.PorIdadeCidade)
          this.participante = []
        else
          this.participante = null;
      }
      else
        this.coordenador = null;
    }
  }

  get opcaoEscolhida(): string {
    return this._opcaoEscolhida;
  }

  private mParticipante: DTOSalaEstudo[];

  set participante(valor: DTOSalaEstudo[]) {

    if (!(valor == null && this.mParticipante == null)) {
      this.mParticipante = valor;
      if (valor == null)
        this.escolhidoChange.emit(null);
      else
        this.escolhidoChange.emit({
          Coordenador: null,
          EscolhidasParticipante: valor
        });
    }
  }

  get participante(): DTOSalaEstudo[] {
    return this.mParticipante;
  }

  private mCoordenador: DTOSalaEstudo;

  set coordenador(valor: DTOSalaEstudo) {
    this.mCoordenador = valor;
    if (valor == null)
      this.escolhidoChange.emit(null);
    else
      this.escolhidoChange.emit({
        Coordenador: valor,
        EscolhidasParticipante: null
      });
  }

  get coordenador(): DTOSalaEstudo {
    return this.mCoordenador;
  }
}

@Component({
  selector: 'comp-sala-participante-com-escolha',
  templateUrl: './comp-sala-participante-com-escolha.html',
  styles: ['.borda_total { border: solid 1px; margin: 5px }']
})
export class ComponenteSalasParticipanteComEscolha {

  private salasRecebidas: DTOSalaEstudo[];

  @Input()
  desabilitar: boolean;

  @Input()
  set salas(valor: DTOSalaEstudo[]) {
    this.salasRecebidas = valor;
    if (valor != null) {
      if (this.salasEscolhidas != null && this.salasEscolhidas.length > 0)
        this.salasDisponiveis = valor.filter(x => this.salasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.salasDisponiveis = valor;
    }
    else
      this.salasDisponiveis = [];
  }

  @Input()
  set valor(salas: DTOSalaEstudo[]) {

    if (salas != null) {
      this.salasEscolhidas = salas;

      if (this.salasRecebidas != null && this.salasRecebidas.length > 0)
        this.salasDisponiveis = this.salasRecebidas.filter(x => this.salasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
      else
        this.salasDisponiveis = [];
    }
    else
      this.salasEscolhidas = [];
  }

  @Output()
  valorChange: EventEmitter<DTOSalaEstudo[]> = new EventEmitter<DTOSalaEstudo[]>();

  salasEscolhidas: DTOSalaEstudo[] = [];
  salasDisponiveis: DTOSalaEstudo[] = [];
  salasDisponiveisSelecionadas: DTOSalaEstudo[] = [];
  salasEscolhidasSelecionadas: DTOSalaEstudo[] = [];
  situacaoSelecaoSalasEscolhidas: EnumSituacaoSelecaoItens = EnumSituacaoSelecaoItens.NenhumItem;

  clicarIncluir(): void {
    for (let item of this.salasDisponiveisSelecionadas) {
      this.salasEscolhidas.push(item);
    }

    this.salasDisponiveis = this.salasDisponiveis.filter(x => this.salasDisponiveisSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.salasDisponiveisSelecionadas = [];

    this.valorChange.emit(this.salasEscolhidas);
  }

  clicarRemover(): void {
    for (let item of this.salasEscolhidasSelecionadas)
      this.salasDisponiveis.push(item);

    this.salasEscolhidas = this.salasEscolhidas.filter(x => this.salasEscolhidasSelecionadas.findIndex(y => y.Id == x.Id) == -1);
    this.salasEscolhidasSelecionadas = [];

    this.valorChange.emit(this.salasEscolhidas);
  }

  processarMudancasSelecaoSalasEscolhidas(evento: any): void {

    if (this.salasEscolhidasSelecionadas.length == 0)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.NenhumItem;
    else if (this.salasEscolhidasSelecionadas.length == 1) {
      this.atualizarSituacaoSelecaoItem();
    }
    else
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.MaisDeUmItem;
  }

  clicarSubir(): void {
    let posicaoItemSubirah = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.salasEscolhidas[posicaoItemSubirah - 1];
    let valorPosicaoItemSubirah = this.salasEscolhidas[posicaoItemSubirah];

    this.salasEscolhidas[posicaoItemSubirah - 1] = valorPosicaoItemSubirah;
    this.salasEscolhidas[posicaoItemSubirah] = valorPosicaoIrah;

    this.valorChange.emit(this.salasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  clicarDescer(): void {
    let posicaoItemDescerah = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    let valorPosicaoIrah = this.salasEscolhidas[posicaoItemDescerah + 1];
    let valorPosicaoItemDescerah = this.salasEscolhidas[posicaoItemDescerah];

    this.salasEscolhidas[posicaoItemDescerah + 1] = valorPosicaoItemDescerah;
    this.salasEscolhidas[posicaoItemDescerah] = valorPosicaoIrah;

    this.valorChange.emit(this.salasEscolhidas);

    this.atualizarSituacaoSelecaoItem();
  }

  private atualizarSituacaoSelecaoItem(): void {
    let posicao = this.salasEscolhidas.findIndex(x => x.Id == this.salasEscolhidasSelecionadas[0].Id);
    if (posicao == 0)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.Primeiro;
    else if (posicao == this.salasEscolhidas.length - 1)
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.Ultimo;
    else
      this.situacaoSelecaoSalasEscolhidas = EnumSituacaoSelecaoItens.EntrePrimeiroUltimo;
  }
}

enum EnumSituacaoSelecaoItens {
  Primeiro,
  Ultimo,
  EntrePrimeiroUltimo,
  MaisDeUmItem,
  NenhumItem
}

@Component({
  selector: 'comp-sala-participante-sem-escolha',
  templateUrl: './comp-sala-participante-sem-escolha.html'
})
export class ComponenteSalasParticipanteSemEscolha {
}

@Component({
  selector: 'comp-sala-coordenador',
  templateUrl: './comp-sala-coordenador.html'
})
export class ComponenteSalaCoordenador {

  @Input()
  desabilitar: boolean;

  @Input()
  salas: DTOSalaEstudo[] = [];

  @Input()
  set valor(sala: DTOSalaEstudo) {
    if (sala != null)
      this.mValor = this.salas.find(x => x.Id == sala.Id);
  }

  @Output()
  valorChange: EventEmitter<DTOSalaEstudo> = new EventEmitter<DTOSalaEstudo>();

  private mValor: DTOSalaEstudo;

  set salaCoordena(valor: DTOSalaEstudo) {
    this.mValor = valor;
    this.valorChange.emit(this.mValor);
  }

  get salaCoordena(): DTOSalaEstudo {
    return this.mValor;
  }
}
