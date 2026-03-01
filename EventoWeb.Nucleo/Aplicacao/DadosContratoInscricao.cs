using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOContratoInscricao: DTOId
    {
        public string Regulamento { get; set; }
        public string InstrucoesPagamento { get; set; }
        public string PassoAPassoInscricao { get; set; }
    }
}
