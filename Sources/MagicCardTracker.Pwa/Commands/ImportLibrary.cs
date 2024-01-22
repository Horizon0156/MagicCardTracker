using MediatR;

namespace MagicCardTracker.Pwa.Commands;

internal class ImportLibrary : IRequest
{
    public ImportLibrary(Stream libraryBackupStream)
    {
        LibraryBackupStream = libraryBackupStream;
    }

    public Stream LibraryBackupStream { get; }
}