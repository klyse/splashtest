# Splash Test

Hackathon Project page: https://hackathon.bz.it/project/splashtest

# Folder structure

    .
    ├── backend/web/      # asp.net core 6 webservice
    ├── backend/cypress/  # cypress runtime
    ├── frontend/         # react frontend
    ├── .envrc            # direnv config file to load nvm and other setup scripts
    └── .nvmrc            # node version manager (nvm)


# How to run

## Requrements

- [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [node 18](https://nodejs.org/en/blog/release/v18.0.0/)

Optional:
- [direnv](https://direnv.net)
- [nvm](https://github.com/nvm-sh/nvm)

## Backend

Start postgres with docker compose:

`docker compose up`

Navigate to the `backend/web` folder and run `dotnet run` to start the asp.net core API.

Navigate to [http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html) to see the swagger specs.

### Init Cypress

Navigate to the `backend/cypress` folder and execute `npm install`.

## Frontend

Navigate to the `frontend` folder and run `npm install` and `npm start`

Navigate to [http://localhost:3000](http://localhost:3000) to access the frontend.

# Screenshots

![homepage](images/homepage.png)


![end to end](images/e2e.gif)

Easy creation of new UI tests. We use `cypress` as test framework and are therefore extremely flexible. New functionallity can be added very quickly:

![create-test](images/create-test.png)

See all your run tests and watch a small video with the result:

![list of test runs](images/list-of-test-run.png)

We have integrated loadtesting with `k6`:

![load test](images/load-test.gif)
