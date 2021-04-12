﻿using DevOpsMetrics.Core.Models.AzureDevOps;
using DevOpsMetrics.Core.Models.Common;
using DevOpsMetrics.Core.Models.GitHub;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsMetrics.Core.DataAccess.TableStorage
{
    public interface IAzureTableStorageDA
    {
        string CreateAzureDevOpsPRCommitPartitionKey(string organization, string project, string pullRequestId);
        string CreateAzureDevOpsPRPartitionKey(string organization, string project);
        string CreateAzureDevOpsSettingsPartitionKey(string organization, string project, string repository);
        string CreateBuildWorkflowPartitionKey(string organization_owner, string project_repo, string buildName_workflowName);
        string CreateGitHubPRCommitPartitionKey(string owner, string repo, string pullRequestId);
        string CreateGitHubPRPartitionKey(string owner, string repo);
        string CreateGitHubSettingsPartitionKey(string owner, string repo, string workflowName);
        List<AzureDevOpsSettings> GetAzureDevOpsSettings(TableStorageConfiguration tableStorageConfig, string settingsTable, string rowKey);
        List<GitHubSettings> GetGitHubSettings(TableStorageConfiguration tableStorageConfig, string settingsTable);
        JArray GetTableStorageItems(TableStorageConfiguration tableStorageConfig, string tableName, string partitionKey);
        Task<int> UpdateAzureDevOpsBuilds(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string branch, string buildName, string buildId, int numberOfDays, int maxNumberOfItems);
        Task<int> UpdateAzureDevOpsPullRequestCommits(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string repository, string pullRequestId, int numberOfDays, int maxNumberOfItems);
        Task<int> UpdateAzureDevOpsPullRequests(string patToken, TableStorageConfiguration tableStorageConfig, string organization, string project, string repository, int numberOfDays, int maxNumberOfItems);
        Task<bool> UpdateAzureDevOpsSetting(string patToken, TableStorageConfiguration tableStorageConfig, string settingsTable, string organization, string project, string repository, string branch, string buildName, string buildId, string resourceGroupName, int itemOrder);
        Task<int> UpdateChangeFailureRate(TableStorageCommonDA tableChangeFailureRateDA, ChangeFailureRateBuild newBuild, string partitionKey, bool forceUpdate = false);
        Task<bool> UpdateDevOpsMonitoringEvent(TableStorageConfiguration tableStorageConfig, MonitoringEvent monitoringEvent);
        Task<int> UpdateGitHubActionPullRequestCommits(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string pull_number);
        Task<int> UpdateGitHubActionPullRequests(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string branch, int numberOfDays, int maxNumberOfItems);
        Task<int> UpdateGitHubActionRuns(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string owner, string repo, string branch, string workflowName, string workflowId, int numberOfDays, int maxNumberOfItems);
        Task<bool> UpdateGitHubSetting(string clientId, string clientSecret, TableStorageConfiguration tableStorageConfig, string settingsTable, string owner, string repo, string branch, string workflowName, string workflowId, string resourceGroupName, int itemOrder);
    }
}