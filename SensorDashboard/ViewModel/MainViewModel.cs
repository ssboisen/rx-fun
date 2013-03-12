using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using SessionDashboard.Model;

namespace SessionDashboard.ViewModel
{
   
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly ObservableCollection<SampleGroup> _sampleGroups = new ObservableCollection<SampleGroup>();
        private readonly ObservableCollection<string> _warnings = new ObservableCollection<string>();
        
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetSamples().GroupBy(sample => new { sample.SensorId, sample.UnitOfMeasure }).ObserveOnDispatcher().Subscribe(grp =>
                                                                                       {
                                                                                           var sampleGroup = new SampleGroup
                                                                                                                 {
                                                                                                                     SensorId = grp.Key.SensorId,
                                                                                                                     UnitOfMeasure = grp.Key.UnitOfMeasure,
                                                                                                                     Samples = new ObservableCollection<double>()
                                                                                                                 };
                                                                                           SampleGroups.Add(sampleGroup);

                                                                                           grp.Buffer(5).Subscribe(window => sampleGroup.AverageMeasurement = window.Average(avg => avg.SensorValue));

                                                                                           /*grp.Scan(new { sum = .0, count = 0 }, (agg, sample) => new { sum = agg.sum + sample.SensorValue, count = agg.count + 1 })
                                                                                                    .Select(agg => agg.sum / agg.count).ObserveOnDispatcher().Subscribe(average => sampleGroup.AverageMeasurement = average);*/
                                                                                           
                                                                                           grp.Sample(TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Select(sample => sample.SensorValue).Subscribe(sampleGroup.Samples.Add);
                                                                                       });
            _dataService
                .GetSamples()
                .Where(sample => sample.SampleType == SensorSample.SampleTypes.Temperature && sample.SensorValue < -25)
                .ObserveOnDispatcher().Subscribe(sample => Warnings.Add(string.Format("Warning: Sensor {0} registerede temperature of {1:0.00}", sample.SensorId, sample.SensorValue)));
        }



        public ObservableCollection<SampleGroup> SampleGroups
        {
            get { return _sampleGroups; }
        }

        public ObservableCollection<string> Warnings
        {
            get { return _warnings; }
        }
    }

    public class SampleGroup : ViewModelBase
    {
        public string SensorId { get; set; }
        public string UnitOfMeasure { get; set; }
        public ObservableCollection<double> Samples { get; set; }
        private double _averageMeasurement;
        public double AverageMeasurement
        {
            get { return _averageMeasurement; }
            set { _averageMeasurement = value; RaisePropertyChanged(() => AverageMeasurement); }
        }

        private double _latestMeasurement;
        public double LatestMeasurement
        {
            get { return _latestMeasurement; }
            set { _latestMeasurement = value; RaisePropertyChanged(() => LatestMeasurement); }
        }
    }
}