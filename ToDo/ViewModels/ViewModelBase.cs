using ToDo.Models.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace ToDo.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public abstract User? CurrentUser { get; set; }

    public event Action<ViewModelBase>? OnChangeViewModel;

    protected void ChangeView(ViewModelBase nextView)
    {
        OnChangeViewModel?.Invoke(nextView);
    }

    protected IEnumerable<Delegate> GetDelegate()
    {
        return OnChangeViewModel?.GetInvocationList() ?? new Delegate[0];
    }
}
