using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BeghCore
{
    //LifeTime Interfaces
    public interface ITransient { }
    public interface IScoped { }
    public interface ISingleton { }
    public interface ITransient<T> { }
    public interface IScoped<T> { }
    public interface ISingleton<T> { }

    //GUI Auto Start Interface
    public interface IGUIAutoStart;
    public interface ICommandLineAutoStart;
    public interface IAutoStart;

    public enum ApplicationStartMode
    {
        GUIApplicationStartMode = 0,
        CommandLineApplicationStartMode = 1,
    }

    public static class Core
    {
        private static IServiceProvider serviceProvider = default!;

        public static T GetService<T>() where T : notnull => serviceProvider.GetRequiredService<T>();
        public static object GetService(Type serviceType) => serviceProvider.GetRequiredService(serviceType);
        public static void RegisterEventHandler<TMessage>(object recipient, Action<TMessage> handler) where TMessage : class
        {
            WeakReferenceMessenger.Default.Register<TMessage>(recipient, (r, m) => handler(m));
        }
        public static void SendEvent<TMessage>(TMessage message) where TMessage : class
        {
            WeakReferenceMessenger.Default.Send(message);
        }
        public static IEnumerable<Type> GetAssemblyTypes() => AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsClass && !t.IsAbstract);

        public static void CoreInit(ApplicationStartMode applicationStartMode = ApplicationStartMode.GUIApplicationStartMode)
        {
            var services = new ServiceCollection();

            var types = GetAssemblyTypes();

            //Init LifeTime
            foreach (var type in types)
            {
                if (typeof(ITransient).IsAssignableFrom(type)) services.AddTransient(type);
                else if (typeof(IScoped).IsAssignableFrom(type)) services.AddScoped(type);
                else if (typeof(ISingleton).IsAssignableFrom(type)) services.AddSingleton(type);

                foreach (var iface in type.GetInterfaces().Where(i => i.IsGenericType))
                {
                    var genDef = iface.GetGenericTypeDefinition();
                    var serviceType = iface.GetGenericArguments()[0];

                    if (genDef == typeof(ITransient<>)) services.AddTransient(serviceType, type);
                    else if (genDef == typeof(IScoped<>)) services.AddScoped(serviceType, type);
                    else if (genDef == typeof(ISingleton<>)) services.AddSingleton(serviceType, type);
                }
            }

            serviceProvider = services.BuildServiceProvider();

            if (applicationStartMode == ApplicationStartMode.GUIApplicationStartMode)
                foreach (var type in types.Where(t => typeof(IGUIAutoStart).IsAssignableFrom(t)))
                    GetService(type);
            else
                foreach (var type in types.Where(t => typeof(ICommandLineAutoStart).IsAssignableFrom(t)))
                    GetService(type);

            foreach (var type in types.Where(t => typeof(IAutoStart).IsAssignableFrom(t)))
                GetService(type);
        }
    }
}
