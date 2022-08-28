#!/bin/bash

# Navigate to the root of the project Infrastructure
cd ../

# Install dotnet-ef if not installed
dotnet tool install --global dotnet-ef

# Migration name is first argument
name=$1

# Print the name of the migration
echo "Creating migration '$name'"

# Create migration
dotnet ef migrations add $name

# Keeps the terminal open
$SHELL