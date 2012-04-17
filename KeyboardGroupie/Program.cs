using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace KeyboardGroupie
{
    class Program
    {
        static IEnumerable<ConsoleKeyInfo> KeyPresses()
        {
            for (; ; )
            {
                var currentKey = Console.ReadKey(true);

                if (currentKey.Key == ConsoleKey.Enter)
                    yield break;
                
                yield return currentKey;
            }
        }
        static void Main()
        {
            var timeToStop = new ManualResetEvent(false);
            var keyPresses = KeyPresses().ToObservable();

            var groupedKeyPresses =
                from k in keyPresses
                group k by k.Key into keyPressGroup
                select keyPressGroup;

            Console.WriteLine("Press Enter to stop.  Now bang that keyboard!");

            groupedKeyPresses.Subscribe(keyPressGroup =>
            {
                int numberPresses = 0;

                keyPressGroup.Subscribe(keyPress =>
                {
                    Console.WriteLine(
                        "You pressed the {0} key {1} time(s)!",
                        keyPress.Key,
                        ++numberPresses);
                },
                () => timeToStop.Set());
            });

            timeToStop.WaitOne();
        }
    }
}
