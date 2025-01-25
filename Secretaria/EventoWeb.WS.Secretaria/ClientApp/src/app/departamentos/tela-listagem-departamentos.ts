import { Component, OnInit } from "@angular/core";
import { Alertas } from "../componentes/alertas-dlg/alertas";
import { ActivatedRoute } from "@angular/router";
import { CaixaMensagemResposta } from '../componentes/alertas-dlg/caixa-mensagem-dlg';
import { ServicoEventoSelecionado } from '../evento/tela-roteamento-evento';
import { DTOEventoCompleto } from '../evento/objetos';
import { DTODepartamento } from './objetos';
import { WebServiceDepartamentos } from '../webservices/webservice-departamentos';
import { DialogosDepartamentos } from './dlg-form-departamento';


@Component({
  selector: 'tela-listagem-departamentos',
  styleUrls: ['./tela-listagem-departamentos.scss'],
  templateUrl: './tela-listagem-departamentos.html'
})
export class TelaListagemDepartamentos implements OnInit {

  departamentoSelecionado: DTODepartamento | null = null;
  departamentos: DTODepartamento[] = [];

  private m_Evento!: DTOEventoCompleto;

  constructor(private wsDepartamentos: WebServiceDepartamentos, private mensageria: Alertas, private roteador: ActivatedRoute,
    private dlgsDepartamentos: DialogosDepartamentos, private srvEventoSelecionado: ServicoEventoSelecionado) { }

  ngOnInit(): void {
    let dlg = this.mensageria.alertarProcessamento("Buscando departamentos...");

    this.m_Evento = this.srvEventoSelecionado.EventoSelecionado;

    this.wsDepartamentos.obterTodos(this.m_Evento.Id)
      .subscribe(
        departamentos => {
          this.departamentos = departamentos;
          dlg.close();
        },
        erro => {
          dlg.close();
          this.mensageria.alertarErro(erro);
        });
  }

  clicarIncluir(): void {
    this.dlgsDepartamentos.apresentarDlgFormDialogoInclusao(this.m_Evento.Id)
      .subscribe(
        (novaDepartamento) => {
          if (novaDepartamento != null)
            this.departamentos.push(novaDepartamento);
        }
      );
  }

  clicarEditar(departamento: DTODepartamento): void {
    this.dlgsDepartamentos.apresentarDlgFormDialogoEdicao(this.m_Evento.Id, departamento.Id)
      .subscribe(
        (departamentoAlterado) => {
          if (departamentoAlterado != null) {
            let indice = this.departamentos.findIndex(x => x.Id == departamentoAlterado.Id);
            if (indice != -1) {
              this.departamentos[indice] = departamentoAlterado;
            }
          }            
        }
      );
  }

  clicarExcluir(departamento: DTODepartamento): void {
    this.mensageria.alertarConfirmacao("Você quer excluir este departamento mesmo?", "")
      .subscribe(
        (resposta) => {
          if (resposta == CaixaMensagemResposta.Sim) {
            let dlg = this.mensageria.alertarProcessamento("Processando exclusão de departamento...");
            this.wsDepartamentos.excluir(this.m_Evento.Id, departamento.Id)
              .subscribe(
                exclusao => {
                  this.departamentos = this.departamentos.filter(x => x.Id != departamento.Id);
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
}
