using System;
using System.Device.Gpio;
using System.Threading;

namespace rPiMotor.Services
{
    public class StepperMotorService
    {
        private readonly int[] _motorPins;
        private readonly uint[] _ccWSteps;
        private readonly uint[] _cwSteps;
        private readonly GpioController _controller;

        /// <summary>
        ///   Initializes a new instance of the <see cref="StepperMotorService" /> class.
        /// </summary>
        /// <param name="motorPins">The motor pins.</param>
        /// <param name="ccWSteps">The Counter-Clockwise step values.</param>
        /// <param name="cwSteps">The Clockwise step values.</param>
        public StepperMotorService(int[] motorPins, uint[] ccWSteps, uint[] cwSteps)
        {
            _motorPins = motorPins;
            _ccWSteps = ccWSteps;
            _cwSteps = cwSteps;
            _controller = new GpioController(PinNumberingScheme.Logical);
        }

        /// <summary>
        /// Moves the Motor in the specified direction for the specified number of steps.
        /// </summary>
        /// <param name="dir">The direction to move the motor.</param>
        /// <param name=",s">How long to wait between steps.</param>
        public void MoveOnePeriod(int dir, int ms)
        {
            var steps = dir == 1 ? _cwSteps : _ccWSteps;

            for (var stepIndex = 0; stepIndex < steps.Length; stepIndex++)
            {
                var step = steps[stepIndex];
                for (int i = 0; i < _motorPins.Length; i++)
                {
                    var pinValue = step == 1 << i ? PinValue.High : PinValue.Low;
                    _controller.Write(_motorPins[i], pinValue );
                }

                Thread.Sleep(Math.Max(3, ms));
            }
           

        }

        /// <summary>
        /// Initializes all the Motor pins.
        /// </summary>
        public void Init()
        {
            foreach (var pin in _motorPins)
            {
                if (_controller.IsPinOpen(pin))
                    _controller.ClosePin(pin);
                _controller.OpenPin(pin, PinMode.Output);
                _controller.Write(pin, PinValue.Low);
            }
        }

        /// <summary>
        /// Moves the Motor in the specified direction for the specified number of steps.
        /// </summary>
        /// <param name="dir">The direction to move the motor.</param>
        /// <param name="steps">How many steps to move the motor.</param>
        /// <param name="ms">How long to wait between steps.</param>
        public void MoveSteps(int dir, int ms, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                MoveOnePeriod(dir, ms);
            }
        }

        /// <summary>
        /// Stops the motor by writing to low value to all the pins.
        /// </summary>
        public void MotorStop()
        {
            for (int i = 0; i < _motorPins.Length; i++)
            {
                _controller.OpenPin(_motorPins[i], PinMode.Output);
                _controller.Write(_motorPins[i], PinValue.Low);
            }
        }

    }
}