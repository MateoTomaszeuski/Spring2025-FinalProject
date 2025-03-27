FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src/.
COPY . .

WORKDIR /src/Consilium/Consilium.API
RUN dotnet publish Consilium.API.csproj -c Release -o /app/publish
WORKDIR /app/publish

ENTRYPOINT ["dotnet", "Consilium.API.dll"]