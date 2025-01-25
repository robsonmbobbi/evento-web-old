import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { GestaoAutenticacao } from "../../seguranca/gestao-autenticacao";
import { DialogoAlteracaoUsuarioComum } from "../../usuarios/alteracao-comum/dlg-form-alteracao-usuario-comum";
import { DialogoSenhaComum } from "../../usuarios/senha-comum/dlg-form-senha-comum";
import { WebServiceAutenticacao } from "../../webservices/webservice-autenticacao";
import { Alertas } from "../alertas-dlg/alertas";

@Component({
  selector: 'menu-usuario',
  templateUrl: './menu-usuario.html',
})
export class MenuUsuario {

  constructor(
    public gestaoAutenticacao: GestaoAutenticacao,
    private router: Router,
    private wsAutenticacao: WebServiceAutenticacao,
    private alertas: Alertas,
    private dlgAlteracaoSenha: DialogoSenhaComum,
    private dlgAlteracaoUsuario: DialogoAlteracaoUsuarioComum) { }

  get ehAdmin() {
    return this.gestaoAutenticacao.autenticado &&
      this.gestaoAutenticacao.dadosAutenticacao?.Usuario?.EhAdministrador;
  }

  clicarCadastroUsuarios(): void {
    this.router.navigate(["/usuarios"]);
  }

  clicarSair(): void {

    let dlg = this.alertas.alertarProcessamento("Autenticando...");

    this.wsAutenticacao.desautenticar()
      .subscribe(retorno => {
        this.gestaoAutenticacao.desautenticar();
        this.router.navigate(['login']);
        dlg.close();
      },
        dadosErro => {
          this.alertas.alertarErro(dadosErro);
          dlg.close();
        });
  }

  clicarAlterarDados(): void {
    this.dlgAlteracaoUsuario.apresentar();
  }

  clicarAlterarSenha(): void {
    this.dlgAlteracaoSenha.apresentar();
  }
}
