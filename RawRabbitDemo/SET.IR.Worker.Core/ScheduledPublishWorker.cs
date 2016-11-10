using System;
using System.Timers;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public abstract class ScheduledPublishWorker:PublishWorker,IDisposable
    {

        private Timer _timer;

        protected override void Initialize()
        {
            var cfg = Configuration.ScheduledConfig;

            if (cfg == null)
            {
                throw new Exception("ScheduledConfig is required.");
            }

            int timerInterval = cfg.IntervalSeconds;

            _timer = new Timer(timerInterval) { Interval = timerInterval };
            _timer.Elapsed += TimerElapsed;

            _timer.Start();
        }


        private void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            ScheduleTriggered();
        }

        protected abstract void ScheduleTriggered();


        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}