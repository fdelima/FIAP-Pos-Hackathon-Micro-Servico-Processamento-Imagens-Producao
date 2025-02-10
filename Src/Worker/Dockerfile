#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER root
RUN mkdir -p /app/temp && chown app:app /app/temp
RUN apt-get update && apt-get install -y ffmpeg
USER app

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/Worker/Worker.csproj", "Src/Worker/"]
COPY ["Src/Core/Ioc/Ioc.csproj", "Src/Core/Ioc/"]
COPY ["Src/Core/Application/Application.csproj", "Src/Core/Application/"]
COPY ["Src/Core/Domain/Domain.csproj", "Src/Core/Domain/"]
COPY ["Src/External/Infra/Infra.csproj", "Src/Adapter/Driven/Infra/"]
RUN dotnet restore "./Src/Worker/Worker.csproj"
COPY . .
WORKDIR "/src/Src/Worker"
RUN dotnet build "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



ENTRYPOINT ["dotnet", "FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.dll"]