using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class MovimentacaoQuarto
    {
        private Quarto mQuarto;
        private IEnumerable<Quarto> mQuartos;

        public MovimentacaoQuarto(Quarto quarto, IEnumerable<Quarto> mQuartos)
        {
            /*if (quarto.EhFamilia)
                throw new ArgumentException("Não é esperado quarto família.");*/

            this.mQuarto = quarto;
            this.mQuartos = mQuartos;
        }

        public void IncluirInscrito(Inscricao inscrito, Boolean ehCoordenador)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");

            /*if (inscrito is InscricaoInfantil)
                throw new ArgumentException("Não se pode incluir uma inscrição de criança.", "inscrito");*/

            if (inscrito is InscricaoParticipante && mQuarto.Sexo != EnumSexoQuarto.Misto && (int)inscrito.Pessoa.Sexo != (int)mQuarto.Sexo)
                throw new ArgumentException("Esta inscrição é de sexo diferente do definido no quarto.", "inscrito");

            var inscritosQuarto = mQuartos.SelectMany(x => x.Inscritos);
            if (inscritosQuarto.Count(x => x.Inscricao == inscrito) > 0)
                throw new ArgumentException("Esta inscrição já está em outro quarto.", "inscrito");

            mQuarto.AdicionarInscrito(inscrito, ehCoordenador);
        }

        public ParaOndeMoverInscricaoQuarto MoverInscrito(Inscricao inscrito)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");

            /*if (inscrito is InscricaoInfantil)
                throw new ArgumentException("Não se pode incluir uma inscrição de criança.", "inscrito");*/ 

            if (inscrito is InscricaoParticipante && (int)inscrito.Pessoa.Sexo != (int)mQuarto.Sexo)
                throw new ArgumentException("Esta inscrição é de sexo diferente do definido no quarto.", "inscrito");

            QuartoInscrito inscricao = null;
            if ((inscricao = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == inscrito)) == null)
                throw new ArgumentException("Esta inscrição não está no quarto.");

            return new ParaOndeMoverInscricaoQuarto(mQuarto, new QuartoInscrito[] { inscricao });
        }

        public void RemoverInscrito(Inscricao inscrito)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");

            /*if (inscrito is InscricaoInfantil)
                throw new ArgumentException("Não se pode excluir uma inscrição de criança.", "inscrito");*/

            QuartoInscrito inscricao = null;
            if ((inscricao = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == inscrito)) == null)
                throw new ArgumentException("Esta inscrição não está no quarto.");

            mQuarto.RemoverInscrito(inscricao);
        }

        public void TornarCoordenador(Inscricao inscrito, bool coordenador)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");

            if (coordenador && inscrito.GetTypeUnproxied() == typeof(InscricaoInfantil))
                throw new ArgumentException("Não se pode tornar uma criança coordenadora de quarto.", "inscrito");

            QuartoInscrito inscricao = null;
            if ((inscricao = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == inscrito)) == null)
                throw new ArgumentException("Esta inscrição não está no quarto.");

            inscricao.EhCoordenador = coordenador;
        }
    }

    /*public class MovimentacaoQuartoFamilia
    {
        private Quarto mQuarto;
        private IEnumerable<Quarto> mQuartos;

        public MovimentacaoQuartoFamilia(Quarto quarto, IEnumerable<Quarto> mQuartos)
        {
            if (!quarto.EhFamilia)
                throw new ArgumentException("É esperado quarto família.");

            this.mQuarto = quarto;
            this.mQuartos = mQuartos;
        }

        public void IncluirInscrito(InscricaoInfantil inscritoCrianca)
        {
            if (inscritoCrianca == null)
                throw new ArgumentNullException("inscritoCrianca", "Não pode ser nulo.");

            if (inscritoCrianca.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta criança é de outro evento.", "inscritoCrianca");
            
            if (mQuarto.Sexo != SexoQuarto.Misto && (int)inscritoCrianca.Pessoa.Sexo != (int)mQuarto.Sexo)
                throw new ArgumentException("Esta criança é de sexo diferente do definido no quarto.", "inscritoCrianca");

            var inscritosQuarto = mQuartos.SelectMany(x => x.Inscritos);
            if (inscritosQuarto.Count(x=>x.Inscricao == inscritoCrianca) > 0)
                throw new ArgumentException("Esta criança já está em outro quarto.", "inscritoCrianca");

            Inscricao responsavelCrianca = null;
            if ((mQuarto.Sexo == SexoQuarto.Misto || 
                 mQuarto.Sexo != SexoQuarto.Misto && inscritoCrianca.InscricaoResponsavel1.Pessoa.Sexo == inscritoCrianca.Pessoa.Sexo) && 
                inscritoCrianca.InscricaoResponsavel1.DormeEvento &&
                inscritosQuarto.Count(x => x.Inscricao == inscritoCrianca.InscricaoResponsavel1) == 0)
                responsavelCrianca = inscritoCrianca.InscricaoResponsavel1;
            else 
            if (inscritoCrianca.InscricaoResponsavel2 != null && 
                (mQuarto.Sexo == SexoQuarto.Misto ||
                 mQuarto.Sexo != SexoQuarto.Misto &&
                 inscritoCrianca.InscricaoResponsavel2.Pessoa.Sexo == inscritoCrianca.Pessoa.Sexo) && 
                inscritoCrianca.InscricaoResponsavel2.DormeEvento &&
                inscritosQuarto.Count(x => x.Inscricao == inscritoCrianca.InscricaoResponsavel2) == 0)
                responsavelCrianca = inscritoCrianca.InscricaoResponsavel2;

            if (responsavelCrianca == null)
                throw new ArgumentException("Esta criança não tem responsável para dormir com ela no quarto.");

            if (mQuarto.Capacidade != null && mQuarto.Inscritos.Count() + 2 > mQuarto.Capacidade.Value)
                throw new ArgumentException("Não há vaga para incluir a criança e o responsável.");

            mQuarto.AdicionarInscrito(inscritoCrianca);
            mQuarto.AdicionarInscrito(responsavelCrianca);
        }

        public ParaOndeMoverParticipanteQuarto MoverInscrito(Inscricao inscrito)
        {
            return new ParaOndeMoverParticipanteQuarto(mQuarto, ObterCriancaEResponsavel(inscrito));
        }

        public void RemoverInscrito(Inscricao inscrito)
        {
            var inscricoes = ObterCriancaEResponsavel(inscrito);

            foreach(var quartoInscricao in inscricoes)
                mQuarto.RemoverInscrito(quartoInscricao);
        }

        private QuartoInscrito[] ObterCriancaEResponsavel(Inscricao inscrito)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");

            QuartoInscrito inscricao = null;
            if ((inscricao = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == inscrito)) == null)
                throw new ArgumentException("Esta inscrição não está no quarto.");

            QuartoInscrito[] quartoInscricoes = new QuartoInscrito[2];
            quartoInscricoes[0] = inscricao;

            if (inscrito is InscricaoInfantil)
            {
                var crianca = (InscricaoInfantil)inscrito;
                quartoInscricoes[1] = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == crianca.InscricaoResponsavel1 ||
                    x.Inscricao == crianca.InscricaoResponsavel2);
            }
            else
            {
                quartoInscricoes[1] = mQuarto.Inscritos.FirstOrDefault(x =>
                    x.Inscricao is InscricaoInfantil &&
                    (((InscricaoInfantil)x.Inscricao).InscricaoResponsavel1 == inscrito ||
                     ((InscricaoInfantil)x.Inscricao).InscricaoResponsavel2 == inscrito));
            }

            return quartoInscricoes;
        }

        public void TornarCoordenador(Inscricao inscrito, bool coordenador)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito", "Não pode ser nulo.");

            if (inscrito.Evento != mQuarto.Evento)
                throw new ArgumentException("Esta inscrição é de outro evento.", "inscrito");
            
            if (EhInscricaoInfantil(inscrito))
                throw new ArgumentException("Uma criança não pode ser coordenadora de quarto.");

            QuartoInscrito inscricao = null;
            if ((inscricao = mQuarto.Inscritos.FirstOrDefault(x => x.Inscricao == inscrito)) == null)
                throw new ArgumentException("Esta inscrição não está no quarto.");

            inscricao.EhCoordenador = coordenador;
        }

        private bool EhInscricaoInfantil(Inscricao inscrito)
        {
            try
            {
                var convertido = (InscricaoInfantil)inscrito;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }*/

    public class ParaOndeMoverInscricaoQuarto 
    {
        private QuartoInscrito[] mInscricoes;
        private Quarto mQuartoOrigem;

        public ParaOndeMoverInscricaoQuarto(Quarto quartoOrigem, QuartoInscrito[] inscricoes)
        {
            mInscricoes = inscricoes;
            mQuartoOrigem = quartoOrigem;
        }

        public void Para(Quarto onde, Boolean ehCoordenador)
        {
            if (onde == null)
                throw new ArgumentNullException("onde", "Não pode ser nulo.");

            if (onde == mQuartoOrigem)
                throw new ArgumentException("Não se pode mover uma inscrição para o mesmo quarto.");

            if (onde.Capacidade != null && onde.Capacidade.Value + mInscricoes.Length > onde.Capacidade.Value)
                throw new ArgumentException("Não há vagas neste quarto.");

            foreach (var inscrito in mInscricoes)
            {
                onde.AdicionarInscrito(inscrito.Inscricao, ehCoordenador);
                mQuartoOrigem.RemoverInscrito(inscrito);
            }
        }
    }

    public class DivisaoManualInscricaoPorQuarto
    {
        private Evento mEvento;
        private IList<Quarto> mQuartos;

        public DivisaoManualInscricaoPorQuarto(Evento evento, AQuartos quartos)
        {
            mEvento = evento;
            mQuartos = quartos.ListarTodosQuartosPorEventoComParticipantes(evento.Id);
 
        }

        public MovimentacaoQuarto Quarto(Quarto quarto)
        {
            ValidarQuarto(quarto);
            return new MovimentacaoQuarto(quarto, mQuartos);
        }

        /*public MovimentacaoQuartoFamilia QuartoFamilia(Quarto quarto)
        {
            ValidarQuarto(quarto);
            return new MovimentacaoQuartoFamilia(quarto, mQuartos);
        }*/

        private void ValidarQuarto(Quarto quarto)
        {
            if (quarto == null)
                throw new ArgumentNullException("quarto", "Quarto não pode ser vazio.");

            if (quarto.Evento != mEvento)
                throw new ArgumentException("Este Quarto é de outro evento.", "quarto");
        }
    }
}
