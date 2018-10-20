using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DataWorld.Client.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataWorld.Client
{
    public class DataWorldClient
    {
        private readonly string _readWriteApiKey;
        private readonly string _adminApiKey;

        private readonly JsonSerializerSettings _serializerSettings;

        private const string ApiVersion = "v0";

        public DataWorldClient(string readWriteApiKey, string adminApiKey)
        {
            _readWriteApiKey = readWriteApiKey;
            _adminApiKey = adminApiKey;

            _serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        public async Task<User> GetUser()
        {
            using (var http = CreateClient())
            {
                var response = await http.GetAsync("user");
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(content);
            }
        }

        /// <summary>
        /// Get a list of datasets the current logged in user either owns or contributes to.
        /// </summary>
        /// <param name="type">Either "own" or "contributing"</param>
        /// <param name="limit">The number of results to return.</param>
        /// <param name="next">The prior value of the next token, if you're paginating.</param>
        /// <returns></returns>
        public async Task<PaginatedResult<Dataset>> ListDatasets(string type = "own", int limit = 50, string next = null)
        {
            using (var http = CreateClient())
            {
                var url = $"user/datasets/{type}?limit={limit}";

                if (!String.IsNullOrWhiteSpace(next)) url = $"{url}&next={next}";
                
                var response = await http.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PaginatedResult<Dataset>>(content);
            }
        }

        /// <summary>
        /// Get a list of projects the current logged in user either owns or contributes to.
        /// </summary>
        /// <param name="type">Either "own" or "contributing"</param>
        /// <param name="limit">The number of results to return.</param>
        /// <param name="next">The prior value of the next token, if you're paginating.</param>
        /// <returns></returns>
        public async Task<PaginatedResult<Project>> ListProjects(string type = "own", int limit = 50, string next = null)
        {
            using (var http = CreateClient())
            {
                var url = $"user/projects/{type}?limit={limit}";

                if (!String.IsNullOrWhiteSpace(next)) url = $"{url}&next={next}";

                var response = await http.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PaginatedResult<Project>>(content);
            }
        }

        public async Task<CreateDatasetResponse> CreateDataset(string owner, CreateDatasetRequest dataset)
        {
            using (var http = CreateClient())
            {
                var url = $"datasets/{owner}";

                var jsonBody = JsonConvert.SerializeObject(dataset, _serializerSettings);
                var postContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, postContent);
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<CreateDatasetResponse>(content);

                // we'll be nice and parse out the Id for you...
                result.Id = result.Uri?.AbsolutePath.Split('/').ToList().Last().Trim('/');

                return result;
            }
        }

        public async Task<MessageResponse> CreateOrReplaceDataset(string owner, string datasetId, CreateDatasetRequest dataset)
        {
            using (var http = CreateClient())
            {
                var url = $"datasets/{owner}/{datasetId}";

                var jsonBody = JsonConvert.SerializeObject(dataset, _serializerSettings);
                var postContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(url, postContent);
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<MessageResponse>(content);

                return result;
            }
        }

        public async Task<CreateProjectResponse> CreateProject(string owner, CreateProjectRequest project)
        {
            using (var http = CreateClient())
            {
                var url = $"projects/{owner}";

                var jsonBody = JsonConvert.SerializeObject(project, _serializerSettings);
                var postContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, postContent);
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<CreateProjectResponse>(content);

                // we'll be nice and parse out the Id for you...
                result.Id = result.Uri?.AbsolutePath.Split('/').ToList().Last().Trim('/');

                return result;
            }
        }

        /// <summary>
        /// Link an existing dataset to an existing project.
        /// </summary>
        /// <param name="projectOwner">The username of the project owner.</param>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="datasetOwner">The username of the dataset owner.</param>
        /// <param name="datasetId">The ID of the dataset.</param>
        /// <returns></returns>
        public async Task<MessageResponse> LinkDataset(string projectOwner, string projectId, string datasetOwner, string datasetId)
        {
            using (var http = CreateClient())
            {
                var url = $"projects/{projectOwner}/{projectId}/linkedDatasets/{datasetOwner}/{datasetId}";

                var response = await http.PutAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<MessageResponse>(content);
            }
        }

        public async Task<MessageResponse> UploadFile(string owner, string datasetId, string filename, Stream fileContents, bool expandArchives = false)
        {
            using (var http = CreateClient())
            {
                var url = $"uploads/{owner}/{datasetId}/files";

                if (expandArchives) url = $"{url}?expandArchives=true";

                var postContent = new MultipartFormDataContent();
                postContent.Add(new StreamContent(fileContents), "file", filename);

                var response = await http.PostAsync(url, postContent);
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<MessageResponse>(content);
            }
        }

        public async Task<MessageResponse> DeleteDataset(string owner, string datasetId)
        {
            using (var http = CreateClient(adminKey: true))
            {
                var url = $"datasets/{owner}/{datasetId}";

                var response = await http.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<MessageResponse>(content);
            }
        }

        private HttpClient CreateClient(bool adminKey = false)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://api.data.world/{ApiVersion}/")
            };

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", (adminKey) ? _adminApiKey : _readWriteApiKey);

            return client;
        }
    }
}
