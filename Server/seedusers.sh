#!/bin/bash
source ./env.sh
docker-compose run --rm --entrypoint "\
  dotnet AuthServer.dll /seed /tmpusr/userdata.json
" auth