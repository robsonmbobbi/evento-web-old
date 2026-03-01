using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorQuarto
    {
        public static DTOQuarto Converter(this Quarto quarto)
        {
            if (quarto == null)
                return null;
            else
                return new DTOQuarto
                {
                    Capacidade = quarto.Capacidade,
                    EhFamilia = quarto.EhFamilia,
                    Id = quarto.Id,
                    Nome = quarto.Nome,
                    Sexo = quarto.Sexo
                };
        }
    }
}
