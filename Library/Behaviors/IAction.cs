using System.Threading.Tasks;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	public interface IAction
	{
		Task<bool> Execute(object sender, object parameter);
	}
}
