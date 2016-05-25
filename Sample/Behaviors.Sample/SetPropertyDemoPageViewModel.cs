using System.ComponentModel;

namespace Behaviors.Sample
{
	public class SetPropertyDemoPageViewModel : INotifyPropertyChanged
	{
		string messageText, colourText;

		public string MessageText { 
			get { return messageText; }
			set {
				if (messageText != value) {
					messageText = value;
					OnPropertyChanged ("MessageText");
				}
			}
		}

		public string ColourText {
			get { return colourText; }
			set {
				if (colourText != value) {
					colourText = value;
					OnPropertyChanged ("ColourText");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public SetPropertyDemoPageViewModel ()
		{
			MessageText = "No message text set";
			ColourText = "Aqua";
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

