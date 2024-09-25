Param(
    [string][Alias('n')]$ctpBranchName,
    [string[]][Alias('p')]$prs
)


# Fetch all the PRs first
foreach ( $pr in $prs )
{
    git fetch origin pull/$pr/head:pr-$pr;
    Write-Output("Fetched PR $pr");
}


# Create CTP Branch
# git checkout main;
# git checkout -b $ctpBranchName;
# Write-Output("Created Branch $ctpBranchName");
# git checkout main;
