FROM mcr.microsoft.com/dotnet/sdk:8.0 AS api-build
WORKDIR /src
COPY ./api-webapi ./api-webapi
COPY ./ApplicationCore ./ApplicationCore
COPY ./Infrastructure ./Infrastructure
RUN dotnet restore ./api-webapi/api-webapi.csproj

WORKDIR /src/api-webapi
RUN dotnet publish -c Release -o /app/publish

FROM node:20 AS angular-build
WORKDIR /src/webapp-angular
COPY ./webapp-angular/package*.json ./
RUN npm install
COPY ./webapp-angular/ .
RUN npm run build --prod

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=api-build /app/publish .
COPY --from=angular-build /src/webapp-angular/dist/webapp-angular ./wwwroot
EXPOSE 80
ENTRYPOINT ["dotnet", "api-webapi.dll"]
# ENTRYPOINT ["tail", "-f", "/dev/null"]