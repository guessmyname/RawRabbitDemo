using System;
using System.Timers;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public abstract class ScheduledPublishWorker:PublishWorker,IDisposable
    {

        private readonly Timer _timer;

        protected ScheduledPublishWorker(IBusClient<AdvancedMessageContext> client) : base(client)
        {
            int timerInterval = Configuration.CustomSettings.Interval;

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