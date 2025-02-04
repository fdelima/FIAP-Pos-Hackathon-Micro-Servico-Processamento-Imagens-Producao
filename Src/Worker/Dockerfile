#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/Worker/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.csproj", "Src/Worker/"]
COPY ["Src/Core/Ioc/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Ioc.csproj", "Src/Core/Ioc/"]
COPY ["Src/Core/Application/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.csproj", "Src/Core/Application/"]
COPY ["Src/Core/Domain/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.csproj", "Src/Core/Domain/"]
COPY ["Src/External/Infra/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.csproj", "Src/Adapter/Driven/Infra/"]
RUN dotnet restore "./Src/Worker/FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.csproj"
COPY . .
WORKDIR "/src/Src/Worker"
RUN dotnet build "./FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Worker.dll"]