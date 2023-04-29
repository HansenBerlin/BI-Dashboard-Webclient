FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinimalApi/MinimalApi.csproj", "MinimalApi/"]
COPY ["BI_Core/BI_Core.csproj", "BI_Core/"]
RUN dotnet restore "MinimalApi/MinimalApi.csproj"
COPY . .
WORKDIR "/src/MinimalApi"
RUN dotnet build "MinimalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApi.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApi.dll"]
