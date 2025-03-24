FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test

WORKDIR /src/.
COPY . .
RUN cd Consilium/Consilium.Tests && dotnet test