﻿// SPDX-License-Identifier: MIT
// Copyright AppMotor Framework (https://github.com/skrysm/AppMotor)

using System.Collections.Immutable;

using AppMotor.CoreKit.Utils;

using JetBrains.Annotations;

namespace AppMotor.CoreKit.Events;

/// <summary>
/// The public API surface of an <see cref="EventSource"/> (accessible via <see cref="EventSource.Event"/>).
/// Contains the methods to register new event handlers.
/// </summary>
public sealed class Event
{
    private readonly Lock _eventHandlersLock = new();

    private ImmutableArray<EventHandlerRegistration> _eventHandlers = [];

    /// <summary>
    /// Constructor.
    /// </summary>
    internal Event()
    {
    }

    /// <summary>
    /// Registers a synchronous event handler and returns its registration.
    /// </summary>
    /// <param name="eventHandler">The event handler</param>
    /// <returns>The event handler registration. Dispose this instance to remove the registration.</returns>
    /// <seealso cref="RegisterEventHandler(Func{Task})"/>
    [MustUseReturnValue]
    public IEventHandlerRegistration RegisterEventHandler(Action eventHandler)
    {
        var registration = new EventHandlerRegistration(
            this,
            () =>
            {
                eventHandler();
                return Task.CompletedTask;
            }
        );

        lock (this._eventHandlersLock)
        {
            this._eventHandlers = this._eventHandlers.Add(registration);
        }

        return registration;
    }

    /// <summary>
    /// Registers an <c>async</c> event handler and returns its registration.
    /// </summary>
    /// <param name="eventHandler">The event handler</param>
    /// <returns>The event handler registration. Dispose this instance to remove the registration.</returns>
    /// <seealso cref="RegisterEventHandler(Action)"/>
    [MustUseReturnValue]
    public IEventHandlerRegistration RegisterEventHandler(Func<Task> eventHandler)
    {
        var registration = new EventHandlerRegistration(this, eventHandler);

        lock (this._eventHandlersLock)
        {
            this._eventHandlers = this._eventHandlers.Add(registration);
        }

        return registration;
    }

    private void RemoveRegistration(EventHandlerRegistration registration)
    {
        lock (this._eventHandlersLock)
        {
            this._eventHandlers = this._eventHandlers.Remove(registration);
        }
    }

    internal async Task RaiseEventAsync()
    {
        // ReSharper disable once InconsistentlySynchronizedField
        ImmutableArray<EventHandlerRegistration> eventHandlers = this._eventHandlers;
        if (eventHandlers.IsEmpty)
        {
            return;
        }

        foreach (var eventHandlerRegistration in eventHandlers)
        {
            await eventHandlerRegistration.EventHandler().ConfigureAwait(false);
        }
    }

    private sealed class EventHandlerRegistration : Disposable, IEventHandlerRegistration
    {
        public readonly Func<Task> EventHandler;

        private readonly Event _containingEvent;

        public EventHandlerRegistration(Event containingEvent, Func<Task> eventHandler)
        {
            this.EventHandler = eventHandler;
            this._containingEvent = containingEvent;
        }

        /// <inheritdoc />
        protected override void DisposeManagedResources()
        {
            this._containingEvent.RemoveRegistration(this);
        }
    }
}
