using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorContratoInscricao
    {
        public static DTOContratoInscricao Converter(this ContratoInscricao contrato)
        {
            if (contrato == null)
                return null;
            else
                return new DTOContratoInscricao
                {
                    Id = contrato.Id,
                    InstrucoesPagamento = contrato.InstrucoesPagamento,
                    PassoAPassoInscricao = contrato.PassoAPassoInscricao,
                    Regulamento = contrato.Regulamento
                };
        }
    }
}
