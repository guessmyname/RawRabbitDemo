using System;
using System.Timers;

namespace SET.IR.Worker.Core
{
    public abstract class ScheduledPublishWorker:PublishWorker,IDisposable
    {

        private readonly Timer _timer;

        protected ScheduledPublishWorker(WorkerInstanceConfiguration configuration) : base(configuration)
        {
            int timerInterval = configuration.CustomSettings.Interval;

            _timer = new Timer(timerInterval) {Interval = timerInterval};
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