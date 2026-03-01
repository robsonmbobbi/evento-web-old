using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public interface IParaOndeMover<T>
    {
        void Para(T onde);
    }

    public interface IMovimentacaoParticipante<T>
    {
        void IncluirParticipante(InscricaoParticipante participante);
        IParaOndeMover<T> MoverParticipante(InscricaoParticipante participante);
        void RemoverParticipante(InscricaoParticipante participante);
    }
}
