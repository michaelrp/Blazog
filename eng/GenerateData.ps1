param(
    $postDir = ".\posts",
    $dataDir = ".\data"
)

# Clear data

Remove-Item "$($dataDir)\indexes\*.*"
Remove-Item "$($dataDir)\posts\*.*"

# Calculate hash

function Get-Hash($text) {
    $hasher = [System.Security.Cryptography.HashAlgorithm]::Create('sha256')
    $hash = $hasher.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($text))
    $hashString = [System.BitConverter]::ToString($hash) | Out-String
    $hashString.Replace('-', '').ToLower().Trim()
}

# Convert posts to json

$index = 0
$postMdFiles = Get-ChildItem "$($postDir)\*.md"
foreach ($file in $postMdFiles)
{
    $metadata = Get-Content $file -First 4
    $title = $metadata[0].Substring(6).Trim()
    $postTags = $metadata[1].Substring(5).Split(",").Trim()
    $dateInput = $metadata[2].Substring(5).Trim()

    # 7/27/2020 6:12 PM
    $date = [datetime]::parseexact($dateInput, 'M/dd/yyyy h:mm tt', $null)

    $blurb = $metadata[3].Substring(6).Trim()

    $label = $file | Select-Object -ExpandProperty BaseName
    $content = Get-Content $file | Select-Object -Skip 4 | Out-String
    $html = ConvertFrom-Markdown -InputObject $content | Select-Object -ExpandProperty Html
    $content = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($html))
    $post = [PSCustomObject]@{
        "index" = $index
        "label" = $label
        "tags" = @($postTags)
        "title" = $title
        "date" = $date
        "blurb" = $blurb
        "content" = $content
        "hash" = Get-Hash($title + @($postTags) + $date + $blurb + $content)
    }
    ConvertTo-Json $post | Out-File -FilePath "$($dataDir)\posts\$($label).json"
    $index += 1
}

# Generate indexes for posts and tags

$posts = New-Object System.Collections.ArrayList
$tagHash = @{}

$postJsonFiles = Get-ChildItem "$($dataDir)\posts\*.json"
foreach ($file in $postJsonFiles)
{
    $postWithContent = Get-Content -Raw -Path $file | ConvertFrom-Json

    $indexPost = [PSCustomObject]@{
        "index" = $postWithContent.Index
        "label" = $postWithContent.Label
        "tags" = @($postWithContent.Tags)
        "title" = $postWithContent.Title
        "date" = $postWithContent.Date
        "blurb" = $postWithContent.Blurb
        "hash" = $postWithContent.Hash
    }

    $r = $posts.Add($indexPost)

    $postTags = $postWithContent.Tags
    foreach ($tag in $postTags) {
        $tagHash[$tag] += 1
    }
}

$posts | 
    Sort-Object -Descending -Property Date |
    ConvertTo-Json |
    Out-File -FilePath "$($dataDir)\indexes\posts.json"

$tagHash.GetEnumerator() | 
    Sort-Object -Property Key |
    Select-Object @{Name="title"; Expression={$_.Key}}, @{Name="postCount"; Expression={$_.Value}} |
    ConvertTo-Json |
    Out-File -FilePath "$($dataDir)\indexes\tags.json"

# Generate a guid to indicate new data

[GUID]::NewGuid().ToString('N') | Out-File -FilePath "$($dataDir)\indexes\guid.txt"