using System;

namespace FluentTokenBuckets
{
    public class StepDownTokenBucket
    {
        public double Capacity { get; set; }
        public double FillRate { get; set; }
        public int FillIntervalSeconds { get; set; }
        private double _tokens;
        private DateTime _lastUpdateDateTime;

        public StepDownTokenBucket WithCapacity(double tokens)
        {
            Capacity = tokens;
            return this;
        }

        public StepDownTokenBucket WithFillRate(double tokens)
        {
            FillRate = tokens;
            return this;
        }

        public StepDownTokenBucket WithFillInterval(int seconds)
        {
            FillIntervalSeconds = seconds;
            return this;
        }

        public StepDownTokenBucket Build()
        {
            _tokens = Capacity;
            _lastUpdateDateTime = DateTime.UtcNow;
            return this;
        }

        public bool Consume(double tokens)
        {
            if (_tokens >= tokens)
            {
                _tokens -= tokens;
                return true;
            }
            return false;
        }

        public bool CanConsume(double tokens)
        {
            var msSinceLastUpdate = DateTime.UtcNow - _lastUpdateDateTime;
            _tokens = Math.Min(Capacity, _tokens + (msSinceLastUpdate.TotalMilliseconds / (FillIntervalSeconds * 1000)) * FillRate);
            _lastUpdateDateTime += msSinceLastUpdate;
            return _tokens >= tokens;
        }
    }
}
