export class DTOMensagemEmailInscricao {
  public MensagemInscricaoRegistradaAdulto!: DTOModeloMensagem;
  public MensagemInscricaoConfirmada!: DTOModeloMensagem;
  public MensagemInscricaoCodigoAcessoAcompanhamento!: DTOModeloMensagem;
  public MensagemInscricaoCodigoAcessoCriacao!: DTOModeloMensagem;
  public MensagemInscricaoRegistradaInfantil!: DTOModeloMensagem;
}

export class DTOModeloMensagem {
  Assunto!: string;
  Mensagem!: string;
}
