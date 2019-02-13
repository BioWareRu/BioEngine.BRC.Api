FROM microsoft/dotnet:2.2.2-aspnetcore-runtime

WORKDIR /app
COPY . .
HEALTHCHECK --interval=30s --timeout=5s --retries=1 CMD curl --silent --fail http://localhost:80/health || exit 1
CMD ["dotnet", "BioEngine.BRC.Api.dll"]
