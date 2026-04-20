FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["IsoTreatment.ApiGateway/IsoTreatment.ApiGateway.csproj", "IsoTreatment.ApiGateway/"]
RUN dotnet restore "IsoTreatment.ApiGateway/IsoTreatment.ApiGateway.csproj"

COPY . .
WORKDIR /src/IsoTreatment.ApiGateway
RUN dotnet publish "IsoTreatment.ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IsoTreatment.ApiGateway.dll"]