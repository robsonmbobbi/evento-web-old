using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEvento: AppBase
    {
        public AppInscOnlineEvento(IContexto contexto)
            : base(contexto) { }

        public DTOEventoMinimo ObterPorIdDisponivelInscricaoOnline(int id)
        {
            DTOEventoMinimo dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                if (evento != null &&
                   evento.PeriodoInscricaoOnLine.DataInicial <= DateTime.Now &&
                   evento.PeriodoInscricaoOnLine.DataFinal >= DateTime.Now)
                    dtoEvento = evento.ConverterMinimo();
            });

            return dtoEvento;
        }

        public IList<DTOEventoMinimo> ListarEventosDisponiveisInscricaoOnline()
        {
            IList<DTOEventoMinimo> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventosEmPeriodoInscricaoOnline(DateTime.Now);
                dtoEventos = eventos.Select(x => x.ConverterMinimo()).ToList();
            });

            return dtoEventos;
        }

        public DTOEventoCompletoInscricao ObterPorIdCompleto(int id)
        {
            DTOEventoCompletoInscricao dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                dtoEvento = evento?.ConverterParaInsOnLine();
                if (dtoEvento != null)
                {
                    dtoEvento.Departamentos = Contexto.RepositorioDepartamentos.ListarTodosPorEvento(id)
                        .Select(x => x.Converter())
                        .ToList();
                    dtoEvento.Oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(id)
                        .Select(x => x.Converter())
                        .ToList();
                    dtoEvento.SalasEstudo = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(id)
                        .Select(x => x.Converter())
                        .ToList();
                }                
            });

            return dtoEvento;
        }
    }
}
