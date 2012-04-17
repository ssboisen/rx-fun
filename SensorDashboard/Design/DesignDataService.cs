using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using SessionDashboard.Model;

namespace SessionDashboard.Design
{
    public class DesignDataService : IDataService
    {
        public IObservable<SensorSample> GetSamples()
        {
            return Observable.Empty<SensorSample>(); 
        }
    }
}