version: '3.8'

services:
  api:
    build:
      context: ./src/api-webapi
      dockerfile: Dockerfile
    ports:
      - "7157:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - DbConnection=your_db_connection_string
      - GeminiAi__ApiBaseUrl=your_gemini_api_url

  webapp:
    build:
      context: ./src/webapp-angular
      dockerfile: Dockerfile
    ports:
      - "4200:80"
    depends_on:
      - api

networks:
  default:
    name: words-network
