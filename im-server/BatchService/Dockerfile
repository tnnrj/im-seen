FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY IMWebAPI.sln ./
COPY IMLibrary/*.csproj ./IMLibrary/
COPY IMWebAPI/*.csproj ./IMWebAPI/
COPY BatchService/*.csproj ./BatchService/
RUN dotnet restore
COPY . .

WORKDIR /src/IMLibrary
RUN dotnet build -c Release -o /app

WORKDIR /src/BatchService
RUN dotnet build -c Release -o /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM build AS publish
RUN dotnet publish -c Release -o /app


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BatchService.dll"]
