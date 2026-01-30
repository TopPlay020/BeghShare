using System;
using System.Collections.Generic;
using System.Text;

namespace BeghShare.Events
{
    public class OnlineComputerCountChangedEvent
    {
        public int Count { get; }
        public OnlineComputerCountChangedEvent(int count)
        {
            Count = count;
        }
    }
}
