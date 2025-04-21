FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test

WORKDIR /src/.
COPY . .

WORKDIR /src/Consilium

RUN dotnet sln remove Consilium.Maui/Consilium.Maui.csproj
RUN rm -rf Consilium.Maui

RUN dotnet restore
RUN dotnet format --verify-no-changes --no-restore

RUN dotnet build --warnaserror
