$postDir = ".\posts"
$dataDir = ".\data"

# Clear data

Remove-Item "$($dataDir)\indexes\*.*"
Remove-Item "$($dataDir)\posts\*.*"

# Convert posts to json

$index = 0
$postMdFiles = Get-ChildItem "$($postDir)\*.md"
foreach ($file in $postMdFiles)
{
    $metadata = Get-Content $file -First 3
    $title = $metadata[0].Substring(6).Trim()
    $tags = $metadata[1].Substring(5).Replace(", ", ",").Trim()
    $date = $metadata[2].Substring(5).Trim()

    $label = $file | Select-Object -ExpandProperty BaseName
    $content = Get-Content $file | Select-Object -Skip 3 | Out-String
    $mainHtml = ConvertFrom-Markdown -InputObject $content | Select-Object -ExpandProperty Html
    $mainContent = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($mainHtml))
    $post = Select-Object `
        @{ Name = "Index"; Expression={$index} }, `
        @{n='Label';e={$label}}, `
        @{n='Tags';e={$tags}}, `
        @{n='Title';e={$title}}, `
        @{n='Date';e={$date}}, `
        @{n='Content';e={$mainContent}} `
        -InputObject ''
    ConvertTo-Json $post | Out-File -FilePath "$($dataDir)\posts\$($label).json"
    $index += 1
}

# Generate indexes for posts and tags

$posts = New-Object System.Collections.ArrayList
$tagHash = @{}

$postJsonFiles = Get-ChildItem "$($dataDir)\posts\*.json"
foreach ($file in $postJsonFiles)
{
    $post = Get-Content -Raw -Path $file | ConvertFrom-Json
    $r = $posts.Add($post)

    $postTags = $post.Tags.Split(',')
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
    Select-Object @{Name="Title"; Expression={$_.Key}}, @{Name="PostCount"; Expression={$_.Value}} |
    ConvertTo-Json |
    Out-File -FilePath "$($dataDir)\indexes\tags.json"

# Generate a guid to indicate new data

[GUID]::NewGuid().ToString('N') | Out-File -FilePath "$($dataDir)\indexes\guid.txt"