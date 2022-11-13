FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Educational.Core.WebAPI/Educational.Core.WebAPI.csproj", "Educational.Core.WebAPI/"]
RUN dotnet restore "Educational.Core.WebAPI/Educational.Core.WebAPI.csproj"
COPY . .
WORKDIR /src/Educational.Core.WebAPI
RUN dotnet build "Educational.Core.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Educational.Core.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Educational.Core.WebAPI.dll"]
