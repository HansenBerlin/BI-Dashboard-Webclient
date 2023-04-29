FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MLModel2_WebApi1/MLModel2_WebApi1.csproj", "MLModel2_WebApi1/"]
RUN dotnet restore "MLModel2_WebApi1/MLModel2_WebApi1.csproj"
COPY . .
WORKDIR "/src/MLModel2_WebApi1"
RUN dotnet build "MLModel2_WebApi1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MLModel2_WebApi1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MLModel2_WebApi1.dll"]
