using System;

namespace FluentTokenBuckets
{
    public class StepUpTokenBucket
    {
        public double Capacity { get; set; }
        public double DrainRate { get; set; }
        public int DrainIntervalSeconds { get; set; }
        private double _tokens;
        private DateTime _lastUpdateDateTime;

        public StepUpTokenBucket WithCapacity(double tokens)
        {
            Capacity = tokens;
            return this;
        }

        public StepUpTokenBucket WithDrainRate(double tokens)
        {
            DrainRate = tokens;
            return this;
        }

        public StepUpTokenBucket WithDrainInterval(int seconds)
        {
            DrainIntervalSeconds = seconds;
            return this;
        }

        public StepUpTokenBucket Build()
        {
            _tokens = 0;
            _lastUpdateDateTime = DateTime.UtcNow;
            return this;
        }

        public bool Consume(double tokens)
        {
            if (_tokens <= Capacity - tokens)
            {
                _tokens += tokens;
                return true;
            }
            return false;
        }

        public bool CanConsume(double tokens)
        {
            var msSinceLastUpdate = DateTime.UtcNow - _lastUpdateDateTime;
            _tokens = Math.Max(0, _tokens - (msSinceLastUpdate.TotalMilliseconds / (DrainIntervalSeconds * 1000)) * DrainRate);
            _lastUpdateDateTime += msSinceLastUpdate;
            return _tokens <= Capacity - tokens;
        }
    }
}
