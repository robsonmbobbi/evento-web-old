import { Component, ViewChild, Input } from '@angular/core';
import {
  EnumApresentacaoAtividades, DTOInscricaoSimplificada,
  DTOPagamento, DTOInscricaoAtualizacao, EnumSexo, DTOInscricaoDadosPessoais, EnumTipoInscricao,
  DTOInscricaoOficina, DTOInscricaoSalaEstudo, DTOInscricaoDepartamento, EnumPagamento
} from './objetos';
import { DxValidationGroupComponent } from 'devextreme-angular';
import { DTOEventoCompletoInscricao, EnumModeloDivisaoOficinas, EnumModeloDivisaoSalasEstudo } from '../evento/objetos';
import { Alertas } from '../componentes/alertas-dlg/alertas';
import { DTOSarau } from '../sarais/objetos';

@Component({
  selector: 'comp-form-inscricao',
  templateUrl: './comp-form-inscricao.html',
  styleUrls: ['./comp-form-inscricao.scss']
})
export class CompFormInscricao {

  @Input()
  naoEhIncompleta: boolean = false;

  dadosTela: DadosTela;

  private mEvento: DTOEventoCompletoInscricao;
  private mInscricao: DTOInscricaoAtualizacao;

  @ViewChild("grupoValidacaoEssencial")
  grupoValidacaoEssencial: DxValidationGroupComponent;

  @ViewChild("grupoValidacaoEspirita")
  grupoValidacaoEspirita: DxValidationGroupComponent;

  constructor(private mensageria: Alertas) { }

  @Input()
  set inscricao(valor: DTOInscricaoAtualizacao) {

    this.mInscricao = valor;

    this.dadosTela = new DadosTela();
    this.dadosTela.nome = this.mInscricao.DadosPessoais.Nome;
    this.dadosTela.dataNascimento = this.mInscricao.DadosPessoais.DataNascimento && new Date(this.mInscricao.DadosPessoais.DataNascimento);
    this.dadosTela.email = this.mInscricao.DadosPessoais.Email;
    this.dadosTela.sexoEscolhido = this.dadosTela.Sexos[this.mInscricao.DadosPessoais.Sexo];
    this.dadosTela.tipoInscricaoEscolhida = this.dadosTela.TiposInscricao[this.mInscricao.TipoInscricao];
    this.dadosTela.cidade = this.mInscricao.DadosPessoais.Cidade;
    this.dadosTela.uf = this.mInscricao.DadosPessoais.Uf;
    this.dadosTela.ehVegetariano = this.mInscricao.DadosPessoais.EhVegetariano;
    this.dadosTela.usaAdocanteDiariamente = this.mInscricao.DadosPessoais.UsaAdocanteDiariamente;
    this.dadosTela.ehDiabetico = this.mInscricao.DadosPessoais.EhDiabetico;
    this.dadosTela.alimentosAlergia = this.mInscricao.DadosPessoais.AlimentosAlergia;
    this.dadosTela.centroEspirita = this.mInscricao.CentroEspirita;
    this.dadosTela.celular = this.mInscricao.DadosPessoais.Celular;
    this.dadosTela.nomeCracha = this.mInscricao.NomeCracha;
    this.dadosTela.dormiraEvento = this.mInscricao.DormeEvento;

    this.dadosTela.oficinasEscolhidas = this.mInscricao.Oficina;

    this.dadosTela.salasEscolhidas = this.mInscricao.SalasEstudo;
    this.dadosTela.departamentoEscolhido = this.mInscricao.Departamento;
    this.dadosTela.sarais = this.mInscricao.Sarais;
    this.dadosTela.pagamento = this.mInscricao.Pagamento;
    this.dadosTela.observacoes = this.mInscricao.Observacoes;

    this.atribuirInscricaoSimples();
  }

  @Input()
  set evento(valor: DTOEventoCompletoInscricao) {

    this.mEvento = valor;

    this.dadosTela.descricaoEvento = this.mEvento.Nome;
    this.dadosTela.idadeMinima = this.mEvento.IdadeMinima;
    this.dadosTela.dataMinimaNascimento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);
    this.dadosTela.dataMinimaNascimento.setFullYear(this.dadosTela.dataMinimaNascimento.getFullYear() - this.mEvento.IdadeMinima);
    this.dadosTela.dataInicioEvento = new Date(this.mEvento.PeriodoRealizacao.DataInicial);

    this.atribuirInscricaoSimples();
  }
  get evento() {
    return this.mEvento;
  }

  private atribuirInscricaoSimples(): void {

    if (this.mEvento && this.mInscricao)
      this.dadosTela.inscricaoSimples = {
        Id: 0,
        IdEvento: this.mEvento.Id,
        Nome: this.mInscricao.DadosPessoais.Nome,
        Cidade: this.mInscricao.DadosPessoais.Cidade,
        UF: this.mInscricao.DadosPessoais.Uf
      };
  }

  gerarAtualizacaoInscricao(): ResultadoAtualizacaoInscricao {

    let resultado = new ResultadoAtualizacaoInscricao();
    resultado.valido = false;
    resultado.inscricaoAtualizar = null;

    let dadosPessoaisValidos = this.grupoValidacaoEssencial.instance.validate().isValid;
    let dadosEspiritasValidos = this.grupoValidacaoEspirita.instance.validate().isValid;

    if (!dadosPessoaisValidos || !dadosEspiritasValidos)
      this.mensageria.alertarAtencao("Há informações pessoais que precisam de seus cuidados.", "Sem essas informações não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoOficinas == EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao &&
      this.dadosTela.oficinasEscolhidas == null)
      this.mensageria.alertarAtencao("Você não escolheu as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoOficinas == EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.mEvento.Oficinas.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as oficinas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas == null)
      this.mensageria.alertarAtencao("Você não escolheu as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.mEvento.SalasEstudo.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as salas.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[0] &&
      this.mEvento.TemDepartamentalizacao &&
      this.dadosTela.departamentoEscolhido == null)
      this.mensageria.alertarAtencao("Você não escolheu o departamento que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[1] &&
      this.mEvento.ConfiguracaoOficinas == EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao &&
      this.dadosTela.oficinasEscolhidas != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.oficinasEscolhidas.EscolhidasParticipante.length != this.mEvento.Oficinas.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as oficinas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[1] &&
      this.mEvento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao &&
      this.dadosTela.salasEscolhidas != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante != null &&
      this.dadosTela.salasEscolhidas.EscolhidasParticipante.length != this.mEvento.SalasEstudo.length)
      this.mensageria.alertarAtencao("Você não escolheu todas as salas que deseja participar.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.tipoInscricaoEscolhida == this.dadosTela.TiposInscricao[1] &&
      ((this.mEvento.TemDepartamentalizacao || this.mEvento.ConfiguracaoOficinas != null || this.mEvento.ConfiguracaoSalaEstudo != null) &&
      !((this.mEvento.ConfiguracaoOficinas != null && this.dadosTela.oficinasEscolhidas != null) ||
          (this.mEvento.TemDepartamentalizacao && this.dadosTela.departamentoEscolhido != null) ||
          (this.mEvento.ConfiguracaoSalaEstudo != null && this.dadosTela.salasEscolhidas != null))))
      this.mensageria.alertarAtencao("Você nos disse que a sua inscrição é de Participante/Trabalhador, mas não escolheu participar em nenhuma atividade!", "Por favor escolha um atividade para fazer parte.");
    else if (this.dadosTela.pagamento == null || this.dadosTela.pagamento.Forma == null)
      this.mensageria.alertarAtencao("Você precisa informar o Pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else if (this.dadosTela.pagamento != null && this.dadosTela.pagamento.Forma == EnumPagamento.Comprovante &&
      (this.dadosTela.pagamento.Comprovantes == null || this.dadosTela.pagamento.Comprovantes.length == 0))
      this.mensageria.alertarAtencao("Você precisa informar o(s) comprovante(s) de pagamento.", "Sem essa informação não é possível enviar a inscrição.");
    else {
      let atualizacao = new DTOInscricaoAtualizacao();
      atualizacao.DadosPessoais = new DTOInscricaoDadosPessoais();
      atualizacao.DadosPessoais.DataNascimento = this.dadosTela.dataNascimento;
      atualizacao.DadosPessoais.Email = this.dadosTela.email;
      atualizacao.DadosPessoais.Nome = this.dadosTela.nome;
      atualizacao.DadosPessoais.Sexo = (this.dadosTela.sexoEscolhido == this.dadosTela.Sexos[0] ? EnumSexo.Masculino : EnumSexo.Feminino);
      atualizacao.DadosPessoais.AlimentosAlergia = this.dadosTela.alimentosAlergia;
      atualizacao.DadosPessoais.Cidade = this.dadosTela.cidade;
      atualizacao.DadosPessoais.EhDiabetico = this.dadosTela.ehDiabetico;
      atualizacao.DadosPessoais.EhVegetariano = this.dadosTela.ehVegetariano;
      atualizacao.DadosPessoais.Celular = this.dadosTela.celular;
      atualizacao.TipoInscricao = this.dadosTela.TiposInscricao.indexOf(this.dadosTela.tipoInscricao);
      atualizacao.DadosPessoais.Uf = this.dadosTela.uf;
      atualizacao.DadosPessoais.UsaAdocanteDiariamente = this.dadosTela.usaAdocanteDiariamente;
      atualizacao.NomeCracha = this.dadosTela.nomeCracha;
      atualizacao.CentroEspirita = this.dadosTela.centroEspirita;
      atualizacao.DormeEvento = this.dadosTela.dormiraEvento;

      if (atualizacao.TipoInscricao != EnumTipoInscricao.Trabalhador) {
        atualizacao.Departamento = this.dadosTela.departamentoEscolhido;
        atualizacao.Oficina = this.dadosTela.oficinasEscolhidas;
        atualizacao.SalasEstudo = this.dadosTela.salasEscolhidas;
      }

      atualizacao.Sarais = this.dadosTela.sarais;
      atualizacao.Observacoes = this.dadosTela.observacoes;

      atualizacao.Pagamento = new DTOPagamento();
      atualizacao.Pagamento.Forma = this.dadosTela.pagamento.Forma;
      atualizacao.Pagamento.Observacao = this.dadosTela.pagamento.Observacao;
      if (this.dadosTela.pagamento.Comprovantes != null)
        atualizacao.Pagamento.Comprovantes = this.dadosTela.pagamento.Comprovantes.map(x =>
          ({ Base64: x.Base64.substring(x.Base64.indexOf(",") + 1), TipoArquivo: x.TipoArquivo }));

      resultado.valido = true;
      resultado.inscricaoAtualizar = atualizacao;
    }

    return resultado;
  }
}

export class DadosTela {

  Sexos: string[] = ["Masculino", "Feminino"];
  EstadosFederacao: string[] = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];
  TiposInscricao: string[] = ['Participante', 'Participante/Trabalhador', 'Trabalhador'];

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
  centroEspirita: string;
  dataInicioEvento: Date;
  observacoes: string;
  celular: string;
  nomeCracha: string;

  formaEscolha: EnumApresentacaoAtividades;
  oficinasEscolhidas: DTOInscricaoOficina;
  salasEscolhidas: DTOInscricaoSalaEstudo;
  departamentoEscolhido: DTOInscricaoDepartamento;
  sarais: DTOSarau[];
  inscricaoSimples: DTOInscricaoSimplificada;
  dormiraEvento: boolean;

  ox: DTOInscricaoOficina;

  pagamento: DTOPagamento;

  constructor() { }

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

  get tipoInscricaoEscolhida(): string {
    return this.tipoInscricao;
  }

  set tipoInscricaoEscolhida(valor: string) {
    this.tipoInscricao = valor;
    if (valor == this.TiposInscricao[0])
      this.formaEscolha = EnumApresentacaoAtividades.ApenasParticipante
    else
      this.formaEscolha = EnumApresentacaoAtividades.PodeEscolher;
  }
}

export class ResultadoAtualizacaoInscricao {
  valido: boolean;
  inscricaoAtualizar: DTOInscricaoAtualizacao;
}
