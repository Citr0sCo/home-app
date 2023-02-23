#!/bin/sh
{
  LATEST=$1
  GREEN='\033[0;32m' # Green
  NC='\033[0m'       # No Color

  echo -e "${GREEN}[1/9] - Stopping docker containers...${NC}"
  docker stop home-app
  docker stop home-app-db

  echo -e "${GREEN}[2/9] - Removing docker containers...${NC}"
  docker rm home-app
  docker rm home-app-db

  echo -e "${GREEN}[3/9] - Removing docker image...${NC}"
  docker image rm home-box-image

  echo -e "${GREEN}[4/9] - Updating repo...${NC}"
  git pull

  echo -e "${GREEN}[4/9] - Switching revision...${NC}"
  git reset --hard $LATEST

  echo -e "${GREEN}[5/9] - Pulling packages...${NC}"
  yarn

  echo -e "${GREEN}[6/9] - Compiling assets...${NC}"
  npm run ng b

  echo -e "${GREEN}[7/9] - Creating fresh docker image...${NC}"
  docker build -t home-box-image .

  echo -e "${GREEN}[8/9] - Creating fresh docker container...${NC}"
  docker compose up -d
}
