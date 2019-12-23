﻿using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(params TEvent[] events) where TEvent : IEvent;
    }
}