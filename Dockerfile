FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AnalysisParalysis.csproj", "."]
RUN dotnet restore "AnalysisParalysis.csproj"
COPY . .
RUN dotnet build "AnalysisParalysis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalysisParalysis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnalysisParalysis.dll"]
