import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EnumApresentacaoAtividades, DTOInscricaoOficina } from '../objetos';
import { DTOOficina } from '../../oficinas/objetos';
import { EnumModeloDivisaoOficinas } from '../../evento/objetos';

@Component({
    selector: 'comp-oficinas',
    templateUrl: './comp-oficinas.html'
})
export class ComponenteOficinas {

    apresentacao: EnumApresentacaoAtividades;
    opcoes: string[] = ["Participante", "Coordenador"];
    _opcaoEscolhida: string;

    @Input()
    configuracao: EnumModeloDivisaoOficinas;

    @Input()
    desabilitar: boolean;

    @Input()
    oficinas: DTOOficina[] = [];

    @Input()
    set escolhido(param: DTOInscricaoOficina) {
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
    escolhidoChange: EventEmitter<DTOInscricaoOficina> = new EventEmitter<DTOInscricaoOficina>();

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
          if (this.configuracao == EnumModeloDivisaoOficinas.PorIdadeCidade)
            this.participante = [];
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

    private mParticipante: DTOOficina[];

  set participante(valor: DTOOficina[]) {

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

    get participante(): DTOOficina[] {
        return this.mParticipante;
    }

    private mCoordenador: DTOOficina;

    set coordenador(valor: DTOOficina) {
        this.mCoordenador = valor;
        if (valor == null)
            this.escolhidoChange.emit(null);
        else
            this.escolhidoChange.emit({
                Coordenador: valor,
                EscolhidasParticipante: null
            });
    }

    get coordenador(): DTOOficina {
        return this.mCoordenador;
    }
}

@Component({
    selector: 'comp-oficina-participante',
    templateUrl: './comp-oficina-participante.html',
    styles: ['.borda_total { border: solid 1px; margin: 5px }']
})
export class ComponenteOficinaParticipante {

    private oficinasRecebidas: DTOOficina[];

    @Input()
    desabilitar: boolean;

    @Input()
    set oficinas(valor: DTOOficina[]) {
        this.oficinasRecebidas = valor;
        if (valor != null) {
            if (this.oficinasEscolhidas != null && this.oficinasEscolhidas.length > 0)
                this.oficinasDisponiveis = valor.filter(x => this.oficinasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
            else
                this.oficinasDisponiveis = valor;
        }
        else
            this.oficinasDisponiveis = [];
    }

    @Input()
    set valor(oficinas: DTOOficina[]) {

        if (oficinas != null) {
            this.oficinasEscolhidas = oficinas;

            if (this.oficinasRecebidas != null && this.oficinasRecebidas.length > 0)
                this.oficinasDisponiveis = this.oficinasRecebidas.filter(x => this.oficinasEscolhidas.findIndex(y => y.Id == x.Id) == -1);
            else
                this.oficinasDisponiveis = [];
        }
        else
            this.oficinasEscolhidas = [];
    }

    @Output()
    valorChange: EventEmitter<DTOOficina[]> = new EventEmitter<DTOOficina[]>();

    oficinasEscolhidas: DTOOficina[] = [];
    oficinasDisponiveis: DTOOficina[] = [];
    oficinasDisponiveisSelecionadas: DTOOficina[] = [];
    oficinasEscolhidasSelecionadas: DTOOficina[] = [];
    situacaoSelecaoOficinasEscolhidas: EnumSituacaoSelecaoItens = EnumSituacaoSelecaoItens.NenhumItem;

    clicarIncluir(): void {
        for (let item of this.oficinasDisponiveisSelecionadas) {
            this.oficinasEscolhidas.push(item);
        }

        this.oficinasDisponiveis = this.oficinasDisponiveis.filter(x => this.oficinasDisponiveisSelecionadas.findIndex(y => y.Id == x.Id) == -1);
        this.oficinasDisponiveisSelecionadas = [];

        this.valorChange.emit(this.oficinasEscolhidas);
    }

    clicarRemover(): void {
        for (let item of this.oficinasEscolhidasSelecionadas)
            this.oficinasDisponiveis.push(item);

        this.oficinasEscolhidas = this.oficinasEscolhidas.filter(x => this.oficinasEscolhidasSelecionadas.findIndex(y => y.Id == x.Id) == -1);
        this.oficinasEscolhidasSelecionadas = [];

        this.valorChange.emit(this.oficinasEscolhidas);
    }

    processarMudancasSelecaoOficinasEscolhidas(evento: any): void {

        if (this.oficinasEscolhidasSelecionadas.length == 0)
            this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.NenhumItem;
        else if (this.oficinasEscolhidasSelecionadas.length == 1) {
            this.atualizarSituacaoSelecaoItem();
        }
        else
            this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.MaisDeUmItem;
    }

    clicarSubir(): void {
        let posicaoItemSubirah = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
        let valorPosicaoIrah = this.oficinasEscolhidas[posicaoItemSubirah - 1];
        let valorPosicaoItemSubirah = this.oficinasEscolhidas[posicaoItemSubirah];

        this.oficinasEscolhidas[posicaoItemSubirah - 1] = valorPosicaoItemSubirah;
        this.oficinasEscolhidas[posicaoItemSubirah] = valorPosicaoIrah;

        this.valorChange.emit(this.oficinasEscolhidas);

        this.atualizarSituacaoSelecaoItem();
    }

    clicarDescer(): void {
        let posicaoItemDescerah = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
        let valorPosicaoIrah = this.oficinasEscolhidas[posicaoItemDescerah + 1];
        let valorPosicaoItemDescerah = this.oficinasEscolhidas[posicaoItemDescerah];

        this.oficinasEscolhidas[posicaoItemDescerah + 1] = valorPosicaoItemDescerah;
        this.oficinasEscolhidas[posicaoItemDescerah] = valorPosicaoIrah;

        this.valorChange.emit(this.oficinasEscolhidas);

        this.atualizarSituacaoSelecaoItem();
    }

    private atualizarSituacaoSelecaoItem(): void {
        let posicao = this.oficinasEscolhidas.findIndex(x => x.Id == this.oficinasEscolhidasSelecionadas[0].Id);
        if (posicao == 0)
            this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Primeiro;
        else if (posicao == this.oficinasEscolhidas.length - 1)
            this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.Ultimo;
        else
            this.situacaoSelecaoOficinasEscolhidas = EnumSituacaoSelecaoItens.EntrePrimeiroUltimo;
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
    selector: 'comp-oficina-coordenador',
    templateUrl: './comp-oficina-coordenador.html'
})
export class ComponenteOficinaCoordenador {

    @Input()
    desabilitar: boolean;

    @Input()
    oficinas: DTOOficina[] = [];

    @Input()
    set valor(oficina: DTOOficina) {
        if (oficina != null)
            this.mValor = this.oficinas.find(x => x.Id == oficina.Id);
    }

    @Output()
    valorChange: EventEmitter<DTOOficina> = new EventEmitter<DTOOficina>();

    private mValor: DTOOficina;

    set oficinaCoordena(valor: DTOOficina) {
        this.mValor = valor;
        this.valorChange.emit(this.mValor);
    }

    get oficinaCoordena(): DTOOficina {
        return this.mValor;
    }
}

@Component({
  selector: 'comp-oficina-participante-sem-escolha',
  templateUrl: './comp-oficina-participante-sem-escolha.html'
})
export class ComponenteOficinaParticipanteSemEscolha {
}
