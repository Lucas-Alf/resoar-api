#!/bin/bash

# Navigate to the root of the project Infrastructure
cd ../

# Install dotnet-ef if not installed
dotnet tool install --global dotnet-ef

# Print the name of the migration
echo "Removing the last migration"

# Create migration
dotnet ef migrations remove

# Keeps the terminal open
$SHELL