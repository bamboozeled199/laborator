FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src

COPY ["Laborator4.csproj","./"]
RUN dotnet restore "./Laborator4.csproj"

COPY . .
WORKDIR "/src/."

RUN dotnet build "Laborator4.csproj" -c Release -o /app/build
FROM build AS publish

RUN dotnet publish "Laborator4.csproj" -c Release -o /app/publish
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet","Laborator4.dll"]