import { Component, Input, Output, ViewChild } from "@angular/core";
import { DxValidationGroupComponent } from "devextreme-angular/ui/validation-group";

@Component({
  selector: 'form-usuario',
  styleUrls: ['./form-usuario.scss'],
  templateUrl: './form-usuario.html'
})
export class FormUsuario {    
  @Input()
  login: string = "";

  @Input()
  nome: string = "";

  @Input()
  ehAdministrador: boolean = false;

  @Input()
  desabilitarLogin: boolean = false;

  @Input()
  desabilitarEhAdministrador: boolean = false;

  @ViewChild("grupoValidacao", { static: true })
  grupoValidacao!: DxValidationGroupComponent;

  @Output()
  get dadosEstaoValidos(): boolean {
    return this.grupoValidacao.instance.validate().isValid ?? false;
  }
}
