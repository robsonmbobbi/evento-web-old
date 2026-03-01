import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EnumApresentacaoAtividades, DTOInscricaoDepartamento } from '../objetos';
import { DTODepartamento } from '../../principal/objetos';

@Component({
  selector: 'comp-departamentos',
  templateUrl: './comp-departamentos.html'
})
export class ComponenteDepartamentos {

  apresentacao: EnumApresentacaoAtividades;
  opcoes: string[] = ["Participante", "Coordenador"];
  _opcaoEscolhida: string;

  private mDepartamentoEscolhido: DTODepartamento;
  private mInscDepartamento: DTOInscricaoDepartamento;
  private mDepartamentos: DTODepartamento[];

  @Input()
  desabilitar: boolean;

  @Input()
  set departamentos(valor: DTODepartamento[]) {
    this.mDepartamentos = valor;
    this.selecionarDepartamento();
  }
  get departamentos(): DTODepartamento[] {
    return this.mDepartamentos;
  }

  @Input()
  set valor(param: DTOInscricaoDepartamento) {
    this.mInscDepartamento = param;
    if (param == null)
      this.mDepartamentoEscolhido = null;
    else {
      if (param.Coordenador != null)
        this._opcaoEscolhida = this.opcoes[1];
      else
        this._opcaoEscolhida = this.opcoes[0];

      this.selecionarDepartamento();
    }
  }

  private selecionarDepartamento(): void {
    if (this.mInscDepartamento && this.mDepartamentos) {
      if (this.mInscDepartamento.Coordenador != null)
        this.mDepartamentoEscolhido = this.mDepartamentos.find(x => x.Id == this.mInscDepartamento.Coordenador.Id)
      else
        this.mDepartamentoEscolhido = this.mDepartamentos.find(x => x.Id == this.mInscDepartamento.Participante.Id)
    }
  }

  @Output()
  valorChange: EventEmitter<DTOInscricaoDepartamento> = new EventEmitter<DTOInscricaoDepartamento>();

  @Input()
  set forma(valor: EnumApresentacaoAtividades) {

    this.apresentacao = valor;

    if (this.apresentacao == EnumApresentacaoAtividades.ApenasParticipante)
      this.opcaoEscolhida = this.opcoes[0];
  }

  set opcaoEscolhida(valor: string) {
    if (valor != this._opcaoEscolhida) {
      this._opcaoEscolhida = valor;
      this.departamentoEscolhido = null;
    }
  }

  get opcaoEscolhida(): string {
    return this._opcaoEscolhida;
  }

  set departamentoEscolhido(valor: DTODepartamento) {
    this.mDepartamentoEscolhido = valor;

    if (valor == null)
      this.valorChange.emit(null);
    else
      this.valorChange.emit({
        Coordenador: this.opcaoEscolhida == this.opcoes[1] ? valor : null,
        Participante: this.opcaoEscolhida == this.opcoes[0] ? valor : null
      });
  }

  get departamentoEscolhido(): DTODepartamento {
    return this.mDepartamentoEscolhido;
  }
}
