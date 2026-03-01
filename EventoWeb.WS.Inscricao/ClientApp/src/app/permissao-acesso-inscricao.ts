import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { CoordenacaoCentral } from './componentes/central/coordenacao-central';

@Injectable()
export class PermissaoAcessoInscricao implements CanActivate {

  constructor(private router: Router, private coordenacao: CoordenacaoCentral) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.coordenacao.AutorizacoesInscricao.obterAutorizacao(route.params["idinscricao"]) != null)
      return true;
    else {
      this.router.navigate(['pesquisar']);
      return false;
    }
  }
}
