FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildBackend
WORKDIR /app
COPY . .
RUN dotnet restore 
RUN dotnet publish -c Release -o out


FROM node:18.12-alpine AS buildFrontend

WORKDIR /app
COPY ./Frontend .
RUN npm i
RUN PUBLIC_BASE_URL="" npm run build



FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled as Final
WORKDIR /home/boardo
# Copy Backend
COPY --from=buildBackend /app/out .
# Copy Frontend
COPY --from=buildFrontend /app/build ./wwwroot/
ENTRYPOINT ["dotnet", "BoarDo.Server.dll"]
