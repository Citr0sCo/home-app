#!/bin/sh
{
  LATEST=$1
  GREEN='\033[0;32m' # Green
  NC='\033[0m'       # No Color

  echo -e "${GREEN}[1/7] - Preparing repo...${NC}"
  git reset --hard

  echo -e "${GREEN}[2/7] - Updating repo...${NC}"
  git pull

  #echo -e "${GREEN}[3/9] - Pulling packages...${NC}"
  #yarn

  #echo -e "${GREEN}[4/9] - Compiling assets...${NC}"
  #npm run ng b

  echo -e "${GREEN}[3/7] - Stopping docker containers...${NC}"
  docker stop home-app

  echo -e "${GREEN}[4/7] - Removing docker containers...${NC}"
  docker rm home-app

  echo -e "${GREEN}[5/7] - Removing docker image...${NC}"
  docker image rm home-box-image

  echo -e "${GREEN}[6/7] - Creating fresh docker image...${NC}"
  docker build -t home-box-image .

  echo -e "${GREEN}[7/7] - Creating fresh docker container...${NC}"
  docker compose up -d
}