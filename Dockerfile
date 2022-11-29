FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AnonymousChat.MVC/AnonymousChat.MVC.csproj", "AnonymousChat.MVC/"]
COPY ["AnonymousChat.Domain/AnonymousChat.Domain.csproj", "AnonymousChat.Domain/"]
COPY ["AnonymousChat.Persistence/AnonymousChat.Persistence.csproj", "AnonymousChat.Persistence/"]
COPY ["AnonymousChat.Application/AnonymousChat.Application.csproj", "AnonymousChat.Application/"]
RUN dotnet restore "AnonymousChat.MVC/AnonymousChat.MVC.csproj"
RUN dotnet restore "AnonymousChat.Application/AnonymousChat.Application.csproj"
RUN dotnet restore "AnonymousChat.Domain/AnonymousChat.Domain.csproj"
RUN dotnet restore "AnonymousChat.Persistence/AnonymousChat.Persistence.csproj"
COPY . .
WORKDIR "/src/AnonymousChat.MVC"
RUN dotnet build "AnonymousChat.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnonymousChat.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnonymousChat.MVC.dll"]
