FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test

WORKDIR /src/.
COPY . .

WORKDIR /src/Consilium/Consilium.Tests

ENTRYPOINT ["dotnet", "test"]