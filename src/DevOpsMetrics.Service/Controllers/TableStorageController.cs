﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpsMetrics.Core.DataAccess.TableStorage;
using DevOpsMetrics.Core.Models.AzureDevOps;
using DevOpsMetrics.Core.Models.Common;
using DevOpsMetrics.Core.Models.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DevOpsMetrics.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableStorageController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IAzureTableStorageDA AzureTableStorageDA;

        public TableStorageController(IConfiguration configuration, IAzureTableStorageDA azureTableStorageDA)
        {
            Configuration = configuration;
            AzureTableStorageDA = azureTableStorageDA;
        }

        // Get builds from the Azure DevOps API, and save new records to the storage table
        [HttpGet("UpdateAzureDevOpsBuilds")]
        public async Task<int> UpdateAzureDevOpsBuilds(
                string organization, string project, string repository, string branch, 
                string buildName, string buildId,
                int numberOfDays, int maxNumberOfItems)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
          
                //Get the PAT token from the settings
                List<AzureDevOpsSettings> settings = AzureTableStorageDA.GetAzureDevOpsSettings(tableStorageConfig, "DevOpsAzureDevOpsSettings", AzureTableStorageDA.CreateAzureDevOpsSettingsPartitionKey(organization, project, repository));
                string patToken = settings[0].PatToken;

                numberOfRecordsSaved = await AzureTableStorageDA.UpdateAzureDevOpsBuilds(patToken, tableStorageConfig, organization, project, branch, buildName, buildId, numberOfDays, maxNumberOfItems);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        // Get builds from the GitHub API
        [HttpGet("UpdateGitHubActionRuns")]
        public async Task<int> UpdateGitHubActionRuns(string clientId, string clientSecret,
                string owner, string repo, string branch, string workflowName, string workflowId,
                int numberOfDays, int maxNumberOfItems)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
                numberOfRecordsSaved = await AzureTableStorageDA.UpdateGitHubActionRuns(clientId, clientSecret, tableStorageConfig,
                        owner, repo, branch, workflowName, workflowId, numberOfDays, maxNumberOfItems);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        [HttpGet("UpdateAzureDevOpsPullRequests")]
        public async Task<int> UpdateAzureDevOpsPullRequests(string patToken,
               string organization, string project, string repository,
               int numberOfDays, int maxNumberOfItems)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
                numberOfRecordsSaved = await AzureTableStorageDA.UpdateAzureDevOpsPullRequests(patToken, tableStorageConfig,
                         organization, project, repository, numberOfDays, maxNumberOfItems);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        [HttpGet("UpdateGitHubActionPullRequests")]
        public async Task<int> UpdateGitHubActionPullRequests(string clientId, string clientSecret,
                string owner, string repo, string branch,
                int numberOfDays, int maxNumberOfItems)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
                numberOfRecordsSaved = await AzureTableStorageDA.UpdateGitHubActionPullRequests(clientId, clientSecret, tableStorageConfig,
                        owner, repo, branch, numberOfDays, maxNumberOfItems);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        [HttpGet("UpdateAzureDevOpsPullRequestCommits")]
        public async Task<int> UpdateAzureDevOpsPullRequestCommits(string patToken,
               string organization, string project, string repository, string pullRequestId,
               int numberOfDays, int maxNumberOfItems)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
                numberOfRecordsSaved = await AzureTableStorageDA.UpdateAzureDevOpsPullRequestCommits(patToken, tableStorageConfig,
                    organization, project, repository, pullRequestId, numberOfDays, maxNumberOfItems);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        [HttpGet("UpdateGitHubActionPullRequestCommits")]
        public async Task<int> UpdateGitHubActionPullRequestCommits(string clientId, string clientSecret,
                string owner, string repo, string pull_number)
        {
            int numberOfRecordsSaved;
            try
            {
                TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
                numberOfRecordsSaved = await AzureTableStorageDA.UpdateGitHubActionPullRequestCommits(clientId, clientSecret, tableStorageConfig,
                        owner, repo, pull_number);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 403 (rate limit exceeded).")
                {
                    numberOfRecordsSaved = -1;
                }
                else
                {
                    throw;
                }
            }
            return numberOfRecordsSaved;
        }

        [HttpGet("GetAzureDevOpsSettings")]
        public List<AzureDevOpsSettings> GetAzureDevOpsSettings(string rowKey = null)
        {
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
            List<AzureDevOpsSettings> settings = AzureTableStorageDA.GetAzureDevOpsSettings(tableStorageConfig, tableStorageConfig.TableAzureDevOpsSettings, rowKey);
            return settings;
        }

        [HttpGet("GetGitHubSettings")]
        public List<GitHubSettings> GetGitHubSettings(string rowKey = null)
        {
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
            List<GitHubSettings> settings = AzureTableStorageDA.GetGitHubSettings(tableStorageConfig, tableStorageConfig.TableGitHubSettings);
            if (rowKey != null)
            {
                return new List<GitHubSettings>
                {
                    settings.Where(x => x.RowKey == rowKey).FirstOrDefault()
                };
            }
            else
            {
                return settings;
            }
        }

        [HttpGet("UpdateAzureDevOpsSetting")]
        public async Task<bool> UpdateAzureDevOpsSetting(string patToken,
                string organization, string project, string repository, string branch, string buildName, string buildId, string resourceGroup, int itemOrder)
        {
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
            return await AzureTableStorageDA.UpdateAzureDevOpsSetting(patToken, tableStorageConfig, tableStorageConfig.TableAzureDevOpsSettings,
                     organization, project, repository, branch, buildName, buildId, resourceGroup, itemOrder);
        }

        [HttpGet("UpdateGitHubSetting")]
        public async Task<bool> UpdateGitHubSetting(string clientId, string clientSecret,
                string owner, string repo, string branch, string workflowName, string workflowId, string resourceGroup, int itemOrder)
        {
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
            return await AzureTableStorageDA.UpdateGitHubSetting(clientId, clientSecret, tableStorageConfig, tableStorageConfig.TableGitHubSettings,
                    owner, repo, branch, workflowName, workflowId, resourceGroup, itemOrder);
        }

        [HttpPost("UpdateDevOpsMonitoringEvent")]
        public async Task<bool> UpdateDevOpsMonitoringEvent([FromBody] MonitoringEvent monitoringEvent)
        {
            TableStorageConfiguration tableStorageConfig = Common.GenerateTableStorageConfiguration(Configuration);
            return await AzureTableStorageDA.UpdateDevOpsMonitoringEvent(tableStorageConfig, monitoringEvent);
        }

    }
}