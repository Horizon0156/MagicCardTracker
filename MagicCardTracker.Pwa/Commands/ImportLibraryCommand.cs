using System.IO;
using MediatR;

namespace MagicCardTracker.Pwa.Commands
{
    internal class ImportLibraryCommand : IRequest
    {
        public ImportLibraryCommand(Stream libraryBackupStream)
        {
            LibraryBackupStream = libraryBackupStream;
        }

        public Stream LibraryBackupStream { get; }
    }
}