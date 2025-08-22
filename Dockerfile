# مرحله Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# کپی csproj ها و restore
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY LibraryManagement/LibraryManagement.csproj LibraryManagement/
RUN dotnet restore LibraryManagement/LibraryManagement.csproj
WORKDIR /src/LibraryManagement
RUN dotnet publish -c Release -o /app

# مرحله Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "LibraryManagement.dll"]
