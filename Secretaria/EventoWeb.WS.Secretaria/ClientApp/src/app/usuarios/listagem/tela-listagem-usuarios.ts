import { Component, OnInit } from "@angular/core";
import { Alertas } from "../../componentes/alertas-dlg/alertas";
import { ActivatedRoute, Router } from "@angular/router";
import { CaixaMensagemResposta } from '../../componentes/alertas-dlg/caixa-mensagem-dlg';
import { DTOUsuario } from "../objetos";
import { WebServiceUsuariosAdm } from "../../webservices/webservice-usuarios-adm";
import { DialogoSenhaAdmin } from "../senha-admin/dlg-form-senha-admin";

@Component({
  selector: 'tela-listagem-usuarios',
  styleUrls: ['./tela-listagem-usuarios.scss'],
  templateUrl: './tela-listagem-usuarios.html'
})
export class TelaListagemUsuarios implements OnInit {

  usuarioSelecionado: DTOUsuario | null = null;
  usuarios: DTOUsuario[] = [];

  constructor(private wsUsuarios: WebServiceUsuariosAdm,
    private mensageria: Alertas,
    private dlgAlteracaoSenha: DialogoSenhaAdmin,
    private navegador: Router,
    private rotaAtual: ActivatedRoute) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando usuarios...");

    this.wsUsuarios.listarTodos()
      .subscribe(
        usuarios => {
          this.usuarios = usuarios;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.navegador.navigate(["incluir"], { relativeTo: this.rotaAtual });
  }

  clicarEditar(usuario: DTOUsuario): void {
    this.navegador.navigate(["editar", usuario.Login], { relativeTo: this.rotaAtual });
  }

  clicarExcluir(usuario: DTOUsuario): void {
    this.mensageria.alertarConfirmacao("Você quer excluir este usuário?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de usuário...");
            this.wsUsuarios.excluir(usuario.Login!)
              .subscribe(
                () => {
                  this.usuarios = this.usuarios.filter(x => x.Login != usuario.Login);
                  dlg.close();
                },
                erro => {
                  dlg.close();
                  this.mensageria.alertarErro(erro);
                });
          }
        }
      );
  }

  clicarAlterarSenha(usuario: DTOUsuario): void {
    this.dlgAlteracaoSenha.apresentarDlgAlteracao(usuario.Login!);
  }
}
