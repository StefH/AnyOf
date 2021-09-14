rem https://github.com/StefH/GitHubReleaseNotes

SET version=0.0.21

GitHubReleaseNotes --output ReleaseNotes.md --exclude-labels question invalid doc --version %version%

GitHubReleaseNotes --output PackageReleaseNotes.txt --exclude-labels question invalid doc --template PackageReleaseNotes.template --version %version%