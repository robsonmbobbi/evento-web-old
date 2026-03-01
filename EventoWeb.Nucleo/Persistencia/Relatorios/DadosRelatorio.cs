using System;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class DTORelDivisao
    {
        public int idDivisao { get; set; }
        public String nomeDivisao { get; set; }
        public String descricaoDivisao { get; set; }
        public String coordenadores { get; set; }
        public String nomeInscrito { get; set; }
        public int totalParticipantes { get; set; }
    }

    public class DTORelApresentacaoSarau
    {
        public String nomeEvento { get; set; }
        public int tempoSarauMin { get; set; }
        public int idApresentacao { get; set; }
        public String tipoApresentacao { get; set; }
        public int duracaoApresentacaoMin { get; set; }
        public String inscritos { get; set; }
        public int idEvento { get; set; }
    }
}
