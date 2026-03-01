using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOQuarto : DTOId
    {
        public string Nome { get; set; }
        public bool EhFamilia { get; set; }
        public EnumSexoQuarto Sexo { get; set; }
        public int? Capacidade { get; set; }
    }

    public class DTODivisaoQuarto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool EhFamilia { get; set; }
        public EnumSexoQuarto Sexo { get; set; }
        public int? Capacidade { get; set; }
        public IEnumerable<DTOBasicoInscricaoResp> Coordenadores { get; set; }
        public IEnumerable<DTOBasicoInscricaoResp> Participantes { get; set; }
    }
}
