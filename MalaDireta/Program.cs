// See https://aka.ms/new-console-template for more information
using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Persistencia;
using MalaDireta;

var anexos = new List<AnexoEmail>()
{
    new("Mensagem Psicografada de Marco Prisco - CEOMG 2025.pdf", Convert.ToBase64String(File.ReadAllBytes("D:\\Mensagem Psicografada de Marco Prisco - CEOMG 2025.pdf"))),
    new("Mensagem Evangelização CEOMG 2025.pdf", Convert.ToBase64String(File.ReadAllBytes("D:\\Mensagem Evangelização CEOMG 2025.pdf"))),
    //new("Caderninho.pdf", Convert.ToBase64String(File.ReadAllBytes("D:\\Caderninho.pdf"))),
};

var sessionFactory = new ConfiguracaoNHibernate().GerarFabricaSessao();

Task.WhenAll(
    [
        //new EnvioEmail(sessionFactory).Enviar(14, anexos),
        new EnvioWhatsApp(sessionFactory).Enviar(14, anexos)
    ]
).Wait();

/*var contexto = new Contexto(sessionFactory.OpenSession());

contexto.IniciarTransacao();
try
{
    var confEmail = contexto.RepositorioConfiguracoesEmail.Obter(13);
    var inscricoes = contexto.RepositorioInscricoes.ListarTodasPorEventoESituacao(14, EventoWeb.Nucleo.Negocio.Entidades.EnumSituacaoInscricao.Aceita);

    var base64Cardapio = Convert.ToBase64String(File.ReadAllBytes("D:\\Cardapio.pdf"));
    var base64Cronograma = Convert.ToBase64String(File.ReadAllBytes("D:\\Cronograma.pdf"));

    var servicoEmail = new ServicoEmail()
    {
        Configuracao = confEmail,
    };


    foreach(var inscrito  in inscricoes)
    {
        servicoEmail.Enviar(
            new Email()
            {
                Assunto = "42º CEOMG - Encontro chegando...",
                Conteudo = "<p>Olá! Minhas companheiras e companheiros de vida💓🌻</p>\r\n<p>Nossa deliciosa 42º CEOMG está chegando! Que delícia e que saudades!</p>\r\n<p>Para você passar o encontro com mais tranquilidade vamos deixar aqui alguns lembretes importantes!!!!</p>\r\n<p>Leve na sua mala!</p>\r\n<p>💓 Afeto<br>▶️ Repelente<br>▶️ Desodorante<br>▶️ Escova de Dentes<br>▶️ Sabonete<br>▶️ Colchão<br>▶️ Roupa de cama<br>▶️ Travesseiro<br>▶️ Se seu colchão for de ar, um pano para colocar embaixo ( isso evita furos)<br>▶️ Se você sente frio, leve uma \"brusinha\" meio termo.<br>▶️ Garrafinha de água (para ir enchendo pelos bebedouros da universidade)<br>😁Bom humor</p>\r\n<p>🗺️ &nbsp;Endereço da UNIFOR: Rua Doutor Arnaldo Sena, 328, bairro Del Rey.<br>🚙 Os veículos poderão estacionar no próprio pátio de estacionamento da universidade, na entrada principal.</p>\r\n<p>🗓️ Segue anexo o cronograma do encontro, bem como o cardápio da alimentação que será oferecida. Caso você tenha necessidade de algum alimento especial, que não está contemplado dentro da alimentação oferecida, sugerimos que você leve o que precisa.</p>\r\n<p>Que tenhamos um excelente encontro, que será temperado pela vibração do teu coração, que o Cristo permaneça vivo a nos iluminar e até breve!💓💓💓🌻</p>\r\n<p>Abraços quentinhos, coordenação e equipe de secretaria CEOMG!</p>",
                Endereco = inscrito.Pessoa.Email,
                Anexos = new List<AnexoEmail>() 
                { 
                    new("Cardapio.pdf", base64Cardapio),
                    new("Cronograma.pdf", base64Cronograma)
                }
            }
        );

        Console.WriteLine($"Enviado para {inscrito.Pessoa.Nome}");
    }

    contexto.SalvarTransacao();
}
catch(Exception ex)
{
    contexto.CancelarTransacao();
    Console.WriteLine(ex.Message);
}*/

Console.WriteLine("Terminado. Aperte qualquer tecla para fechar o programa");
Console.ReadLine();

