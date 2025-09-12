using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetGeminiSDK.Services
{
    /// <summary>
    /// Interface for metrics service
    /// </summary>
    public interface IMetricsService
    {
        /// <summary>
        /// Record a counter metric
        /// </summary>
        /// <param name="name">Metric name</param>
        /// <param name="value">Counter value</param>
        /// <param name="tags">Optional tags</param>
        void IncrementCounter(string name, double value = 1, Dictionary<string, string>? tags = null);
        
        /// <summary>
        /// Record a timing metric
        /// </summary>
        /// <param name="name">Metric name</param>
        /// <param name="duration">Duration in milliseconds</param>
        /// <param name="tags">Optional tags</param>
        void RecordTiming(string name, double duration, Dictionary<string, string>? tags = null);
        
        /// <summary>
        /// Record a gauge metric
        /// </summary>
        /// <param name="name">Metric name</param>
        /// <param name="value">Gauge value</param>
        /// <param name="tags">Optional tags</param>
        void RecordGauge(string name, double value, Dictionary<string, string>? tags = null);
        
        /// <summary>
        /// Record an error metric
        /// </summary>
        /// <param name="name">Error name</param>
        /// <param name="exception">Exception details</param>
        /// <param name="tags">Optional tags</param>
        void RecordError(string name, Exception exception, Dictionary<string, string>? tags = null);
        
        /// <summary>
        /// Get metrics summary
        /// </summary>
        /// <returns>Metrics summary</returns>
        Task<MetricsSummary> GetSummaryAsync();
    }
    
    /// <summary>
    /// Console metrics service implementation
    /// </summary>
    public class ConsoleMetricsService : IMetricsService
    {
        private readonly ILoggingService _loggingService;
        private readonly Dictionary<string, double> _counters = new();
        private readonly Dictionary<string, List<double>> _timings = new();
        private readonly Dictionary<string, double> _gauges = new();
        private readonly Dictionary<string, int> _errors = new();
        
        /// <summary>
        /// Initialize console metrics service
        /// </summary>
        /// <param name="loggingService">Logging service</param>
        public ConsoleMetricsService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        /// <inheritdoc/>
        public void IncrementCounter(string name, double value = 1, Dictionary<string, string>? tags = null)
        {
            lock (_counters)
            {
                _counters[name] = (_counters.ContainsKey(name) ? _counters[name] : 0) + value;
            }
            
            var tagString = tags != null ? $" [{string.Join(", ", tags)}]" : "";
            _loggingService.LogDebug($"Counter {name}: +{value}{tagString}");
        }
        
        /// <inheritdoc/>
        public void RecordTiming(string name, double duration, Dictionary<string, string>? tags = null)
        {
            lock (_timings)
            {
                if (!_timings.ContainsKey(name))
                    _timings[name] = new List<double>();
                _timings[name].Add(duration);
            }
            
            var tagString = tags != null ? $" [{string.Join(", ", tags)}]" : "";
            _loggingService.LogDebug($"Timing {name}: {duration}ms{tagString}");
        }
        
        /// <inheritdoc/>
        public void RecordGauge(string name, double value, Dictionary<string, string>? tags = null)
        {
            lock (_gauges)
            {
                _gauges[name] = value;
            }
            
            var tagString = tags != null ? $" [{string.Join(", ", tags)}]" : "";
            _loggingService.LogDebug($"Gauge {name}: {value}{tagString}");
        }
        
        /// <inheritdoc/>
        public void RecordError(string name, Exception exception, Dictionary<string, string>? tags = null)
        {
            lock (_errors)
            {
                _errors[name] = (_errors.ContainsKey(name) ? _errors[name] : 0) + 1;
            }
            
            var tagString = tags != null ? $" [{string.Join(", ", tags)}]" : "";
            _loggingService.LogError($"Error {name}: {exception.Message}{tagString}", exception);
        }
        
        /// <inheritdoc/>
        public Task<MetricsSummary> GetSummaryAsync()
        {
            var summary = new MetricsSummary
            {
                Counters = new Dictionary<string, double>(_counters),
                Timings = new Dictionary<string, TimingStats>(),
                Gauges = new Dictionary<string, double>(_gauges),
                Errors = new Dictionary<string, int>(_errors)
            };
            
            lock (_timings)
            {
                foreach (var timing in _timings)
                {
                    var values = timing.Value;
                    if (values.Count > 0)
                    {
                        values.Sort();
                        summary.Timings[timing.Key] = new TimingStats
                        {
                            Count = values.Count,
                            Min = values[0],
                            Max = values[values.Count - 1],
                            Average = values.Sum() / values.Count,
                            Median = values.Count % 2 == 0 
                                ? (values[values.Count / 2 - 1] + values[values.Count / 2]) / 2
                                : values[values.Count / 2]
                        };
                    }
                }
            }
            
            return Task.FromResult(summary);
        }
    }
    
    /// <summary>
    /// Metrics summary data
    /// </summary>
    public class MetricsSummary
    {
        /// <summary>
        /// Counter metrics
        /// </summary>
        public Dictionary<string, double> Counters { get; set; } = new();
        
        /// <summary>
        /// Timing metrics
        /// </summary>
        public Dictionary<string, TimingStats> Timings { get; set; } = new();
        
        /// <summary>
        /// Gauge metrics
        /// </summary>
        public Dictionary<string, double> Gauges { get; set; } = new();
        
        /// <summary>
        /// Error metrics
        /// </summary>
        public Dictionary<string, int> Errors { get; set; } = new();
    }
    
    /// <summary>
    /// Timing statistics
    /// </summary>
    public class TimingStats
    {
        /// <summary>
        /// Number of measurements
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// Minimum value
        /// </summary>
        public double Min { get; set; }
        
        /// <summary>
        /// Maximum value
        /// </summary>
        public double Max { get; set; }
        
        /// <summary>
        /// Average value
        /// </summary>
        public double Average { get; set; }
        
        /// <summary>
        /// Median value
        /// </summary>
        public double Median { get; set; }
    }
}
