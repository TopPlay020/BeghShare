using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Fabrics;

namespace BeghShare.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MsgEventHandlerAttribute : Attribute
    {
        public string Header { get; set; }
        public MsgEventHandlerAttribute(string Header)
        {
            this.Header = Header;
        }
    }
    public class MsgEventHandlerAspect : TypeAspect
    {
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            var eventHandlerMethods = builder.Target.Methods
                .Where(m => m.Attributes.Any(a => a.Type.Name == typeof(MsgEventHandlerAttribute).Name))
                .ToList();

            foreach (var method in eventHandlerMethods)
            {
                var header = method.Attributes.First(a => a.Type.Name == nameof(MsgEventHandlerAttribute)).ConstructorArguments[0].Value as string;
                var methodName = method.Name;

                builder.AddInitializer(
                    StatementFactory.Parse($"BeghCore.Core.GetService<MsgEventService>().RegisterEventHandler(this,{methodName},\"{header}\");"),
                    InitializerKind.BeforeInstanceConstructor
                );

            }
        }
    }

    public class MsgEventHandlerFabric : ProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender
                .SelectMany(compilation => compilation.AllTypes)
                .Where(type => type.Methods.Any(m =>
                    m.Attributes.Any(a => a.Type.Name == nameof(MsgEventHandlerAttribute))))
                .AddAspectIfEligible<MsgEventHandlerAspect>();
        }
    }
}
