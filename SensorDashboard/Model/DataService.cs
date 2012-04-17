using System;
using System.Reactive.Linq;

namespace SessionDashboard.Model
{
    public class DataService : IDataService
    {
        public IObservable<SensorSample> GetSamples()
        {
            var random = new Random();
            var sensor1 = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Select(tick => new SensorSample("SensorNr1", random.NextDouble() * 80 - 40, "° Celcius", SensorSample.SampleTypes.Temperature));
            var sensor2 = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1.5)).Select(tick => new SensorSample("SensorNr2", random.NextDouble() * 100, "%", SensorSample.SampleTypes.Humidity));
            var sensor3 = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(10)).Select(tick => new SensorSample("SensorNr3", random.NextDouble() * 40 + 80, "kPa", SensorSample.SampleTypes.Pressure));
            return Observable.Merge(sensor1, sensor2, sensor3);
        }
    }
}