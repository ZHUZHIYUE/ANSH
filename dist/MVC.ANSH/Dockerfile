# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /ANSH
COPY . ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]