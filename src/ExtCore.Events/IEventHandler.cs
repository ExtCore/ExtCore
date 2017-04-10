// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ExtCore.Events
{
  /// <summary>
  /// Describes an event handler that can handle the events broadcasted by the
  /// <see cref="Event{TEventHandler}"/> class.
  /// </summary>
  public interface IEventHandler
  {
    /// <summary>
    /// Priority of the event handler. The event handlers of the same event will be executed in the order
    /// specified by the priority.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles the event.
    /// </summary>
    void HandleEvent();
  }

  /// <summary>
  /// Describes an event handler that can handle the events broadcasted by the
  /// <see cref="Event{TEventHandler, TEventArgument}"/> class.
  /// </summary>
  /// <typeparam name="TEventArgument">Defines the type of the argument that will be passed to the event handler.</typeparam>
  public interface IEventHandler<TEventArgument>
  {
    /// <summary>
    /// Priority of the event handler. The event handlers of the same event will be executed in the order
    /// specified by the priority.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="argument">The event argument.</param>
    void HandleEvent(TEventArgument argument);
  }

  /// <summary>
  /// Describes an event handler that can handle the events broadcasted by the
  /// <see cref="Event{TEventHandler, TEventArgument1, TEventArgument2}"/> class.
  /// </summary>
  /// <typeparam name="TEventArgument1">Defines the type of the first argument that will be passed to the event handler.</typeparam>
  /// <typeparam name="TEventArgument2">Defines the type of the second argument that will be passed to the event handler.</typeparam>
  public interface IEventHandler<TEventArgument1, TEventArgument2>
  {
    /// <summary>
    /// Priority of the event handler. The event handlers of the same event will be executed in the order
    /// specified by the priority.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="argument1">The first event argument.</param>
    /// <param name="argument2">The second event argument.</param>
    void HandleEvent(TEventArgument1 argument1, TEventArgument2 argument2);
  }

  /// <summary>
  /// Describes an event handler that can handle the events broadcasted by the
  /// <see cref="Event{TEventHandler, TEventArgument1, TEventArgument2, TEventArgument3}"/> class.
  /// </summary>
  /// <typeparam name="TEventArgument1">Defines the type of the first argument that will be passed to the event handler.</typeparam>
  /// <typeparam name="TEventArgument2">Defines the type of the second argument that will be passed to the event handler.</typeparam>
  /// <typeparam name="TEventArgument3">Defines the type of the first argument that will be passed to the event handler.</typeparam>
  public interface IEventHandler<TEventArgument1, TEventArgument2, TEventArgument3>
  {
    /// <summary>
    /// Priority of the event handler. The event handlers of the same event will be executed in the order
    /// specified by the priority.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="argument1">The first event argument.</param>
    /// <param name="argument2">The second event argument.</param>
    /// <param name="argument3">The third event argument.</param>
    void HandleEvent(TEventArgument1 argument1, TEventArgument2 argument2, TEventArgument3 argument3);
  }
}