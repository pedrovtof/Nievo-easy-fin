# Discovery: Rename NievoEasyFin to NievoEasyFin

## Goal
Rename all occurrences of `NievoEasyFin` to `NievoEasyFin` across the entire codebase.

## Findings
- Many namespaces in `.cs` files use `NievoEasyFin` (lowercase 'f').
- Documentation files in `AI/` also use `NievoEasyFin`.
- `env-example.txt` contains `NievoEasyFin` in connection strings and comments.
- Most project directories already use `NievoEasyFin` (uppercase 'F').

## Plan
1. Use `sed` or `replace` to update all file contents.
2. Rename the solution file `Nievo-easyfin.slnx` if necessary (user mentioned "palavras NievoEasyFin", which implies the case mismatch).
3. Verify with a build or grep.
