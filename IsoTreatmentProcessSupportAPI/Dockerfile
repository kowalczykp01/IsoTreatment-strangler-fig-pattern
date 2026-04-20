FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IsoTreatmentProcessSupportAPI/IsoTreatmentProcessSupportAPI.csproj", "IsoTreatmentProcessSupportAPI/"]
RUN dotnet restore "IsoTreatmentProcessSupportAPI/IsoTreatmentProcessSupportAPI.csproj"

COPY . .
WORKDIR /src/IsoTreatmentProcessSupportAPI
RUN dotnet publish "IsoTreatmentProcessSupportAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IsoTreatmentProcessSupportAPI.dll"]