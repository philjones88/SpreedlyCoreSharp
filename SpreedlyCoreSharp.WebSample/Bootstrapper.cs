using System.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace SpreedlyCoreSharp.WebSample
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register<ICoreService>(new CoreService(
                    ConfigurationManager.AppSettings["APILogin"],
                    ConfigurationManager.AppSettings["APISecret"],
                    ConfigurationManager.AppSettings["GatewayToken"])
            );
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content"));
        }
    }
}