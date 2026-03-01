using EventoWeb.Nucleo.Negocio.Excecoes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EventoWeb.WS.Secretaria
{
    public class FiltroExcecao : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var excecao = context.Exception;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            using(var writer = new StreamWriter(response.Body))
            {
                new JsonSerializer().Serialize(writer, GerarObjeto(excecao));
                writer.FlushAsync().ConfigureAwait(false);
            }
        }

        private object GerarObjeto(Exception excecao)
        {
            if (excecao is ExcecaoAPI erroApi)
                return new
                {
                    erroApi.GetType().Name,
                    erroApi.API,
                    erroApi.Message,
                    erroApi.StackTrace
                };
            else if (excecao is ExcecaoNegocioAtributo erroNegAtributo)
                return new
                {
                    erroNegAtributo.GetType().Name,
                    erroNegAtributo.Atributo,
                    erroNegAtributo.Classe,
                    erroNegAtributo.Message,
                    erroNegAtributo.StackTrace
                };
            else if (excecao is ExcecaoNegocioRepositorio erroNegRepositorio)
                return new
                {
                    erroNegRepositorio.GetType().Name,
                    erroNegRepositorio.Classe,
                    erroNegRepositorio.Message,
                    erroNegRepositorio.StackTrace
                };
            else
                return new
                {
                    excecao.GetType().Name,
                    excecao.Message,
                    excecao.StackTrace
                };
        }
    }
}
