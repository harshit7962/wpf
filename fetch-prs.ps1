Param(
    [string][Alias('n')]$ctpBranchName,
    [string[]][Alias('p')]$prs
)


# Fetch all the PRs first
foreach ( $pr in $prs )
{
    git fetch dotnet pull/$pr/head:pr-$pr;
    Write-Output("Fetched PR $pr");
}


# Create CTP Branch
git checkout release/9.0;
git checkout -b $ctpBranchName;
Write-Output("Created Branch $ctpBranchName");
git checkout release/9.0;
