FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api-image-processor-aws/api-image-processor-aws.csproj", "api-image-processor-aws/"]
RUN dotnet restore "api-image-processor-aws/api-image-processor-aws.csproj"
COPY . .
WORKDIR "/src/api-image-processor-aws"
RUN dotnet build "./api-image-processor-aws.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api-image-processor-aws.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api-image-processor-aws.dll"]
