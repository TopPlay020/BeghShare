using BeghShare.Core.Services;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace BeghShare.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseBackedAttribute : FieldOrPropertyAspect
    {
        public override void BuildAspect(IAspectBuilder<IFieldOrProperty> builder)
        {
            builder.OverrideAccessors(
                getTemplate: nameof(GetValue),
                setTemplate: nameof(SetValue), // or nameof(SetValue) if you have one
                args: new { T = builder.Target.Type } // Maps the metadata Type to the template's T
            );
        }

        [Template]
        public dynamic GetValue<[CompileTime] T>()
        {
            var fieldName = meta.Target.FieldOrProperty.Name;
            return GetService<DatabaseService>().Get<T>(fieldName);
        }

        [Template]
        public void SetValue(dynamic value)
        {
            var fieldName = meta.Target.FieldOrProperty.Name;

            BeghCore.Core.GetService<BeghShare.Core.Services.DatabaseService>().Set(fieldName, value);
        }
    }
}
