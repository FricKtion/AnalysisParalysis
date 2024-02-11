FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY AnalysisParalysis.csproj AnalysisParalysis.csproj
RUN dotnet restore AnalysisParalysis.csproj
COPY . .
RUN dotnet publish -c Release -o /output --no-restore --nologo
