# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./API/API.csproj ./API/
COPY ./Core/Core.csproj ./Core/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY ./devops-lab-app.sln ./

RUN dotnet restore ./API/API.csproj

COPY . .

WORKDIR /src/API
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "API.dll"]
