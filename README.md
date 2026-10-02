# Legacy Test Player

A small server-rendered practice-test app. It stands in for an older .NET test-taking experience: ASP.NET MVC, a SQL database, and a plain HTML front end. It is not a copy of any production system.

Today every student receives the same ten questions in the same order. Each question has a skill and a difficulty so a later change can choose what to show next. Grading runs on the server. The browser never receives the correct answer.

## Prerequisites

- Docker Desktop, or
- the .NET 8 SDK, if you want to run it without Docker

Docker is the path to use in the interview. The first build downloads the .NET images and can take a few minutes. After that, startup takes a few seconds.

## Run with Docker

From this folder:

```bash
docker compose up --build
```

Open http://localhost:8080

Stop it with Ctrl+C, then:

```bash
docker compose down
```

Attempts are stored in a Docker volume named `testplayer-data`. `docker compose down -v` deletes that data.

## Run with the .NET SDK

```bash
dotnet run --project src/LegacyTestPlayer
```

Open the URL printed in the console, usually http://localhost:5080.

The SQLite file `testplayer.db` is created in the project directory.

## Tests

```bash
dotnet test
```

## What you can do in the app

1. Enter a name and start a practice check.
2. Answer ten multiple-choice questions. Everyone sees the same set, in sort order.
3. Submit each answer. The server grades it and stores the response.
4. Open the results page for a score and a count by skill.

There is no login. The name is only a label on the attempt.

## Project layout

```
src/LegacyTestPlayer/        Web app
  Controllers/               Page flow: start, question, answer, results
  Data/                      EF Core context, SQLite, seed data
  Models/                    Tables and the view model sent to the browser
  Services/                  Grading, and the rule that picks the next question
  Views/                     Razor pages
  wwwroot/                   CSS
tests/LegacyTestPlayer.Tests/   Unit tests for grading and question order
```

## Data model

| Table | What it holds |
| --- | --- |
| Questions | Stem, skill, difficulty (1 easiest, 3 hardest), four choices, sort order |
| AnswerKeys | Correct choice for a question. Not loaded into page views |
| Attempts | One practice sitting, with the student's name |
| Responses | The choice submitted, and whether the server marked it correct |

Seed data is in `src/LegacyTestPlayer/Data/CatalogSeeder.cs`. The sample items are fictional. They are not from a real exam.

`SequentialQuestionSelector` picks the next unanswered question by sort order. Replacing that rule is the natural place to choose a question from the student's results.

## Configuration

| Setting | Local default | Docker |
| --- | --- | --- |
| `ConnectionStrings:TestDb` | `Data Source=testplayer.db` | `Data Source=/data/testplayer.db` |
| HTTP port | 5080 | 8080 |

## Current limits

- Multiple choice only. Four choices, labeled A through D.
- One fixed form. Skill and difficulty are stored and shown, and they do not yet change which question comes next.
- No accounts, no timer, and no review of earlier questions.
- SQLite is used so the app starts without a database server. The table shapes are the part worth reading.

## Troubleshooting

- Port 8080 is already in use: change the host port in `docker-compose.yml` (`"8081:8080"`) and open that URL.
- Docker is not running: start Docker Desktop and run `docker compose up --build` again.
- A page error after changing code: the running container does not reload on its own. Rebuild with `docker compose up --build`.
