# 🚀 Release v{{buildDetails.buildNumber}}

## Associated Pull Requests ({{pullRequests.length}})
{{#forEach pullRequests}}
* Pull Request [#{{this.pullRequestId}} {{this.title}}]({{replace (replace this.url "_apis/git/repositories" "_git") "pullRequests" "pullRequest"}})
{{/forEach}}

## Associated Azure Board Items ({{workItems.length}})
{{#forEach this.workItems}}
*  Item [#{{this.id}} {{lookup this.fields 'System.Title'}}]({{replace this.url "_apis/wit/workItems" "_workitems/edit"}})
{{/forEach}}

## Assets
* **[Release v{{buildDetails.buildNumber}} as ZIP]({{replace buildDetails.project.url "_apis/projects/" ""}}/_apis/git/repositories/{{buildDetails.repository.id}}/items?path=/&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=1&versionDescriptor%5Bversion%5D=v{{buildDetails.buildNumber}}&resolveLfs=true&%24format=zip&api-version=5.0&download=true)**

---