#!/bin/bash
service nginx start
dotnet /web-api/app/home-box-landing.api.dll
