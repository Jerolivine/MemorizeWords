cd..
cd..
cd..
cd "Back End"
cd MemorizeWords
cd MemorizeWords
dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"MemorizeWords" /d:sonar.host.url="http://127.0.0.1:9000"  /d:sonar.token="sqp_62bc1789216859d89b6c6b918c41a92010440305"
dotnet build
dotnet sonarscanner end /d:sonar.token="sqp_62bc1789216859d89b6c6b918c41a92010440305"