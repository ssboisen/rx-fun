using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SessionDashboard.Model
{
    public interface IDataService
    {
        IObservable<SensorSample> GetSamples();
    }

    public class SensorSample
    {
        public enum SampleTypes { Temperature, Humidity, Pressure }

        public SensorSample(string sensorId, double sensorValue, string unitOfMeasure, SampleTypes sampleType)
        {
            SampleType = sampleType;
            UnitOfMeasure = unitOfMeasure;
            SensorValue = sensorValue;
            SensorId = sensorId;
        }

        public string SensorId { get; private set; }
        public double SensorValue { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public SampleTypes SampleType { get; private set; }
    }
}
