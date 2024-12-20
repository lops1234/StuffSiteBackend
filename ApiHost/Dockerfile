# Development version
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7039
EXPOSE 5092

# Set environment variables for ports
ENV HTTP_PORT=5092
ENV HTTPS_PORT=7039
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=ola1234
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=aspnet-dev-cert.pfx
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the certificate from the current directory
COPY ./ApiHost/aspnet-dev-cert.pfx /app/aspnet-dev-cert.pfx

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["ApiHost/ApiHost.csproj", "ApiHost/"]
RUN dotnet restore "ApiHost/ApiHost.csproj"
COPY . .
WORKDIR "/src/ApiHost"
RUN dotnet build "ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
# Development version
ARG BUILD_CONFIGURATION=Development 
RUN dotnet publish "ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiHost.dll"]
