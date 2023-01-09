#!/bin/sh
{
    LATEST=$1
    GREEN='\033[0;32m' # Green
    NC='\033[0m' # No Color

    echo -e "${GREEN}[1/4] - Updating repo...${NC}";
    git pull

    echo -e "${GREEN}[2/4] - Compiling assets...${NC}";
    npm run ng b

    echo -e "${GREEN}[3/4] - Creating fresh docker image...${NC}";
    docker build -t home-box-image .

    echo -e "${GREEN}[4/4] - Creating fresh docker container...${NC}";
    docker compose up -d
}