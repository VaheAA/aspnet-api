FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GameStore.Api.csproj .
RUN dotnet restore GameStore.Api.csproj

COPY . .
RUN dotnet publish GameStore.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
RUN mkdir -p /app/data && chown -R app:app /app
USER app

EXPOSE 8080
ENTRYPOINT ["dotnet", "GameStore.Api.dll"]
