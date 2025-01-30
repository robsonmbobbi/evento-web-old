import { Component, ViewChild, Input } from '@angular/core';
import { CoordenacaoCentral } from '../componentes/central/coordenacao-central';
import {
  DTOSarau, DTOInscricaoSimplificada, DTOPagamento, EnumSexo,
  DTOInscricaoDadosPessoais, EnumPagamento, DTOInscricaoAtualizacaoInfantil
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { WsManutencaoInscricoes } from '../webservices/wsManutencaoInscricoes';
import { DTOEventoCompleto } from '../principal/objetos';
import { DialogosInscricao } from './dlg-inscricao-adulto-codigo';

@Component({
  selector: 'comp-form-inscricao-infantil',
  templateUrl: './comp-form-inscricao-infantil.html',
  styleUrls: ['./comp-form-inscricao-infantil.scss']
})
export class CompFormInscricaoInfantil {

  @Input()
  naoEhIncompleta: boolean = false;

  dadosTela: DadosTela;

  private mEvento: DTOEventoCompleto;
  private mInscricao: DTOInscricaoAtualizacaoInfantil;

  @ViewChild("grupoValidacaoEssencial")
  grupoValidacaoEssencial: DxValidationGroupComponent;

  @ViewChild("grupoValidacaoEspirita")
  grupoValidacaoEspirita: DxValidationGroupComponent;

  constructor(public coordenacao: CoordenacaoCentral, private wsInscricoes: WsManutencaoInscricoes, private DlgsInscricao: DialogosInscricao) { }

  @Input()
  set inscricao(valor: DTOInscricaoAtualizacaoInfantil) {

    this.mInscricao = valor;

    this.dadosTela = new DadosTela(this.coordenacao);
    this.dadosTela.nome = this.mInscricao.DadosPessoais.Nome;
    this.dadosTela.dataNascimento = this.mInscricao.DadosPessoais.DataNascimento && new Date(this.mInscricao.DadosPessoais.DataNascimento);
    this.dadosTela.email = this.mInscricao.DadosPessoais.Email;
    this.dadosTela.sexoEscolhido = this.coordenacao.Sexos[this.mInscricao.DadosPessoais.Sexo];
    this.dadosTela.cidade = this.mInscricao.DadosPessoais.Cidade;
    this.dadosTela.uf = this.mInscricao.DadosPessoais.Uf;
    this.dadosTela.ehVegetariano = this.mInscricao.DadosPessoais.EhVegetariano;
    this.dadosTela.usaAdocanteDiariamente = this.mInscricao.DadosPessoais.UsaAdocanteDiariamente;
    this.dadosTela.ehDiabetico = this.mInscricao.DadosPessoais.EhDiabetico;
    this.dadosTela.alimentosAlergia = this.mInscricao.DadosPessoais.AlimentosAlergia;
    this.dadosTela.celular = this.mInscricao.DadosPessoais.Celular;
    this.dadosTela.nomeCracha = this.mInscricao.NomeCracha;
    this.dadosTela.responsavel1 = this.mInscricao.Responsavel1;
    this.dadosTela.responsavel2 = this.mInscricao.Responsavel2;
    this.dadosTela.dormiraEvento = this.mInscricao.DormeEvento;

    this.dadosTela.sarais = this.mInscricao.Sarais;
    this.dadosTela.pagamento = this.mInscricao.Pagamento;
    this.dadosTela.observacoes = this.mInscricao.Observacoes;

    this.atribuirInscricaoSimples();
  }

  @Input()
  set evento(valor: DTOEventoCompleto) {

    this.mEvento = valor;

    this.dadosTela.descricaoEvento = this.mEvento.Nome;
    this.dadosTela.idadeMinima = this.mEvento.IdadeMinima;
    this.dadosTela.dataMinimaNascimento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);
    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - (this.mEvento.IdadeMinima));
    this.dadosTela.dataInicioEvento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);

    this.atribuirInscricaoSimples();
  }
  get evento() {
    return this.mEvento;
  }

  private atribuirInscricaoSimples(): void {

    if (this.mEvento && this.mInscricao)
      this.dadosTela.inscricaoSimples = { Id: 0, IdEvento: this.mEvento.Id, Nome: this.mInscricao.DadosPessoais.Nome };
  }

  gerarAtualizacaoInscricao(): ResultadoAtualizacaoInscricaoInfantil {

    let resultado = new ResultadoAtualizacaoInscricaoInfantil();
    resultado.valido = false;
    resultado.inscricaoAtualizar = null;

    let dadosPessoaisValidos = this.grupoValidacaoEssencial.instance.validate().isValid;
    let dadosEspiritasValidos = this.grupoValidacaoEspirita.instance.validate().isValid;

    if (!dadosPessoaisValidos || !dadosEspiritasValidos)
      this.coordenacao.Alertas.alertarAtencao("Há informações pessoais que precisam de seus cuidados.", "Sem essas informações não é possível enviar a inscrição.");
    else if (this.dadosTela.responsavel1 == null)
      this.coordenacao.Alertas.alertarAtencao("Você não nos informou um pessoa responsável pela criança.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento == null || this.dadosTela.pagamento.Forma == null)
      this.coordenacao.Alertas.alertarAtencao("Você precisa informar o Pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento != null && this.dadosTela.pagamento.Forma == EnumPagamento.Comprovante &&
      (this.dadosTela.pagamento.Comprovantes == null || this.dadosTela.pagamento.Comprovantes.length == 0))
      this.coordenacao.Alertas.alertarAtencao("Você precisa informar o(s) comprovante(s) de pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else {
      let atualizacao = new DTOInscricaoAtualizacaoInfantil();
      atualizacao.DadosPessoais = new DTOInscricaoDadosPessoais();
      atualizacao.DadosPessoais.DataNascimento = this.dadosTela.dataNascimento;
      atualizacao.DadosPessoais.Email = this.dadosTela.email;
      atualizacao.DadosPessoais.Nome = this.dadosTela.nome;
      atualizacao.DadosPessoais.Sexo = (this.dadosTela.sexoEscolhido == this.coordenacao.Sexos[0] ? EnumSexo.Masculino : EnumSexo.Feminino);
      atualizacao.DadosPessoais.AlimentosAlergia = this.dadosTela.alimentosAlergia;
      atualizacao.DadosPessoais.Cidade = this.dadosTela.cidade;
      atualizacao.DadosPessoais.EhDiabetico = this.dadosTela.ehDiabetico;
      atualizacao.DadosPessoais.EhVegetariano = this.dadosTela.ehVegetariano;
      atualizacao.DadosPessoais.Celular = this.dadosTela.celular;
      atualizacao.DadosPessoais.Uf = this.dadosTela.uf;
      atualizacao.DadosPessoais.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;
      atualizacao.NomeCracha = this.dadosTela.nomeCracha;
      atualizacao.Responsavel1 = this.dadosTela.responsavel1;
      atualizacao.Responsavel2 = this.dadosTela.responsavel2;
      atualizacao.DormeEvento = this.dadosTela.dormiraEvento;

      atualizacao.Sarais = this.dadosTela.sarais;
      atualizacao.Observacoes = this.dadosTela.observacoes;

      atualizacao.Pagamento = new DTOPagamento();
      atualizacao.Pagamento.Forma = this.dadosTela.pagamento.Forma;
      atualizacao.Pagamento.Observacao = this.dadosTela.pagamento.Observacao;
      if (this.dadosTela.pagamento.Comprovantes != null)
        atualizacao.Pagamento.Comprovantes = this.dadosTela.pagamento.Comprovantes.map(x =>
          ({ Base64: x.Base64.substring(x.Base64.indexOf(",") + 1), TipoArquivo: x.TipoArquivo }))

      resultado.valido = true;
      resultado.inscricaoAtualizar = atualizacao;
    }

    return resultado;
  }

  clicarPesquisarResp1(): void {
    this.DlgsInscricao.apresentarDlgCodigo(this.evento.Id)
      .subscribe(
        (codigo) => {
          if (codigo != null) {

            let dlg = this.coordenacao.Alertas.alertarProcessamento("Buscando inscrição pelo código...");

            this.wsInscricoes.obterInscricaoAdultoSimplificadaPorCodigo(this.evento.Id, codigo.toUpperCase())
              .subscribe(
                (inscricao) => {
                  dlg.close();

                  if (inscricao != null)
                    this.dadosTela.responsavel1 = inscricao;
                  else
                    this.coordenacao.Alertas.alertarInformacao("Inscrição não encontrada com o código informado, ou ela é infantil", "");
                },
                (erro) => {
                  dlg.close();
                  this.coordenacao.ProcessamentoErro.processar(erro);
                }
              );
          }
        }
      );
  }

  clicarPesquisarResp2(): void {
    if (this.dadosTela.responsavel1 == null)
      this.coordenacao.Alertas.alertarAtencao("Você precisa informar o Responsável 1 pela criança, antes de informar o segundo.", "");
    else {
      this.DlgsInscricao.apresentarDlgCodigo(this.evento.Id)
        .subscribe(
          (codigo) => {
            if (codigo != null) {

              let dlg = this.coordenacao.Alertas.alertarProcessamento("Buscando inscrição pelo código...");

              this.wsInscricoes.obterInscricaoAdultoSimplificadaPorCodigo(this.evento.Id, codigo.toUpperCase())
                .subscribe(
                  (inscricao) => {
                    dlg.close();

                    if (inscricao != null)
                      this.dadosTela.responsavel2 = inscricao;
                    else
                      this.coordenacao.Alertas.alertarInformacao("Inscrição não encontrada com o código informado, ou ela é infantil", "");
                  },
                  (erro) => {
                    dlg.close();
                    this.coordenacao.ProcessamentoErro.processar(erro);
                  }
                );
            }
          }
        );
    }
  }
}

class DadosTela {
  nome: string;
  dataNascimento: Date;
  email: string;
  descricaoEvento: string;
  dataMinimaNascimento: Date;
  idadeMinima: number;
  sexoEscolhido: string;
  tipoInscricao: string;
  cidade: string;
  uf: string;
  ehVegetariano: boolean;
  usaAdocanteDiariamente: boolean;
  ehDiabetico: boolean;
  alimentosAlergia: string;
  dataInicioEvento: Date;
  observacoes: string;
  celular: string;
  nomeCracha: string;
  dormiraEvento: boolean;

  sarais: DTOSarau[];
  responsavel1: DTOInscricaoSimplificada;
  responsavel2: DTOInscricaoSimplificada;

  inscricaoSimples: DTOInscricaoSimplificada;

  pagamento: DTOPagamento;

  constructor(private coordenacao: CoordenacaoCentral) { }

  get idade(): number {
    if (this.dataInicioEvento == null || this.dataNascimento == null)
      return 0;
    else {
      let idade = this.dataInicioEvento.getFullYear() - this.dataNascimento.getFullYear();
      let meses = this.dataInicioEvento.getMonth() - this.dataNascimento.getMonth();

      if (meses < 0 || (meses === 0 && this.dataInicioEvento.getDate() < this.dataNascimento.getDate()))
        idade--;

      return idade;
    }
  }
}

export class ResultadoAtualizacaoInscricaoInfantil {
  valido: boolean;
  inscricaoAtualizar: DTOInscricaoAtualizacaoInfantil;
}
