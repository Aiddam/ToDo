using System.Threading.Tasks;

namespace ToDo.ViewModels;

public abstract class ViewModelBaseWithParameters<T> : ViewModelBase
{
    public abstract Task SetAdditionalParameter(T parameter);
}
