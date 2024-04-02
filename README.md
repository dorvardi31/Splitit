# IMDB and Ranker Actors API Scraper

This project scrapes actor data from IMDB and Ranker, stores it in an in-memory database, and provides an API to interact with this data. Ideal for those looking to analyze actor popularity, compare data across sources, or simply fetch actor information programmatically.

## Description

This API offers endpoints to get actor lists, add new actors, update existing ones, and delete actors. It uses .NET 6 for backend services, making it easy to set up and run on any platform supporting .NET Core.

## Getting Started

### Dependencies

- .NET 8.0 SDK
- An IDE like Visual Studio Code or Visual Studio 2022
- Postman or any API testing tool for API interaction

### Installing

Clone the repository:

```bash
git clone [https://github.com/yourusername/yourprojectname.git](https://github.com/dorvardi31/Splitit.git)
cd Splitit
```
###Executing program
Restore dependencies, build the solution, and run the project:
```bash
dotnet restore
dotnet build
dotnet run
```

API Reference
###Get Actors
GET /api/actors
Request:
```bash
curl -X 'GET' \
  'https://localhost:7056/api/Actors?name=Tom&pageNumber=1&pageSize=10' \
  -H 'accept: text/plain'
```
Response:

{
  "statusCode": 200,
  "message": "Actors fetched successfully.",
  "data": [
    {
      "id": 7,
      "rank": 7,
      "name": "Tom Hanks",
      "type": "Producer",
      "description": "Thomas Jeffrey Hanks was born in Concord, California, to Janet Marylyn (Frager), a hospital worker, and Amos Mefford Hanks, an itinerant cook. His mother's family, originally surnamed \"Fraga\", was entirely Portuguese, while his father was of mostly English ancestry. Tom grew up in what he has ...",
      "source": "IMDb"
    },
    {
      "id": 24,
      "rank": 24,
      "name": "Tommy Lee Jones",
      "type": "Actor",
      "description": "Tommy Lee Jones was born in San Saba, Texas, the son of Lucille Marie (Scott), a police officer and beauty shop owner, and Clyde C. Jones, who worked on oil fields. Tommy himself worked in underwater construction and on an oil rig. He attended St. Mark's School of Texas, a prestigious prep school ...",
      "source": "IMDb"
    }

  ],
  "errors": null
}

###GET /actors/{id}
Request:
```bash
curl -X 'GET' \
  'https://localhost:7056/api/Actors/111' \
  -H 'accept: text/plain'
```
Response:

{
  "statusCode": 200,
  "message": "Actor fetched successfully.",
  "data": {
    "id": 111,
    "rank": 11,
    "name": "Hugh Grant",
    "type": "Actor",
    "description": "Hugh Grant, one of Britain's best known faces, has been equally entertaining on-screen as well as in real life, and has had enough sense of humor to survive a media frenzy. He is known for his roles in Four Weddings and a Funeral (1994), with Andie MacDowell, Notting Hill (1999), opposite Julia ...",
    "source": "IMDb"
  },
  "errors": null
}



###POST /actors
Request:
```bash
curl -X 'POST' \
  'https://localhost:7056/api/Actors' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 1000,
  "rank": 1000,
  "name": "a",
  "type": "PROGRAMMER",
  "description": "test",
  "source": "SPLITIT"
}'
```
Response:

{
  "statusCode": 200,
  "message": "Actor added successfully",
  "data": {
    "id": 826,
    "rank": 1000,
    "name": "a",
    "type": "PROGRAMMER",
    "description": "test",
    "source": "SPLITIT"
  },
  "errors": null
}

###PUT /actors/{id}
Request:
```bash
curl -X 'PUT' \
  'https://localhost:7056/api/Actors/1' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 1,
  "rank": 999,
  "name": "Jack Nicholson",
  "type": "ballerina",
  "description": "no history",
  "source": "IMDB"
}'
```
Response:

{
  "statusCode": 200,
  "message": "Actor updated successfully",
  "data": {
    "id": 1,
    "rank": 999,
    "name": "Jack Nicholson",
    "type": "ballerina",
    "description": "no history",
    "source": "IMDB"
  },
  "errors": null
}
###DELETE /actors/{id}
Request:
```bash
curl -X 'DELETE' \
  'https://localhost:7056/api/Actors/1' \
  -H 'accept: */*'
```
Response:

{
  "statusCode": 200,
  "message": "Actor deleted successfully",
  "data": null,
  "errors": null
}

##Authors
Dor Vardi
