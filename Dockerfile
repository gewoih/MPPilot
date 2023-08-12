#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["MPPilot.App/MPPilot.App.csproj", "MPPilot.App/"]
COPY ["MPPilot.Domain/MPPilot.Domain.csproj", "MPPilot.Domain/"]
RUN dotnet restore "MPPilot.App/MPPilot.App.csproj"
COPY . .
WORKDIR "MPPilot.App/"
RUN dotnet build "MPPilot.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MPPilot.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MPPilot.App.dll"]