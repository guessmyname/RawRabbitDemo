using System;
using System.Timers;

namespace RawRabbitMain
{
    public abstract class ScheduledPublishWorker:PublishWorker,IDisposable
    {

        private readonly Timer _timer;

        protected ScheduledPublishWorker(WorkerInstanceConfiguration configuration) : base(configuration)
        {
            int timerInterval = configuration.CustomSettings.Interval;

            _timer = new Timer(timerInterval) {Interval = timerInterval};
            _timer.Elapsed += ScheduleTriggered;

            _timer.Start();

        }


        protected abstract void ScheduleTriggered(object sender,ElapsedEventArgs args);


        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}