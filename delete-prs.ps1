# Delete branches with the given PR numbers
Param(
    [string[]][Alias('p')]$prs
)

foreach ($pr in $prs) {
    git branch -D pr-$pr;
    Write-Output("Deleted branch pr-$pr");
}