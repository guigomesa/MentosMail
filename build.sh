#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

cd NetCore

dotnet restore

# Ideally we would use the 'dotnet test' command to test netcoreapp and net451 so restrict for now 
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp

dotnet clean

dotnet build

dotnet test ./TestsMentosMailCore/TestsMentosMailCore.csproj -c Release

# Instead, run directly with mono for the full .net version 
# dotnet build ./test/TEST_PROJECT_NAME -c Release -f net451

#revision=${TRAVIS_JOB_ID:=1}  
#revision=$(printf "%04d" $revision) 

dotnet pack