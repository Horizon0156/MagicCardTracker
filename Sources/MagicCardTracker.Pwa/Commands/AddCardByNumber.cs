
using MagicCardTracker.Contracts;
using MediatR;

namespace MagicCardTracker.Pwa.Commands;

internal class AddCardByNumber : IRequest<CollectedCard>
{
    public AddCardByNumber(
        string setCode, 
        string cardNumber, 
        string languageCode, 
        bool addAsFoil = false)
    {
        SetCode = setCode;
        CardNumber = cardNumber;
        LanguageCode = languageCode;
        AddAsFoil = addAsFoil;
    }

    public string SetCode { get; }

    public string CardNumber { get; }

    public string LanguageCode { get; set; }

    public bool AddAsFoil { get; }
}
