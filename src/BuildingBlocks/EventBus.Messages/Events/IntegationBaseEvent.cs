namespace EventBus.Messages.Events
{
    using System;

    public class IntegationBaseEvent
    {
        public IntegationBaseEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        public IntegationBaseEvent(Guid id, DateTime creationDate)
        {
            this.Id = id;
            this.CreationDate = creationDate;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}
