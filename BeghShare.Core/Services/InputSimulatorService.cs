using BeghCore;
using SharpHook;
using SharpHook.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeghShare.Core.Services
{
    public class InputSimulatorService : ISingleton
    {
        private EventSimulator _simulator;

        public InputSimulatorService()
        {
            _simulator = new EventSimulator();
        }

        public void SimulateMouseMovement(short x, short y)
        {
            _simulator.SimulateMouseMovement(x, y);

        }

        public void SimulateKeyPress(KeyCode keyCode)
        {
            _simulator.SimulateKeyPress(keyCode);
        }

        public void SimulateKeyRelease(KeyCode keyCode)
        {
            _simulator.SimulateKeyRelease(keyCode);
        }

        public void SimulateMousePress(MouseButton button)
        {
            _simulator.SimulateMousePress(button);
        }

        public void SimulateMouseRelease(MouseButton button)
        {
            _simulator.SimulateMouseRelease(button);
        }

        public void SimulateMouseWheel(short rotation, MouseWheelScrollDirection direction, MouseWheelScrollType scrollType)
        {
            _simulator.SimulateMouseWheel(rotation, direction, scrollType);
        }
    }
}
