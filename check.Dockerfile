FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test

WORKDIR /src/.
COPY . .

WORKDIR /src/Consilium

RUN dotnet restore

RUN dotnet format --verify-no-changes --no-restore

RUN dotnet build Consilium.API --warnaserror
