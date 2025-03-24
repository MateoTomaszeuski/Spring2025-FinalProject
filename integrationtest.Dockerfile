FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test

WORKDIR /src/.
COPY . .
RUN cd Consilium/Consilium.IntegrationTests && dotnet test