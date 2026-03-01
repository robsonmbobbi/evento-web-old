export class OperacoesImagem {
  static obterImagemOuSemImagem(imagemBase64: string | null): string {
    if (imagemBase64 == null || imagemBase64.trim().length == 0)
      return 'assets/semimagem.jpg';
    else if (imagemBase64.indexOf('data:image/jpeg;base64,') != -1)
      return imagemBase64;
    else
      return 'data:image/jpeg;base64,' + imagemBase64;
  }
}
