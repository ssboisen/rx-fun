using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace CombinationOperators
{
    class Program
    {
        private static IObservable<int> Xs
        {
            get { return Generate(0, new List<int> { 1, 2, 2, 2, 2 }); }
        }

        private static IObservable<int> Ys
        {
            get { return Generate(100, new List<int> { 2, 2, 2, 2, 2 }); }
        }

        private static IObservable<int> Generate(int initialValue, IList<int> intervals)
        {
            // work-around for Observable.Generate calling timeInterval before resultSelector
            intervals.Add(0);

            return Observable.Generate(initialValue,
                                       x => x < initialValue + intervals.Count - 1,
                                       x => x + 1,
                                       x => x,
                                       x => TimeSpan.FromSeconds(intervals[x - initialValue]));
        }

        private static void Main()
        {
            Console.WriteLine("Press any key to unsubscribe");

            Console.WriteLine("------------ Merge -------------");
            using (Xs.Merge(Ys).Timestamp().Subscribe(
                z => Console.WriteLine("{0,3}: {1}", z.Value, z.Timestamp),
                () => Console.WriteLine("Completed, press a key")))
            {
                Console.ReadKey();
            }

            Console.WriteLine("------------ Zip -------------");
            using (Xs.Zip(Ys, (x, y) => x + y).Timestamp().Subscribe(
                z => Console.WriteLine("{0,3}: {1}", z.Value, z.Timestamp),
                () => Console.WriteLine("Completed, press a key")))
            {
                Console.ReadKey();
            }

            Console.WriteLine("------------ CombineLatest -------------");
            using (Xs.CombineLatest(Ys, (x, y) => x + y).Timestamp().Subscribe(
                z => Console.WriteLine("{0,3}: {1}", z.Value, z.Timestamp),
                () => Console.WriteLine("Completed, press a key")))
            {
                Console.ReadKey();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
