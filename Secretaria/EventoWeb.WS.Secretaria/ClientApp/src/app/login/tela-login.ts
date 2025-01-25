import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GestaoAutenticacao } from '../seguranca/gestao-autenticacao';
import { WebServiceAutenticacao } from '../webservices/webservice-autenticacao';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTWDadosAutenticacao } from '../seguranca/objetos';
import { DxValidationGroupComponent } from 'devextreme-angular/ui/validation-group';

@Component({
  selector: 'tela-login',
  templateUrl: './tela-login.html',
  styleUrls: ['./tela-login.scss']
})

export class TelaLogin implements OnInit {

  paraOndeRedirecionar!: string;
  nomeUsuario!: string;
  senha!: string;

  @ViewChild("grupoValidacao", { static: true })
  grupoValidacao!: DxValidationGroupComponent;

  constructor(public gestaoAutenticacao: GestaoAutenticacao,
    public router: Router,
    public route: ActivatedRoute,
    public wsAutenticacao: WebServiceAutenticacao,
    public alertas: Alertas) {

  }

  ngOnInit() {
    this.paraOndeRedirecionar = this.route.snapshot.queryParams['urlRetornar'] || '/';

    if (this.gestaoAutenticacao.autenticado)
      this.router.navigate([this.paraOndeRedirecionar]);
  }

  clicarAutenticar(): void {

    const resultadoValidacao = this.grupoValidacao.instance.validate();

    if (!resultadoValidacao.isValid)
      this.alertas.alertarAtencao("Nâo deu para fazer o login!!", "Ops, acho que alguns dados informados não estão legais!");
    else {
      let dlg = this.alertas.alertarProcessamento("Autenticando...");

      let dto = new DTWDadosAutenticacao();
      dto.Login = this.nomeUsuario;
      dto.Senha = this.senha;

      this.wsAutenticacao.autenticar(dto)
        .subscribe(dadosAutenticacao => {

          if (dadosAutenticacao != null) {
            this.gestaoAutenticacao.autenticar(dadosAutenticacao);
            this.router.navigate([this.paraOndeRedirecionar]);
          }
          else
            this.alertas.alertarAtencao("Usuário ou senha inválidos!!", "");

          dlg.close();
        },
          dadosErro => {
            dlg.close();
            this.alertas.alertarErro(dadosErro);
          });
    }
  }
}
