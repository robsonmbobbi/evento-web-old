using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SexoPessoa { Masculino, Feminino }

    public class Pessoa: PessoaComum
    {
        private DateTime m_DataNascimento;
        private readonly Endereco m_Endereco;

        protected Pessoa()
        {
        }

        public Pessoa(string nome, Endereco endereco, DateTime dataNascimento, SexoPessoa sexo, string email)
            :base(nome)
        {
            DataNascimento = dataNascimento;

            if (string.IsNullOrWhiteSpace(email))
                throw new ExcecaoNegocioAtributo("Pessoa", "Email", "Email esta vazio");
            Email = email;
                        
            m_Endereco = endereco ?? throw new ExcecaoNegocioAtributo("Pessoa", "endereco", "Endereço não informado");
            Sexo = sexo;
        }

        public virtual DateTime DataNascimento
        {
            get
            {
                return m_DataNascimento;
            }

            set
            {
                DateTime dataHoje = DateTime.Now;
                dataHoje = dataHoje.AddMilliseconds(dataHoje.Millisecond * -1);
                dataHoje = dataHoje.AddSeconds(dataHoje.Second * -1);

                if (value > dataHoje)
                    throw new ArgumentException("Data deve ser menor que a data atual do sistema.", "DataNascimento");

                m_DataNascimento = value;
            }
        }

        public virtual Endereco Endereco { get { return m_Endereco; } }

        public virtual Boolean EhVegetariano { get; set; }

        public virtual Boolean EhDiabetico { get; set; }

        public virtual Boolean UsaAdocanteDiariamente { get; set; }

        public virtual String AlergiaAlimentos { get; set; }

        public virtual SexoPessoa Sexo { get; set; }              

        public virtual int CalcularIdadeEmAnos(DateTime dataAtual)
        {
            if (dataAtual < m_DataNascimento)
                throw new ArgumentException("A data de comparação deve ser maior ou igual a do nascimento.", "dataAtual");

            return (dataAtual.Year - m_DataNascimento.Year - 1) +
                    (((dataAtual.Month > m_DataNascimento.Month) ||
                    ((dataAtual.Month == m_DataNascimento.Month) && (dataAtual.Day >= m_DataNascimento.Day))) ? 1 : 0);
        }
    }
}
