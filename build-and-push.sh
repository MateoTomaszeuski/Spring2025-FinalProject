#!/bin/bash

docker buildx \
    build --platform linux/amd64 \
    -t mateotomaszeuski/consilium \
    .

docker push mateotomaszeuski/consilium