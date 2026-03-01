using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorAtividades
    {
        public static DTOSalaEstudo Converter(this SalaEstudo sala)
        {
            return new DTOSalaEstudo
            {
                DeveSerParNumeroTotalParticipantes = sala.DeveSerParNumeroTotalParticipantes,
                Id = sala.Id,
                IdadeMaxima = sala.FaixaEtaria?.IdadeMax,
                IdadeMinima = sala.FaixaEtaria?.IdadeMin,
                Nome = sala.Nome
            };
        }
        public static DTOOficina Converter(this Oficina oficina)
        {
            return new DTOOficina
            {
                Id = oficina.Id,
                Nome = oficina.Nome,
                DeveSerParNumeroTotalParticipantes = oficina.DeveSerParNumeroTotalParticipantes,
                NumeroTotalParticipantes = oficina.NumeroTotalParticipantes
            };
        }
        public static DTODepartamento Converter(this Departamento departamento)
        {
            return new DTODepartamento
            {
                Id = departamento.Id,
                Nome = departamento.Nome
            };
        }

        public static DTOSarau Converter(this ApresentacaoSarau sarau)
        {
            var dto = new DTOSarau();
            dto.Converter(sarau);
            return dto;
        }

        public static DTOSarauCodigo ConverterComCodigo(this ApresentacaoSarau sarau)
        {
            var dto = new DTOSarauCodigo();
            dto.Converter(sarau);
            return dto;
        }

        private static DTOSarau Converter(this DTOSarau dto, ApresentacaoSarau sarau)
        {
            dto.DuracaoMin = sarau.DuracaoMin;
            dto.Id = sarau.Id;
            dto.Participantes = sarau.Inscritos.Select(y => y.ConverterSimplificada()).ToList();
            dto.Tipo = sarau.Tipo;

            return dto;
        }
    }
}
  
