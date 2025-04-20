FROM mcr.microsoft.com/dotnet/sdk:9.0 as build

WORKDIR /src

COPY *.sln .
COPY Presentation.RestApi/*.csproj Presentation.RestApi/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/

RUN dotnet restore

COPY . .
WORKDIR /src/Presentation.RestApi
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
COPY --from=build /app .

EXPOSE 5000

ENTRYPOINT [ "dotnet", "Presentation.RestApi.dll" ]