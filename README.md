This is the repository for the Quantum Computing tool for CSE 423/485 Capstone for the Spring '24 and Fall '24 semesters. 

The solution currently consists of three projects:
1. the library
2. test cases
3. console app for demos

To build and launch the documentation, first install [DocFX](https://github.com/dotnet/docfx)

Then run the following commands in the terminal:
```
dotnet clean
dotnet build
docfx build
docfx ./docfx.json --serve
```

Then visit the [Docs](https://localhost:8080)
