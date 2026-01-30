using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Fabrics;
using System;
using System.Linq;

namespace BeghCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandlerAttribute : Attribute { }

    public class EventHandlerAspect : TypeAspect
    {
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            var eventHandlerMethods = builder.Target.Methods
                .Where(m => m.Attributes.Any(a => a.Type.Name == typeof(EventHandlerAttribute).Name))
                .ToList();

            foreach (var method in eventHandlerMethods)
            {
                var eventTypeDisplay = method.Parameters.First().Type.ToDisplayString();
                var methodName = method.Name;

                builder.AddInitializer(
                    StatementFactory.Parse($"Core.RegisterEventHandler<{eventTypeDisplay}>(this, {methodName});"),
                    InitializerKind.BeforeInstanceConstructor
                );

            }
        }
    }

    public class EventHandlerFabric : TransitiveProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender
                .SelectMany(compilation => compilation.AllTypes)
                .Where(type => type.Methods.Any(m =>
                    m.Attributes.Any(a => a.Type.Name == nameof(EventHandlerAttribute))))
                .AddAspectIfEligible<EventHandlerAspect>();
        }
    }
}