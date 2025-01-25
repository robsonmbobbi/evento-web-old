import { Component, OnInit, ViewChild } from '@angular/core';
import { Alertas } from '../../componentes/alertas-dlg/alertas';
import { DTOUsuario } from '../objetos';
import { WebServiceUsuariosAdm } from '../../webservices/webservice-usuarios-adm';
import { ActivatedRoute, Router } from '@angular/router';
import { FormUsuario } from './form-usuario';
import { DialogoSenhaAdmin } from '../senha-admin/dlg-form-senha-admin';

enum EnumSituacaoTelaUsuario { Inclusao, Alteracao, NaoEncontrado }

@Component({
  selector: 'tela-manutencao-usuario',
  styleUrls: ['./tela-manutencao-usuario.scss'],
  templateUrl: './tela-manutencao-usuario.html'
})
export class TelaManutencaoUsuario implements OnInit {

  usuario!: DTOUsuario;
  situacao: EnumSituacaoTelaUsuario = EnumSituacaoTelaUsuario.Inclusao;

  @ViewChild("formUsuario", { static: true })
  formUsuario!: FormUsuario;

  constructor(private wsUsuarios: WebServiceUsuariosAdm,
    private mensageria: Alertas,
    private dlgSenha: DialogoSenhaAdmin,
    private navegadorUrlAtual: ActivatedRoute,
    private navegador: Router) { }

  ngOnInit(): void {

    this.usuario = {
      EhAdministrador: false,
      Login: "",
      Nome: ""
    };

    this.navegadorUrlAtual.params
      .subscribe(
        (parametros) => {

          let nomeUsuario = parametros["login"];

          if (nomeUsuario != null) {

            let dlg = this.mensageria.alertarProcessamento("Buscando usuário...");

            this.wsUsuarios.obter(nomeUsuario)
              .subscribe(
                (usuarioRetornado) => {

                  if (usuarioRetornado == null)
                    this.situacao = EnumSituacaoTelaUsuario.NaoEncontrado
                  else {
                    this.situacao = EnumSituacaoTelaUsuario.Alteracao;
                    this.usuario = usuarioRetornado;
                  }
                  dlg.close();
                },
                (erro) => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                }
              );
          }
        }
      );
  }

  clicarSalvar(): void {
    
    if (!this.formUsuario.dadosEstaoValidos) {
      this.mensageria.alertarAtencao("Houve um problema com os dados informados!", "Verifique as informações digitadas!!");
    }
    else {
      if (this.situacao == EnumSituacaoTelaUsuario.Alteracao) {

        let dlg = this.mensageria.alertarProcessamento("Atualizando usuário...");

        this.usuario.Login = this.formUsuario.login;
        this.usuario.Nome = this.formUsuario.nome;
        this.usuario.EhAdministrador = this.formUsuario.ehAdministrador;

        this.wsUsuarios.atualizar(this.usuario)
          .subscribe(
            () => {
              dlg.close();
              this.navegador.navigate(["/usuarios"]);
            },
            erro => {
              dlg.close();
              this.mensageria.alertarErro(erro);
            }
          );
      }
      else {
        this.dlgSenha.apresentarDlgForm()
          .subscribe(
            dadosSenha => {
              if (dadosSenha != null) {

                let dlg = this.mensageria.alertarProcessamento("Incluindo usuário...");

                this.wsUsuarios.incluir({
                  EhAdministrador: this.formUsuario.ehAdministrador,
                  Login: this.formUsuario.login,
                  Nome: this.formUsuario.nome,
                  RepeticaoSenha: dadosSenha.NovaSenhaRepetida,
                  Senha: dadosSenha.NovaSenha
                })
                  .subscribe(
                    () => {
                      dlg.close();
                      this.navegador.navigate(["/usuarios"]);
                    },
                    erro => {
                      dlg.close();
                      this.mensageria.alertarErro(erro);
                    }
                  );
              }
            }
          );
      }
    }
  }
}
