# Design: Rename NievoEasyFin to NievoEasyFin

## Approach
We will perform a global find-and-replace for the string `NievoEasyFin` to `NievoEasyFin`.

## Steps
1. **File Content Update**:
   Execute a shell command to replace `NievoEasyFin` with `NievoEasyFin` in all files, excluding binary and git directories.
   `grep -rl "NievoEasyFin" . | xargs sed -i 's/NievoEasyFin/NievoEasyFin/g'`

2. **File Renaming**:
   - Check if any files have `NievoEasyFin` in their name.
   - The solution file `Nievo-easyfin.slnx` contains `Nievo-easyfin`. I will rename it to `Nievo-EasyFin.slnx` or similar if appropriate. Actually, following the user's pattern exactly: `Nievo-easyfin` -> `Nievo-easyFin`?
   - Wait, the user said "todas as palavras NievoEasyFin por NievoEasyFin".
   - If I see `Nievo-easyfin`, should I change it to `Nievo-easyFin`? Probably.

3. **Validation**:
   - Run `grep -r "NievoEasyFin" .` again to ensure no occurrences remain.
   - Verify if the project still builds (optional but recommended if I can run dotnet build).

## Risks
- Case-sensitivity in configuration files or external dependencies (unlikely here as it's a project name).
- Broken solution file if renaming is not done carefully.
