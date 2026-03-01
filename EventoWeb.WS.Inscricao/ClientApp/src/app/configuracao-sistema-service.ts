
export class Configuracao {
  urlBaseWs: string;
}

export class ConfiguracaoSistemaService {

  private static mConfiguracao: Configuracao = new Configuracao();

  static set configuracao(valor: Configuracao) {
      this.mConfiguracao = valor;
  }

  static get configuracao(): Configuracao {
    return this.mConfiguracao;
  }
}
