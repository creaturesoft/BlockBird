# Force UTF-8 encoding for safe output
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8


function Show-ErrorMessage {
    param([string]$message)
    Add-Type -AssemblyName PresentationFramework
    [System.Windows.MessageBox]::Show($message, "Git Error", 'OK', 'Error')
    exit 1
}

try {
    Write-Host "Staging changes..."
    git add .
    
    Write-Host "Committing..."
    git commit -m "Auto commit"
} catch {
    Show-ErrorMessage "Commit failed: $($_.Exception.Message)"
}

try {
    Write-Host "Pulling latest code with rebase..."
    git pull --rebase -X ours
} catch {
    Show-ErrorMessage "Pull failed: $($_.Exception.Message)"
}

try {
    Write-Host "Pushing to remote..."
    git push
} catch {
    Show-ErrorMessage "Push failed: $($_.Exception.Message)"
}

Write-Host "Done!"
