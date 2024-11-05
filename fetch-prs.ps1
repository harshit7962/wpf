Param(
    [string[]][Alias('p')]$prs
)


# Fetch all the PRs first
foreach ( $pr in $prs )
{
    git fetch origin pull/$pr/head:pr-$pr;
    Write-Output("Fetched PR $pr");
}