# Blazog

A static Blazor blog that reads converted Markdown content from Blob storage, GitHub, S3, or where ever you like.

## Local Testing

To run locally, generate some post data by running

```pwsh
.\eng\GenerateData.ps1 .\posts .\src\Blazog\wwwroot\data
```

at the repo root.

This adds post files to the local static directory. Make sure `"RootUrl": "data"` is set in `appsettings.Development.json`.
