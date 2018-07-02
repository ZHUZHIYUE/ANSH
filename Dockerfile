# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /ANSH
COPY ./dist/MVC.ANSH/ ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]