using BeghShare.Core.Services;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;

namespace BeghShare.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseBackedAttribute : OverrideFieldOrPropertyAspect
    {
        public override dynamic OverrideProperty
        {
            get
            {
                if (meta.Target.FieldOrProperty.Writeability == Writeability.ConstructorOnly)
                {
                    var cach = meta.Proceed();
                    if (cach == null)
                        cach = GetService<DatabaseService>().Get(meta.Target.FieldOrProperty.Type.ToType(), meta.Target.FieldOrProperty.Name);
                    return cach;
                }
                else
                    return GetService<DatabaseService>().Get(meta.Target.FieldOrProperty.Type.ToType(), meta.Target.FieldOrProperty.Name);
            }
            set
            {
                if (meta.Target.FieldOrProperty.Writeability == Writeability.ConstructorOnly)
                    throw new NotSupportedException();
                else
                    GetService<DatabaseService>().Set(meta.Target.FieldOrProperty.Name, value);
            }
        }
    }
}
