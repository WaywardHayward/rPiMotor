using System;
using System.Threading.Tasks;
using rPiMotor.Services;

namespace rPiMotor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Motor!");
            Task.Run(RunMotorService);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void RunMotorService()
        {
            var motorService = new StepperMotorService(
               new[] { 18, 23, 24, 25 },
               new uint[] { 0x01, 0x02, 0x04, 0x08 },
               new uint[] { 0x08, 0x04, 0x02, 0x01 }
           );

            motorService.Init();

            while (true)
            {
                motorService.MoveSteps(1, 60000 / 512, 512);
            }

        }
    }
}
