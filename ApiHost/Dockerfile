# Define build arguments
ARG ENVIRONMENT_CONFIGURATION
ARG BUILD_CONFIGURATION
ARG CERT_PASSWORD

# Development version
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG ENVIRONMENT_CONFIGURATION
ARG CERT_PASSWORD
USER $APP_UID
WORKDIR /app
EXPOSE 7039
EXPOSE 5092

# Set environment variables for ports
ENV HTTP_PORT=5092
ENV HTTPS_PORT=7039
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERT_PASSWORD
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=aspnet-dev-cert.pfx
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT_CONFIGURATION

# Copy the certificate from the current directory
COPY ./ApiHost/aspnet-dev-cert.pfx /app/aspnet-dev-cert.pfx

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["ApiHost/ApiHost.csproj", "ApiHost/"]
RUN dotnet restore "ApiHost/ApiHost.csproj"
COPY . .
WORKDIR "/src/ApiHost"
RUN dotnet build "ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "ApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiHost.dll"]
