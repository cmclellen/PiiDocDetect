# PII Document Detection

### AI Search
#### How to create index, skillset, indexer and datasource
https://learn.microsoft.com/en-us/azure/search/cognitive-search-tutorial-blob

### Develop onboarding
* Create the Service Principal
```
az login --tenant dca5775e-99b4-497c-90c1-c8e73396999e
az ad sp create-for-rbac --json-auth --name PiiDocDetect --role owner --scopes /subscriptions/761399c5-3790-4380-b6a8-a11554fafa7a
```
* Query logs
```
union traces, exceptions | where timestamp > ago(5min) and cloud_RoleName == "func-piiid-prd"
```
