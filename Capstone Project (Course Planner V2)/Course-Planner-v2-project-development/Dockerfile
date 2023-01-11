FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . .
RUN dotnet dev-certs https
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "CoursePlanner.dll"]
