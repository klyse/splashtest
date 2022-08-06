# Splashtest

Project page: https://hackathon.bz.it/project/splashtest


# How to run

## Requrements

```
dotnet 6.0
nodejs 18
```

## Backend

Start postgres with docker compose:

`docker compose up`

Navigate to the `backend/web` folder and run `dotnet run` to start the asp.net core API.

Navigate to `http://localhost:5001/swagger/index.html` to see the swagger specs.

## Frontend

Navigate to the `frontend` folder and run `npm start`