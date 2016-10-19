using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawRabbitMain
{
    class Program
    {
        static void Main(string[] args)
        {
            CookedRabbit rab = new CookedRabbit("Test","Cottontail");

            rab.Subscribe<string>(async (s, context) =>
            {
                await Console.Out.WriteLineAsync(s);
            });
        }
    }
}
