import { Component, OnInit, Input, PipeTransform, Pipe } from '@angular/core';
import { WsEventos } from '../webservices/wsEventos';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import { DTOContratoInscricao } from '../principal/objetos';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'comp-contrato',
  templateUrl: './comp-contrato.html',
  styleUrls: ['./comp-contrato.scss']
})
export class ComponenteContrato implements OnInit {

  contrato: DTOContratoInscricao;

  @Input()
  idEvento: number;

  constructor(private wsEventos: WsEventos, private coordenacao: CoordenacaoCentral) { }

  ngOnInit(): void {

    this.contrato = new DTOContratoInscricao();    

    let dlg = this.coordenacao.Alertas.alertarProcessamento("Buscando contrato...");

    this.wsEventos.obterContrato(this.idEvento)
      .subscribe(
        (contrato) => {
          if (contrato != null)
            this.contrato = contrato;
          dlg.close();
        },
        (erro) => {
          dlg.close();
          this.coordenacao.ProcessamentoErro.processar(erro);
        }
      );
  }
}

@Pipe({ name: 'sanitizeHtml', pure: false })
export class SanitizeHtmlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {
  }

  transform(content) {
    return this.sanitizer.bypassSecurityTrustHtml(content);
  }
}
