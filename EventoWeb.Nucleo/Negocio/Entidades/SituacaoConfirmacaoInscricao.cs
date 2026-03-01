using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SituacaoEnvioEmail { Enviado, ErroEnvio }
    
    public class SituacaoConfirmacaoInscricao: Entidade
    {
        private Inscricao mInscricao;
        private DateTime mDataHora;
        private SituacaoEnvioEmail mSituacao;
        private String mObservacao;

        public SituacaoConfirmacaoInscricao(Inscricao inscricao, DateTime dataHora, 
            SituacaoEnvioEmail situacao, String observacao = null)
        {
            if (inscricao == null)
                throw new ArgumentNullException("Inscricao", "A inscrição não pode ser nula.");

            mInscricao = inscricao;
            mDataHora = dataHora;
            mSituacao = situacao;
            mObservacao = observacao;
        }

        protected SituacaoConfirmacaoInscricao() { }

        public virtual Inscricao Inscricao { get { return mInscricao; } }
        public virtual DateTime DataHora { get { return mDataHora; } }
        public virtual SituacaoEnvioEmail Situacao { get { return mSituacao; } }
        public virtual String Observacao { get { return mObservacao; } }
    }
}
