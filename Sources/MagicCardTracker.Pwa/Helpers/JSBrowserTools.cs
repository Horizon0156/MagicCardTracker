using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MagicCardTracker.Pwa.Helpers
{
    internal class JSBrowserTools : IBrowserTools
    {
        private readonly IJSRuntime _jsRuntime;

        public JSBrowserTools(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public async Task SaveAsFileAsync(
            string filename, 
            string fileContent, 
            CancellationToken cancellationToken)
        {
            await _jsRuntime.InvokeVoidAsync(
                "saveFileAs",
                cancellationToken,
                filename,
                fileContent);
        }

        public async Task ScrollToTopAsync(CancellationToken cancellationToken)
        {
            await _jsRuntime.InvokeVoidAsync("scrollToTop");
        }
    }
}
