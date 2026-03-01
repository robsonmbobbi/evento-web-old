using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class InscritosAfrac
    {
        private Oficina mAfrac;
        private InscricaoParticipante[] mInscritos;

        public InscritosAfrac(Oficina afrac, InscricaoParticipante[] inscricao)
        {
            mAfrac = afrac;
            mInscritos = inscricao;
        }

        public Oficina Afrac { get { return mAfrac; } }
        public IEnumerable<InscricaoParticipante> Inscritos { get { return mInscritos; } }
    }
}
