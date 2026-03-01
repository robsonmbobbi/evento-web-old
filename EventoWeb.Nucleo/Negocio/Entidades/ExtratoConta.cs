using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ExtratoConta
    {
        public int IdConta { get; set; }
        public String DescricaoConta { get; set; }
        public Decimal SaldoAnterior { get; set; }
        public Decimal SaldoFinal { get; set; }
        public IList<Transacao> Transacoes { get; set; }
    }
}
