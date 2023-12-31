#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["MPPilot.App/MPPilot.App.csproj", "MPPilot.App/"]
COPY ["MPPilot.Domain/MPPilot.Domain.csproj", "MPPilot.Domain/"]
RUN dotnet restore "MPPilot.App/MPPilot.App.csproj"
COPY . .
WORKDIR "/src/MPPilot.App"
RUN dotnet build "MPPilot.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MPPilot.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MPPilot.App.dll"]