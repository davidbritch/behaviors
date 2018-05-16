using System;
using Xamarin.Forms;

namespace Behaviors.Sample
{
	public partial class InvokeMethodDemoPage : ContentPage
	{
		public static readonly BindableProperty CounterProperty = BindableProperty.Create (nameof(Counter), typeof(int), typeof(InvokeMethodDemoPage), default(int));

		public int Counter {
			get { return (int)GetValue (CounterProperty); }
			set { SetValue (CounterProperty, value); }
		}

		public InvokeMethodDemoPage ()
		{
			InitializeComponent ();
		}

		public void OnButtonClicked (object sender, EventArgs args)
		{
			Counter++;
		}

		public void OnPickerChanged ()
		{
			selectedItemLabel.Text = picker.Items [picker.SelectedIndex];
		}
	}
}
