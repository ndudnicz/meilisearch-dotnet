#!/bin/bash
docker run -d --name coucou -p 7700:7700 getmeili/meilisearch:v0.10.1 ./meilisearch --master-key=masterKey
