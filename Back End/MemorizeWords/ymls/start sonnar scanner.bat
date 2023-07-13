cd..
cd MemorizeWords
dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"MemorizeWords" /d:sonar.host.url="http://127.0.0.1:9000"  /d:sonar.token="sqp_5ed4e463f092c3fb270d0af53fc57047ea7ad634"
dotnet build
dotnet sonarscanner end /d:sonar.token="sqp_5ed4e463f092c3fb270d0af53fc57047ea7ad634"