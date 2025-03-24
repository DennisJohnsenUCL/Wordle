$inputFile = "..\WordleCore\Data\allowed_words.txt"

$outputFile = "..\WordleCore\Data\allowed_words.txt"

$fileContent = Get-Content $inputFile

$uppercaseContent = $fileContent | ForEach-Object { $_.ToUpper() }

$uppercaseContent | Set-Content $outputFile

Write-Host "File content has been converted to uppercase and saved to $outputFile"

Read-Host -prompt "Press any key to exit"