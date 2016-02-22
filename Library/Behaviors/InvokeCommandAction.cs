using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors
{
	public sealed class InvokeCommandAction : BindableObject, IAction
	{
		public static readonly BindableProperty CommandProperty = BindableProperty.Create ("Command", typeof(ICommand), typeof(InvokeCommandAction), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create ("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
		public static readonly BindableProperty InputConverterProperty = BindableProperty.Create ("Converter", typeof(IValueConverter), typeof(InvokeCommandAction), null);

		public ICommand Command {
			get { return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}

		public object CommandParameter {
			get { return GetValue (CommandParameterProperty); }
			set { SetValue (CommandParameterProperty, value); }
		}

		public IValueConverter Converter {
			get { return (IValueConverter)GetValue (InputConverterProperty); }
			set { SetValue (InputConverterProperty, value); }
		}

		public object Execute (object sender, object parameter)
		{
			if (Command == null) {
				return false;
			}

			object resolvedParameter;
			if (CommandParameter != null) {
				resolvedParameter = CommandParameter;
			} else if (Converter != null) {
				resolvedParameter = Converter.Convert (parameter, typeof(object), null, null);
			} else {
				resolvedParameter = parameter;
			}

			if (!Command.CanExecute (resolvedParameter)) {
				return false;
			}

			Command.Execute (resolvedParameter);
			return true;
		}
	}
}

