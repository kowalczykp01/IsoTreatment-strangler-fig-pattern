FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["IsoTreatment.Users/src/Api/Api.csproj", "IsoTreatment.Users/src/Api/"]
RUN dotnet restore "IsoTreatment.Users/src/Api/Api.csproj"

COPY . .
WORKDIR /src/IsoTreatment.Users/src/Api
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]