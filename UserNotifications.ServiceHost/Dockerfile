#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserNotifications.ServiceHost/UserNotifications.ServiceHost.csproj", "UserNotifications.ServiceHost/"]
RUN dotnet restore "UserNotifications.ServiceHost/UserNotifications.ServiceHost.csproj"
COPY . .
WORKDIR "/src/UserNotifications.ServiceHost"
RUN dotnet build "UserNotifications.ServiceHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserNotifications.ServiceHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserNotifications.ServiceHost.dll"]