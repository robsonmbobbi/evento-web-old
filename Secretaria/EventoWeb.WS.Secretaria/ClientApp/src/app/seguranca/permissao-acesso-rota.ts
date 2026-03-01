import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { GestaoAutenticacao } from './gestao-autenticacao'

@Injectable()
export class PermissaoAcessoRota implements CanActivate {

  constructor(public router: Router, public gestaoAutenticacao: GestaoAutenticacao) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.gestaoAutenticacao.autenticado)
      return true;

    // not logged in so redirect to login page with the return url
    this.router.navigate(['login'], { queryParams: { urlRetornar: state.url } });
    return false;
  }
}
