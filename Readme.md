# Trainee Management System

A Trainee management system API to manage all trainee records by performing CRUD operations through REST APIs. The system is built using .NET and EF in-memory database

## Tech Stack

ASP.NET, OpenAPI / Swagger, EF Core


## How to Run

Go to the project directory. First install all required packages.
```bash
  dotnet restore
```
To launch the project in development.
```bash
 dotnet run --launch-profile https    
```
To launch in watch mode.
```bash
  dotnet watch --launch-profile https    
```




## API Reference

#### Health check of application

```http
  GET /api/Health
```

#### Interactive Swagger UI for testing of routes
```http
  GET /swagger
```

#### Get all Trainees with optional search query 

```http
  GET /api/Trainees
```

| query | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `search` | `string` | **Optional** It checks whether first name, last name, tech stack, email contains the search string.  |

#### Get Trainee by id

```http
  GET /api/Trainees/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `long` | **Required**. Id of trainee to fetch |

#### Add Trainee 
```http
  POST /api/Trainees
```
Request Body
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `firstName`      | `string` | **Required**. First name max 50 characters. |
| `lastName`      | `string` | **Required**. Last name max 50 |
| `email`      | `string` | **Required**. Valid email. |
| `techStack`      | `string` | **Required**. |
| `status`      | `string` | **Required**. Status in 'Active', 'Inactive' |

#### Update Trainee 
```http
  PUT /api/Trainees/${id}
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `long` | **Required**. Id of trainee to update |

Request Body
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `firstName`      | `string` | **Required**. First name max 50 characters. |
| `lastName`      | `string` | **Required**. Last name max 50 characters. |
| `email`      | `string` | **Required**. Valid email. |
| `techStack`      | `string` | **Required**. |
| `status`      | `string` | **Required**. status in 'Active', 'Inactive' |

#### Delete Trainee 
```http
  DELETE /api/Trainees/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `long` | **Required**. Id of trainee to be deleted |

## Sample Request JSON

```bash
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "techStack": "string",
  "status": "string"
}
```

## Sample Response JSON

```bash
{
  "id": "long",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "techStack": "string",
  "status": "string"
}
```

## Known limitations

The data is temporarily stored inside in-memory database. Once the server restarts the data gets lost. 