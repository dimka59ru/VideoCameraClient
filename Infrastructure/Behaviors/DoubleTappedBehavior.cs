using System.Windows.Input;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace App.Infrastructure.Behaviors;

public class DoubleTappedBehavior : AvaloniaObject
{
    static DoubleTappedBehavior()
    {
        CommandProperty.Changed.AddClassHandler<Interactive>(HandleCommandChanged);
    }

    /// <summary>
    /// Identifies the <seealso cref="CommandProperty"/> avalonia attached property.
    /// </summary>
    /// <value>Provide an <see cref="ICommand"/> derived object or binding.</value>
    public static readonly AttachedProperty<ICommand> CommandProperty = AvaloniaProperty.RegisterAttached<DoubleTappedBehavior, Interactive, ICommand>(
        "Command", default(ICommand), false, BindingMode.OneTime);

    /// <summary>
    /// Identifies the <seealso cref="CommandParameterProperty"/> avalonia attached property.
    /// Use this as the parameter for the <see cref="CommandProperty"/>.
    /// </summary>
    /// <value>Any value of type <see cref="object"/>.</value>
    public static readonly AttachedProperty<object> CommandParameterProperty = AvaloniaProperty.RegisterAttached<DoubleTappedBehavior, Interactive, object>(
        "CommandParameter", default(object), false, BindingMode.OneWay, null);


    /// <summary>
    /// <see cref="CommandProperty"/> changed event handler.
    /// </summary>
    private static void HandleCommandChanged(Interactive interactElem, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is ICommand commandValue)
        {
             // Add non-null value
             interactElem.AddHandler(InputElement.DoubleTappedEvent, Handler);
        }
        else
        {
             // remove prev value
             interactElem.RemoveHandler(InputElement.DoubleTappedEvent, Handler);
        }
        // local handler fcn
        static void Handler(object s, RoutedEventArgs e)
        {
            if (s is Interactive interactElem)
            {
                // This is how we get the parameter off of the gui element.
                object commandParameter = interactElem.GetValue(CommandParameterProperty);
                ICommand commandValue = interactElem.GetValue(CommandProperty);
                if (commandValue?.CanExecute(commandParameter) == true)
                {
                    commandValue.Execute(commandParameter);
                }
            }
        }
    }


    /// <summary>
    /// Accessor for Attached property <see cref="CommandProperty"/>.
    /// </summary>
    public static void SetCommand(AvaloniaObject element, ICommand commandValue)
    {
        element.SetValue(CommandProperty, commandValue);
    }

    /// <summary>
    /// Accessor for Attached property <see cref="CommandProperty"/>.
    /// </summary>
    public static ICommand GetCommand(AvaloniaObject element)
    {
        return element.GetValue(CommandProperty);
    }

    /// <summary>
    /// Accessor for Attached property <see cref="CommandParameterProperty"/>.
    /// </summary>
    public static void SetCommandParameter(AvaloniaObject element, object parameter)
    {
        element.SetValue(CommandParameterProperty, parameter);
    }

    /// <summary>
    /// Accessor for Attached property <see cref="CommandParameterProperty"/>.
    /// </summary>
    public static object GetCommandParameter(AvaloniaObject element)
    {
        return element.GetValue(CommandParameterProperty);
    }
}