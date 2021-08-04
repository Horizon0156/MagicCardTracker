
using Horizon.MagicCardTracker.Contracts;
using MediatR;

namespace Horizon.MagicCardTracker.Pwa.Commands
{
    internal class AddCardByNumber : IRequest<CollectedCard>
    {
        public AddCardByNumber(string setCode, string cardNumber, string languageCode)
        {
            SetCode = setCode;
            CardNumber = cardNumber;
            LanguageCode = languageCode;
        }

        public string SetCode { get; }

        public string CardNumber { get; }

        public string LanguageCode { get; set; }
    }
}
