using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

public class MsgEventService : ISingleton, IAutoStart
{
    private class EventHandlerEntry
    {
        public required WeakReference Target { get; set; }
        public required Action<string, IPAddress> Action { get; set; }
    }

    private readonly Dictionary<string, List<EventHandlerEntry>> _handlers = new();
    private List<string> _eventHeaders = new();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public void RegisterEventHandler(object instance, Action<string, IPAddress> action, string msgHeader)
    {
        _lock.EnterWriteLock();
        try
        {
            if (!_handlers.ContainsKey(msgHeader))
                _handlers[msgHeader] = new List<EventHandlerEntry>();

            _handlers[msgHeader].Add(new EventHandlerEntry
            {
                Target = new WeakReference(instance),
                Action = action
            });
            _eventHeaders.Add(msgHeader);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void RaiseEvent(string msgHeader, string data, IPAddress ip)
    {
        List<EventHandlerEntry> handlers;

        _lock.EnterReadLock();
        try
        {
            if (!_handlers.ContainsKey(msgHeader))
                return;

            handlers = new List<EventHandlerEntry>(_handlers[msgHeader]);
        }
        finally
        {
            _lock.ExitReadLock();
        }

        // Invoke outside the lock - non-blocking!
        foreach (var handler in handlers)
        {
            if (handler.Target.IsAlive)
            {
                handler.Action(data, ip);
            }
        }

        // Cleanup dead references
        _lock.EnterWriteLock();
        try
        {
            if (_handlers.ContainsKey(msgHeader))
                _handlers[msgHeader].RemoveAll(h => !h.Target.IsAlive);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    [EventHandler]
    public async void OnUdpMsgReceived(UdpMsgReceivedEvent e)
    {
        foreach (var header in _eventHeaders)
        {
            if (e.Data.StartsWith(header))
            {
                var data = e.Data.Substring(header.Length);
                RaiseEvent(header, data, e.Ip);
            }
        }
    }

    [EventHandler]
    public async void OnTcpMsgReceived(TcpMsgReceivedEvent e)
    {
        foreach (var header in _eventHeaders)
        {
            if (e.Data.StartsWith(header))
            {
                var data = e.Data.Substring(header.Length);
                RaiseEvent(header, data, e.Ip);
            }
        }
    }
}