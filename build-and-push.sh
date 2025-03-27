#!/bin/bash

docker buildx \
    build --platform linux/amd64 \
    -f Dockerfiles/Dockerfile \
    -t mateotomaszeuski/consilium:local_v1 \
    .

docker push mateotomaszeuski/consilium:local_v1
