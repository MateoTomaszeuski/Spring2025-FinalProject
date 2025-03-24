FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src/.
COPY . .


RUN dotnet publish ./Consilium/Consilium.API/Consilium.API.csproj -c Release -o /app/publish
WORKDIR /app/publish

ENTRYPOINT ["dotnet", "Consilium.API.dll"]