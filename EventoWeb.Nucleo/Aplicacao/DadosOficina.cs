using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOOficina : DTOId
    {
        public string Nome { get; set; }
        public bool DeveSerParNumeroTotalParticipantes { get; set; }
        public int? NumeroTotalParticipantes { get; set; }
    }

    public class DTODivisaoOficina
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public IEnumerable<DTOBasicoInscricao> Coordenadores { get; set; }
        public IEnumerable<DTOBasicoInscricao> Participantes { get; set; }
    }
}
