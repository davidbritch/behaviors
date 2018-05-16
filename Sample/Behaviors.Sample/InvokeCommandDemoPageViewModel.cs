using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.Sample
{
	public class InvokeCommandDemoPageViewModel : INotifyPropertyChanged
	{
		readonly IPageDialogService pageDialogService;

		public List<Person> People { get; private set; }

		public ICommand PageAppearingCommand { get; private set; }

		public ICommand PageDisappearingCommand { get; private set; }

		public ICommand ItemSelectedCommand { get; private set; }

		public ICommand OutputAgeCommand { get; private set; }

		public ICommand OutputMessageCommand { get; private set; }

		public string SelectedItemText { get; private set; }

		public string AgeText { get; private set; }

		public string MessageText { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public InvokeCommandDemoPageViewModel ()
		{
			pageDialogService = new PageDialogService ();

			People = new List<Person> {
				new Person ("Steve", 21),
				new Person ("John", 37),
				new Person ("Tom", 42),
				new Person ("Lucas", 29)
			};
			PageAppearingCommand = new Command (OnPageAppearing);
			PageDisappearingCommand = new Command (OnPageDisappearing);
			ItemSelectedCommand = new Command<Person> (OutputItemSelected);
			OutputAgeCommand = new Command<Person> (OutputAge);
			OutputMessageCommand = new Command (OutputMessage);
		}

		void OnPageAppearing ()
		{
			pageDialogService.DisplayAlert ("Invoke Command Demo Page", "Appearing event fired.", "OK");
		}

		void OnPageDisappearing ()
		{
			pageDialogService.DisplayAlert ("Invoke Command Demo Page", "Disappearing event fired.", "OK");
		}

		void OutputItemSelected (Person person)
		{
			SelectedItemText = string.Format ("{0} is selected in the list.", person.Name);
			OnPropertyChanged ("SelectedItemText");
		}

		void OutputAge (Person person)
		{
    		if (person.AgeParameter > person.Age)
    			AgeText = string.Format("{0} is {1}. That's younger than {2}.", person.Name, person.Age, person.AgeParameter);
    		else
    			AgeText = string.Format("{0} is {1}. That's older than {2}.", person.Name, person.Age, person.AgeParameter);
            OnPropertyChanged ("AgeText");
		}

		void OutputMessage ()
		{
			MessageText = "Successfully entered text.";
			OnPropertyChanged ("MessageText");
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

