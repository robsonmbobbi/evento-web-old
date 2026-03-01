"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServicoProcessamentoErros = /** @class */ (function () {
    function ServicoProcessamentoErros(alertas, gestorAutenticacao, router) {
        this.alertas = alertas;
        this.gestorAutenticacao = gestorAutenticacao;
        this.router = router;
    }
    ServicoProcessamentoErros.prototype.processar = function (erro) {
        if (typeof erro == "string") {
            if (erro.toLowerCase().indexOf("http failure response") >= 0) {
                this.gestorAutenticacao.desautenticar();
                this.router.navigate(['']);
            }
            else
                this.alertas.alertarErro(erro);
        }
        else {
            this.alertas.alertarErro(erro.MensagemErro);
        }
        console.log(erro);
    };
    return ServicoProcessamentoErros;
}());
exports.ServicoProcessamentoErros = ServicoProcessamentoErros;
//# sourceMappingURL=servico-processamento-erros.js.map