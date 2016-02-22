using System.ComponentModel;

namespace Behaviors.Sample
{
	public class InvokeMethodDemoPageViewModel : INotifyPropertyChanged
	{
		public int Counter { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public InvokeMethodDemoPageViewModel ()
		{
			Counter = 0;
		}

		public void IncrementCounter ()
		{
			Counter++;
			OnPropertyChanged ("Counter");
		}

		protected virtual void OnPropertyChanged (string propertyName)
		{
			var changed = PropertyChanged;
			if (changed != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}

