# Auto-approve PR script for Azure DevOps Pipeline
# This script automatically approves pull requests based on pipeline conditions

# Check if this is a PR build
if ($env:BUILD_REASON -ne "PullRequest") {
    exit 0
}

# Only approve PRs from dotnet-maestro bot
$allowedAuthors = @('dotnet-maestro[bot]')

# Get PR details
$prApiUrl = "${env:SYSTEM_COLLECTIONURI}${env:SYSTEM_TEAMPROJECTID}/_apis/git/repositories/${env:BUILD_REPOSITORY_ID}/pullRequests/${env:SYSTEM_PULLREQUEST_PULLREQUESTID}?api-version=7.0"
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$env:SYSTEM_ACCESSTOKEN"))
$headers = @{
    'Authorization' = "Basic $base64AuthInfo"
    'Content-Type' = 'application/json'
}
$prDetails = Invoke-RestMethod -Uri $prApiUrl -Method Get -Headers $headers
$prAuthor = $prDetails.createdBy.uniqueName

if (-not $allowedAuthors.Contains($prAuthor)) {
    exit 0
}

# Get PR changes
$changesApiUrl = "${env:SYSTEM_COLLECTIONURI}${env:SYSTEM_TEAMPROJECTID}/_apis/git/repositories/${env:BUILD_REPOSITORY_ID}/pullRequests/${env:SYSTEM_PULLREQUEST_PULLREQUESTID}/changes?api-version=7.0"
$changes = Invoke-RestMethod -Uri $changesApiUrl -Method Get -Headers $headers

$validChange = $false
foreach ($change in $changes.changes) {
    $filePath = $change.item.path
    if ($filePath -like '/eng/*' -or $filePath -eq '/global.json' -or $filePath -eq '/NuGet.config') {
        $validChange = $true
        break
    }
}

if (-not $validChange) {
    exit 0
}

# Create the REST API URL for PR approval
$apiUrl = "${env:SYSTEM_COLLECTIONURI}${env:SYSTEM_TEAMPROJECTID}/_apis/git/repositories/${env:BUILD_REPOSITORY_ID}/pullRequests/${env:SYSTEM_PULLREQUEST_PULLREQUESTID}/reviewers?api-version=7.0"

# Create approval body
$approvalBody = @{
    vote = 10
    isRequired = $false
} | ConvertTo-Json

# Make the API call to approve the PR
$response = Invoke-RestMethod -Uri $apiUrl -Method POST -Body $approvalBody -Headers $headers