using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumSexoQuarto { Masculino, Feminino, Misto }

    public class QuartoInscrito: Entidade
    {
        private Inscricao m_Inscricao;
        private Quarto m_Quarto;

        public QuartoInscrito(Quarto quarto, Inscricao inscrito, Boolean ehCoordenador)
        {
            m_Quarto = quarto ?? throw new ExcecaoNegocioAtributo("QuartoInscrito", "quarto", "Quarto não pode ser nulo");
            m_Inscricao = inscrito ?? throw new ExcecaoNegocioAtributo("QuartoInscrito", "inscrito", "Inscrito não pode ser nulo");
            EhCoordenador = ehCoordenador;
        }

        protected QuartoInscrito() { }

        public virtual Quarto Quarto { get { return m_Quarto; } }
        public virtual Inscricao Inscricao { get { return m_Inscricao; } }
        public virtual Boolean EhCoordenador { get; set; }
    }

    public class Quarto: Entidade
    {
        private String m_Nome;
        private Evento m_Evento;
        private int? m_Capacidade;
        private IList<QuartoInscrito> m_Inscritos;

        public Quarto(Evento evento, string nome, Boolean ehFamilia, EnumSexoQuarto sexo)
        {
            Nome = nome;
            Evento = evento;
            Sexo = sexo;
            AtribuirSexoEEhFamilia(ehFamilia, sexo);
            m_Inscritos = new List<QuartoInscrito>();
        }

        protected Quarto() { }

        public virtual Evento Evento
        {
            get { return m_Evento; }
            protected set
            {
                if (value == null)
                    throw new ExcecaoNegocioAtributo("Quarto", "Evento", "Evento não pode ser nulo");

                if (!value.TemDormitorios)
                    throw new ExcecaoNegocioAtributo("Quarto", "Evento", "Este evento não está configurado para ter Quartos.");

                m_Evento = value;
            }
        }

        public virtual string Nome 
        {
            get { return m_Nome; }
            set
            {
                if (value == null)
                    throw new ExcecaoNegocioAtributo("Quarto", "Nome", "Nome não pode ser nulo.");

                if (value == "")
                    throw new ExcecaoNegocioAtributo("Quarto", "Nome", "Nome não pode ser vazio.");

                m_Nome = value;
            }
        }

        public virtual int? Capacidade
        {
            get { return m_Capacidade; }
            set
            {
                if (value != null && value <= 0)
                    throw new ExcecaoNegocioAtributo("Quarto", "Capacidade", "A capacidade informada deve ser maior que zero.");

                m_Capacidade = value;
            }
        }

        public virtual Boolean EhFamilia { get; protected set; }

        public virtual EnumSexoQuarto Sexo { get; protected set; }


        public virtual IEnumerable<QuartoInscrito> Inscritos { get { return m_Inscritos; } }

        public virtual void AtribuirSexoEEhFamilia(bool ehFamilia, EnumSexoQuarto sexo)
        {
            if (!ehFamilia && sexo == EnumSexoQuarto.Misto)
                throw new ExcecaoNegocio("Quarto", "Somente quarto família pode ter o sexo misto.");

            EhFamilia = ehFamilia;
            Sexo = sexo;
        }

        public virtual void AdicionarInscrito(Inscricao inscrito, Boolean ehCoordenador = false)
        {
            if (inscrito.Evento != Evento)
                throw new ExcecaoNegocio("Quarto", "A inscrição não é do mesmo evento do quarto.");

            if (inscrito is InscricaoParticipante && this.Sexo != EnumSexoQuarto.Misto && (int)inscrito.Pessoa.Sexo != (int)this.Sexo)
                throw new ExcecaoNegocio("Quarto", "O inscrito não é do mesmo sexo definido para o quarto.");

            if (Capacidade != null && m_Inscritos.Count == Capacidade.Value)
                throw new ExcecaoNegocio("Quarto", "Nâo é possível incluir mais participantes neste quarto.");


            m_Inscritos.Add(new QuartoInscrito(this, inscrito, ehCoordenador));
        }

        public virtual void RemoverInscrito(QuartoInscrito inscrito)
        {
            if (!m_Inscritos.Remove(inscrito))
                throw new ExcecaoNegocio("Quarto", "Nâo existe este inscrito no quarto.");
        }

        public virtual void RemoverTodosInscritos()
        {
            m_Inscritos.Clear();
        }
    }
}
