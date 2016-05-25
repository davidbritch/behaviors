using System.Threading.Tasks;

namespace Behaviors
{
	public interface IAction
	{
		Task<bool> Execute (object sender, object parameter);
	}
}
