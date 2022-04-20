#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["YaNCo_netcore31/YaNCo_netcore31.csproj", "YaNCo_netcore31/"]
RUN dotnet restore "YaNCo_netcore31/YaNCo_netcore31.csproj"
COPY . .
WORKDIR "/src/YaNCo_netcore31"
RUN dotnet build "YaNCo_netcore31.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YaNCo_netcore31.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV LD_LIBRARY_PATH=/app
ENTRYPOINT ["dotnet", "YaNCo_netcore31.dll"]