import { Alertas } from '../alertas-dlg/alertas';
import { Router } from '@angular/router';

export class ServicoProcessamentoErros {

  constructor(private alertas: Alertas, private router: Router) { }

  public processar(erro: any): void {
    if (typeof erro == "string") {

      if (erro.toLowerCase().indexOf("http failure response") >= 0) {
        this.router.navigate(['']);
      }
      else
        this.alertas.alertarErro(erro);
    }
    else {
      this.alertas.alertarErro(erro.MensagemErro);
    }      

    console.log(erro);
  }
}
