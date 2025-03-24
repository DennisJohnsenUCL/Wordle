$url = "https://wordfinder.yourdictionary.com/wordle/answers/"

$response = Invoke-WebRequest -Uri $url

$htmlContent = $response.Content

$regex = '<td[^>]*>\s*([A-Z]{5})\s*</td>'

$matches = [regex]::Matches($htmlContent, $regex)

$matches | ForEach-Object {
    $word = $_.Groups[1].Value
    $word | Out-File -Append -FilePath "..\WordleCore\Data\previous_wordles.txt"
}