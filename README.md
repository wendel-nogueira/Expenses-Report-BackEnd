<h2 align="center">
    Expenses Report API - Hands On
</h2>

<div align="center">

![GitHub repo size](https://img.shields.io/github/repo-size/wendel-nogueira/Expenses-Report-BackEnd?style=for-the-badge)
![GitHub language count](https://img.shields.io/github/languages/count/wendel-nogueira/Expenses-Report-BackEnd?style=for-the-badge)
![GitHub forks](https://img.shields.io/github/forks/wendel-nogueira/Expenses-Report-BackEnd?style=for-the-badge)
![GitHub open issues](https://img.shields.io/github/issues/wendel-nogueira/Expenses-Report-BackEnd?style=for-the-badge)
![GitHub open pull requests](https://img.shields.io/github/issues-pr/wendel-nogueira/Expenses-Report-BackEnd?style=for-the-badge)

</div>

Expenses Report is a project that is being developed as part of the Hands On project, with its primary goal being the development of an expense report application. This repository aims to provide an API based on microservices architecture, which is consumed by the [Front End](https://github.com/wendel-nogueira/Expenses-Report-FrontEnd).

## 💻 Prerequisites

Before you begin, make sure you have met the following requirements:

* You have installed the latest version of [Docker](https://www.docker.com) on your machine.

## 🚀 Installing Expenses Report API

To install Expenses Report API, follow these steps:

* Clone the repository to a folder on your computer.

* Open the project folder.

* Rename the file .env.example to .env.

* Open the terminal.

* Navigate to the folder in your terminal:

```
cd path/to/project
```

* Run the docker-compose command to create the project's containers:

```
docker-compose up -d --build
```

After this, the project will be up and running 😊

## ☕ Using Expenses Report API

To use the API, simply open your web browser and navigate to:

* [http://localhost:5000/swagger](http://localhost:5000/swagger) - for user microservice.


## 📫 Contributing to Expenses Report API

To contribute to Expenses Report API, follow these steps:

1. Fork this repository.
2. Create a branch: `git checkout -b <branch_name>`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`.
4. Push to the original branch: `git push origin <branch_name>`.
5. Create the pull request.

Alternatively, refer to GitHub documentation on [how to create a pull request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## 📝 Tasks

Below, you'll find all open and completed tasks during the project implementation.

- [ ] Implement identity microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with the database storage (MySQL).
    - [ ] Implement tests.
    - [ ] Provide a means of authentication and authorization for other microservices.
    - [ ] Deploy to Azure AKS.
- [ ] Implement users microservice.
    - [x] Implement application routes.
    - [x] Integrate with the database storage (MySQL).
    - [x] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement projects microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with the database storage (MySQL).
    - [ ] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement departments microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with the database storage (MySQL).
    - [ ] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement expenses microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with the database storage (Azure Cosmos DB).
    - [ ] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement export microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with the database storage (Azure Cosmos DB).
    - [ ] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement file upload microservice.
    - [ ] Implement application routes.
    - [ ] Integrate with file storage (Azure Files).
    - [ ] Implement tests.
    - [ ] Implement authentication and authorization for the routes.
    - [ ] Deploy to Azure AKS.
- [ ] Implement API gateway.
    - [ ] Implement an API gateway service.
    - [ ] Integrate with all microservices.
    - [ ] Deploy to Azure AKS.