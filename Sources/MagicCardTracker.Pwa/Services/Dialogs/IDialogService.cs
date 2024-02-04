using Microsoft.AspNetCore.Components;

namespace MagicCardTracker.Pwa.Services.Dialogs;

internal interface IDialogService
{
    event EventHandler CloseRequested;

    event EventHandler<DialogMetadata> DialogRequested;

    void CloseDialog();
    
    void ShowDialog<TDialog>() where TDialog : IComponent;

    void ShowDialog<TDialog>(string parameterName, object parameterValue) where TDialog : IComponent;

    void ShowDialog<TDialog>(Dictionary<string, object> parameters) where TDialog : IComponent;
}