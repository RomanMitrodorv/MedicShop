FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "MedicShop.sln" "MedicShop.sln"

COPY ["Basket.API/Basket.API.csproj", "Basket.API/Basket.API.csproj"]
COPY ["Basket.FunctionalTests/Basket.FunctionalTests.csproj", "Basket.FunctionalTests/Basket.FunctionalTests.csproj"]
COPY ["Basket.UnitTests/Basket.UnitTests.csproj", "Basket.UnitTests/Basket.UnitTests.csproj"]


COPY ["Catalog.API/Catalog.API.csproj", "Catalog.API/Catalog.API.csproj"]
COPY ["Catalog.FunctionalTests/Catalog.FunctionalTests.csproj", "Catalog.FunctionalTests/Catalog.FunctionalTests.csproj"]
COPY ["Catalog.UnitTests/Catalog.UnitTests.csproj", "Catalog.UnitTests/Catalog.UnitTests.csproj"]


COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/EventBusRabbitMQ.csproj"]
COPY ["EventBus/EventBus.csproj", "EventBus/EventBus.csproj"]
COPY ["EventBus.Tests/EventBus.Tests.csproj", "EventBus.Tests/EventBus.Tests.csproj"]

COPY ["Identity.API/Identity.API.csproj", "Identity.API/Identity.API.csproj"]

COPY ["IntegrationEventLogEF/IntegrationEventLogEF.API.csproj", "IntegrationEventLogEF/IntegrationEventLogEF.csproj"]

COPY ["Ordering.API/Ordering.API.csproj", "Ordering.API/Ordering.API.csproj"]
COPY ["Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/Ordering.Domain.csproj"]
COPY ["Ordering.Infastructure/Ordering.Infastructure.csproj", "Ordering.Infastructure/Ordering.Infastructure.csproj"]
COPY ["Ordering.FunctionalTests/Ordering.FunctionalTests.csproj", "Ordering.FunctionalTests/Ordering.FunctionalTests.csproj"]
COPY ["Ordering.UnitTests/Ordering.UnitTests.csproj", "Ordering.UnitTests/Ordering.UnitTests.csproj"]


COPY ["Web.HttpAggregator/Web.HttpAggregator.csproj", "Web.HttpAggregator/Web.HttpAggregator.csproj"]

COPY ["docker-compose.dcproj", "docker-compose.dcproj"]

COPY ["NuGet.config", "NuGet.config"]

RUN dotnet restore "MedicShop.sln"

COPY . .
WORKDIR /src/Basket/Basket.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build as unittest
WORKDIR /src/Basket/Basket.UnitTests

FROM build as functionaltest
WORKDIR /src/Basket/Basket.FunctionalTests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
