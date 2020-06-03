#!/bin/bash
docker run -d --name coucou -p 7700:7700 getmeili/meilisearch:v0.10.1 ./meilisearch --master-key=masterKey
sleep 2
dotnet build -c Release
dotnet test
docker rm -f coucou
