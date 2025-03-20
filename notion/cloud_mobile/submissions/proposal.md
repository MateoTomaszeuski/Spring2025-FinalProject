## Project Premise & Chosen Domain

Consilium: A student time management app, with a social twist!

Our app aims to help students organize their courses and assignments through custom to-do lists, productivity insights, and additional study tools like a Pomodoro timer and GPA calculator.

The app will optionally display statistics based on the student’s workflow. These statistics may include: 

- Time spent on each class per week
- Average time to complete assignments
- Procrastination trends—how close to the deadline assignments are typically started

Social features, like adding friends and sharing progress, will motivate students to stick to their goals and complete items on their to-do list.

---

## Cloud Services

### Data Storage

We’ll use Azure Postgres for our database.

### Compute

Azure Kubernetes (AKS) will host the following microservices:

- API
- OTEL
- Loki
- Grafana
- Prometheus

### Everything Else

A static site will point users to the app and demonstrate how it works.

---

## What Parts of Your Solution Will Be Running Where

Static website runs in the Static Web App service. API runs in AKS. The MAUI app runs locally on the user’s device.

---

## Data Management

### Cached

Data that needs to be cached on the mobile app:

- Authentication
- Current state of the user’s to-do list

### Server

The API will be responsible for communication between the database and the client app. Information pertaining to assignments and courses (and data relevant to the calculation of statistics) will be persisted.

---

## Components

### Shared Components

Our static website will stand on its own, eventually including a link to the app store to download the mobile app. No components will be shared with the mobile app.