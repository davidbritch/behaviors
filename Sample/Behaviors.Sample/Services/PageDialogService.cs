using System.Threading.Tasks;
using Xamarin.Forms;

namespace Behaviors.Sample
{
	public class PageDialogService : IPageDialogService
	{
		public async Task<bool> DisplayAlert (string title, string message, string acceptButtonText, string cancelButtonText)
		{
			return await Application.Current.MainPage.DisplayAlert (title, message, acceptButtonText, cancelButtonText);
		}

		public async Task DisplayAlert (string title, string message, string cancelButtonText)
		{
			await Application.Current.MainPage.DisplayAlert (title, message, cancelButtonText);
		}
	}
}
