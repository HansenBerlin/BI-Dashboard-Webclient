FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BI_Dashboard_Webclient/BI_Dashboard_Webclient.csproj", "BI_Dashboard_Webclient/"]
COPY ["BI_Core/BI_Core.csproj", "BI_Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "BI_Dashboard_Webclient/BI_Dashboard_Webclient.csproj"
COPY . .
WORKDIR "/src/BI_Dashboard_Webclient"
RUN dotnet build "BI_Dashboard_Webclient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BI_Dashboard_Webclient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BI_Dashboard_Webclient.dll"]
