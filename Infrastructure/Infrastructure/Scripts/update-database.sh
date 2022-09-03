#!/bin/bash

# Navigate to the root of the project Infrastructure
cd ../

# Install dotnet-ef if not installed
dotnet tool install --global dotnet-ef

# Apply the migrations to the database
dotnet ef database update