using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class SaldoAtualConta
    {
        public int Id { get; set; }
        public String Descricao { get; set; }
        public Decimal SaldoAtual { get; set; }
    }

    public class ParcelaAberta
    {
        public int Id { get; set; }
        public String DescricaoTitulo { get; set; }
        public String Quem { get; set; }
        public DateTime DataRegistrar { get; set; }
        public Decimal Valor { get; set; }
        public EnumTipoTransacao TipoTransacao { get; set; }
    }

    public class VisaoGeralFinanceiro
    {
        public IEnumerable<SaldoAtualConta> Contas { get; set; }
        public IEnumerable<ParcelaAberta> TitulosPagar { get; set; }
        public IEnumerable<ParcelaAberta> TitulosReceber { get; set; }
    }
}
