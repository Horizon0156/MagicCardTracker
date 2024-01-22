[![🚨 CI](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/continiuos_integration.yml/badge.svg)](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/continiuos_integration.yml)
[![Docker (Dev)](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/continious_delivery.yml/badge.svg)](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/continious_delivery.yml)
[![🐳 Docker (Release)](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/release.yml/badge.svg)](https://github.com/Horizon0156/MagicCardTracker/actions/workflows/release.yml)

![Magic Card Tracker](https://raw.githubusercontent.com/Horizon0156/MagicCardTracker/main/Artwork/banner_small.jpg)

Magic Card Tracker (MCT) is a Progressive Web Application to keep track of your MTG card collection. Card information and prices are loaded from [Scryfall](https://scryfall.com), MCT will enrich your collection information and store the data in your browser. Besides the hosting of the application itself, no additional backend services or databases are required. 

_This application was implemented for personal use and is designed to be simple, extendable and lightweight. Actually, I started this project to try out Blazor in spare time and built the features as I needed those to keep track of my cards while getting rid of Excel. Feel free to run this application to track your collection as well, but appreciate that not everything might work as you would have expected it. Neither Wizards nor Scryfall endorsed this application. In case you host MCT and made it available to the public, please have look at the privacy policy and license information and adjust those to your needs. In addition, you are not allowed to drop the disclaimer about Wizards and the usage related to the Fan Content Policy._

## Installation

Magic Card Tracker is a web application developed on top of ASP.NET Core Blazor (Webassembly). The application is also containerized and ships as a lightwight NGINX image that will host the assemblies. Therefore, you have two options to run this application

### .NET CLI / Dev. Container

If you are using Visual Studio Code, [Dev Containers](https://code.visualstudio.com/docs/remote/containers) will provide you a preconfigred development environment without messing up your host machine. Simply clone the repository into a volume or your computer and open the folder in a container.

Alternatively you can use the [.NET CLI](https://dotnet.microsoft.com/download) to build and serve the application.

1. Download or clone the repository

2. Run Magic Card Tracker from source
```bash
dotnet run --project Sources/MagicCardTracker.Pwa/MagicCardTracker.Pwa.csproj
```
3. Enjoy your instance running on http://localhost:5000 or https://localhost:5001

In case you are planning to develop new features, you can also run the application in `watch` mode. 

### Docker 
Build the image on your own or make use of a pre-build multi architecture image that is pushed to [ghcr.io](https://github.com/Horizon0156/MagicCardTracker/pkgs/container/magic-card-tracker). The image will host the application on port `80`. You might wanna mount a local `appsettings.json` to `/usr/share/nginx/html/appsettings.json` in order to adjust parameter during runtime without building a custom image. 

```bash
docker run -d -p 8080:80 --name mct ghcr.io/horizon0156/magic-card-tracker
```

## Changelog
The [Releases](https://github.com/Horizon0156/MagicCardTracker/releases) page will provide an overview about recent releases and their included features.

## Roadmap
Check out the [open issues](https://github.com/Horizon0156/MagicCardTracker/issues) for a list of proposed features (and known issues). If you have ideas for new features that you don't feel comfortable to contribute on your own, feel free to create a ticket as well, maybe someone will find the time.

## Contributing
Contributions are welcome. For major changes, please open an issue first to discuss what you would like to change. For minor changes just fork the repo, create a branch (prefixed with `/feature` or `/bugfix`) and open a pull request. 

Please make sure to update tests as appropriate.

## Acknowledgements
The application is build on top of the following frameworks / libraries. Without this great work, I wouldn't be apple to build this app in that short amount of time.

* [.NET8 / ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor) 
* [Blazored/LocalStorage](https://github.com/Blazored/LocalStorage) for accessing the local storage
* [ChartJs.Blazor](https://github.com/mariusmuntean/ChartJs.Blazor) for the charts
* [Bootstrap 5](https://getbootstrap.com/docs/5.0/getting-started/introduction/) for the grid layout and CSS helper classes
* [Satans Minions Font](https://www.dafont.com/satans-minions.font) for the typings in the application's logo
* [AutoMapper](https://automapper.org/), [NSubstitue](https://nsubstitute.github.io/), [XUnit](https://xunit.net/), [MediatR](https://github.com/jbogard/MediatR) for unit testing, DTOs and separation of concerns.
* [Scryfall](https://scryfall.com/) for the card database, imaginary and pricing information

## Localization
The application is displayed in English and supports to add cards from Scryfall printed in German or English language. As already mentioned, this tool was written quickly for personal needs. Feel free to contribute additional localization support.

## License
The source code of Magic Card Tracker is licensed under the [MIT](https://choosealicense.com/licenses/mit/) license. Therefore, there is no liability or warranty. Feel free to host or extend the applications to your needs. If you do so, please adjust the privacy policy. The provided page acts as a template and might not cover your hosting scenarios. 

Magic Card Tracker is is unofficial Fan Content permitted under the Fan Content Policy. Not approved/endorsed by Wizards. Portions of the materials used are property of Wizards of the Coast. ©Wizards of the Coast LLC. 
