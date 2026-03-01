"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Configuracao = /** @class */ (function () {
    function Configuracao() {
    }
    return Configuracao;
}());
exports.Configuracao = Configuracao;
var ConfiguracaoSistemaService = /** @class */ (function () {
    function ConfiguracaoSistemaService() {
    }
    Object.defineProperty(ConfiguracaoSistemaService, "configuracao", {
        get: function () {
            return this.mConfiguracao;
        },
        set: function (valor) {
            this.mConfiguracao = valor;
        },
        enumerable: true,
        configurable: true
    });
    ConfiguracaoSistemaService.mConfiguracao = new Configuracao();
    return ConfiguracaoSistemaService;
}());
exports.ConfiguracaoSistemaService = ConfiguracaoSistemaService;
//# sourceMappingURL=configuracao-sistema-service.js.map