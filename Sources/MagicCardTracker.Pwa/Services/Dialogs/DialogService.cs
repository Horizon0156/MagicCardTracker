using Microsoft.AspNetCore.Components;

namespace MagicCardTracker.Pwa.Services.Dialogs;

internal class DialogService : IDialogService
{
    public event EventHandler? CloseRequested;
    
    public event EventHandler<DialogMetadata>? DialogRequested;
   
    public void CloseDialog() => CloseRequested?.Invoke(this, EventArgs.Empty);

    public void ShowDialog<TDialog>() where TDialog : IComponent => ShowDialog<TDialog>([]);

    public void ShowDialog<TDialog>(string parameterName, object parameterValue) where TDialog : IComponent
        => ShowDialog<TDialog>(new() {{parameterName, parameterValue}});

    public void ShowDialog<TDialog>(Dictionary<string, object> parameters) where TDialog : IComponent
        => DialogRequested?.Invoke(this, new (typeof(TDialog), parameters));
}