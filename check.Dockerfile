FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test

WORKDIR /src/.
COPY . .

WORKDIR /src/Consilium

RUN dotnet format --verify-no-changes

RUN dotnet build Consilium.API --warnaserror
