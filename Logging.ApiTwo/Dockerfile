#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Logging.ApiTwo/Logging.ApiTwo.csproj", "Logging.ApiTwo/"]
RUN dotnet restore "Logging.ApiTwo/Logging.ApiTwo.csproj"
COPY . .
WORKDIR "/src/Logging.ApiTwo"
RUN dotnet build "Logging.ApiTwo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logging.ApiTwo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logging.ApiTwo.dll"]