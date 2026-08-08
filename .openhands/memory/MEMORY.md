# Project memory

- GitHub push workaround: this repository's HTTPS `origin` may prompt for credentials even when the injected token is valid. Do not persist the token in Git config; push with an explicit temporary URL instead: `GIT_TERMINAL_PROMPT=0 git push "https://x-access-token:$GITHUB_PERSONAL_ACCESS_TOKEN@github.com/Citr0sCo/home-app.git" refs/heads/<branch>:refs/heads/<branch>`.
- If local tracking is needed after an explicit-URL push, run `GIT_TERMINAL_PROMPT=0 git fetch origin <branch>:refs/remotes/origin/<branch>` followed by `git branch --set-upstream-to=origin/<branch> <branch>`.
- When the dedicated PR tool is unavailable, use the GitHub REST API with the injected bearer token to create the PR; use `convertPullRequestToDraft` through GraphQL if the PR must remain draft.
