using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Persistencia;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Net;

namespace EventoWeb.WS.Secretaria
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuracao = new ConfiguracaoAutorizacao();
            services.AddSingleton(configuracao);

            services.AddSingleton<ISessionFactory>(new ConfiguracaoNHibernate().GerarFabricaSessao());
            services.AddScoped<IContexto>((provider) => {
                var factory = provider.GetService<ISessionFactory>();
                return new Contexto(factory.OpenSession());
            });
            services.AddTransient<AServicoEmail>(provider => new ServicoEmail());
            services.AddTransient<AServicoWhatsapp>(provider => new ServicoWhatsapp());
            services.AddTransient<GeracaoMensagemEmailRazor>();
            services.AddTransient<GeracaoMensagemSand>();
            services.AddTransient<IList<IComunicacao>>(provider =>
            {
                return
                [
                    /*new AppEmailMsgPadrao(
                        provider.GetService<IContexto>(), 
                        provider.GetService<AServicoEmail>(), 
                        provider.GetService<GeracaoMensagemEmailRazor>()
                    ),*/
                    new AppWhatsappMsgPadrao(
                        provider.GetService<IContexto>(),
                        provider.GetService<AServicoWhatsapp>(),
                        provider.GetService<GeracaoMensagemSand>()
                    )
                ];
            });
            services.AddTransient<AppComunicacao>();
            services.AddTransient<AppInscricoes>();

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = configuracao.ChaveEmissor;
                paramsValidation.ValidAudience = configuracao.Publico;
                paramsValidation.ValidIssuer = configuracao.Emissor;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services
                .AddMvc()
                .AddNewtonsoftJson(opcoes =>
                {
                    opcoes.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opcoes.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    opcoes.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureExceptionHandler();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //app.UseMvc();

            app.UseRouting();

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed(_ => true)
               .AllowCredentials()
           );

            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                //    spa.UseAngularCliServer(npmScript: "start");
                //}
            });

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
            );
            app.UseHttpsRedirection();

            //app.UseMvc();
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(ProcessarErro(contextFeature.Error)));
                    }
                });
            });
        }

        private static Object ProcessarErro(Exception erro)
        {
            if (erro == null)
                return null;
            else
                return new
                {
                    ClasseErro = erro.GetType().Name,
                    MensagemErro = erro.Message,
                    ErroInterno = ProcessarErro(erro.InnerException),
                    PilhaExecucao = erro.StackTrace
                };
        }
    }
}
