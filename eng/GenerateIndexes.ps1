$dataDir = ".\data"

# delete existing posts.json file
# Remove-Item "$($dataDir)\posts\posts.json"

# iterate through the data dir and make array of posts
$posts = New-Object System.Collections.ArrayList
$tagHash = @{}

$files = Get-ChildItem "$($dataDir)\posts\*.json"
foreach ($file in $files)
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
    Out-File -FilePath "$($dataDir)\posts\posts.json"

$tagHash.GetEnumerator() | 
    Sort-Object -Property Key |
    Select-Object @{Name="Title"; Expression={$_.Key}}, @{Name="PostCount"; Expression={$_.Value}} |
    ConvertTo-Json |
    Out-File -FilePath "$($dataDir)\tags\tags.json"
