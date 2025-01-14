﻿using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Sdk4me.GraphQL
{
    /// <summary>
    /// The 4me GraphQL client.
    /// </summary>
    public sealed class Sdk4meClient : IDisposable
    {
        private readonly AuthenticationTokenCollection authenticationTokens;
        private readonly JsonSerializer jsonSerializer;
        private readonly InterfaceConverter interfaceConverter;
        private readonly string url;
        private readonly string restUrl;
        private readonly string oauth2Url;
        private readonly HttpClient client;
        private readonly bool traceEnabled = Trace.Listeners != null && Trace.Listeners.Count > 0;
        private readonly string applicationJsonMediaType = "application/json";
        private AuthenticationToken currentToken;
        private string accountID;
        private int maximumRecursiveRequests = 10;
        private int maximumQueryDepthLevelConnections = 2;
        private int itemsPerRequest = 100;
        private bool disposedValue;

        /// <summary>
        /// <br>Get or set the number of recursive requests.</br>
        /// <br>The value must be at least 1 and maximum 1000.</br>
        /// </summary>
        public int MaximumRecursiveRequests
        {
            get => maximumRecursiveRequests;
            set => maximumRecursiveRequests = (value < 1 || value > 1000) ? 10 : value;
        }

        /// <summary>
        /// <br>Get or set the maximum number of GraphQL depth level connections.</br>
        /// <br>The value must be at least 1 and maximum 13.</br>
        /// <para>Warning: changing this to a higher value can impact performance significantly because of the built-in pagination handling. The default value is 2.</para>
        /// </summary>
        public int MaximumQueryDepthLevelConnections
        {
            get => maximumQueryDepthLevelConnections;
            set => maximumQueryDepthLevelConnections = (value < 1 || value > 13) ? 2 : value;
        }

        /// <summary>
        /// <br>Get or set the number of objects returned per API call.</br>
        /// <br>The value needs to be between 1 and 100 inclusive.</br>
        /// </summary>
        public int DefaultItemsPerRequest
        {
            get => itemsPerRequest;
            set
            {
                itemsPerRequest = itemsPerRequest switch
                {
                    <= 0 => 1,
                    > 100 => 100,
                    _ => value,
                };
            }
        }

        /// <summary>
        /// Get or set the 4me account ID.
        /// </summary>
        public string AccountID
        {
            get => accountID;
            set => accountID = value;
        }

        /// <summary>
        /// <para>
        /// <br>Specify the enumerator serialization behavior.</br>
        /// </para>
        /// <br>True to ignore unmappable enumerator values; otherwise false.</br>
        /// <br>If the SDK cannot recognize a specific enumerator value, it will either return null or a default value instead.</br>
        /// </summary>
        public bool EnumeratorTolerantSerializer
        {
            get => jsonSerializer.Converters.Any(x => x.GetType() == typeof(JsonEnumConverter));
            set
            {
                if (value)
                {
                    if (!jsonSerializer.Converters.Any(x => x.GetType() == typeof(JsonEnumConverter)))
                        jsonSerializer.Converters.Add(new JsonEnumConverter());
                }
                else
                {
                    if (jsonSerializer.Converters.FirstOrDefault(x => x.GetType() == typeof(JsonEnumConverter)) is JsonEnumConverter converter)
                        jsonSerializer.Converters.Remove(converter);
                }
            }
        }

        /// <summary>
        /// Create a new instance of the <see cref="Sdk4meClient"/>.
        /// </summary>
        /// <param name="authenticationToken">The authentication token.</param>
        /// <param name="accountID">The 4me Account ID.</param>
        /// <param name="environment">The 4me environment.</param>
        /// <param name="environmentRegion">The 4me environment region.</param>
        /// <param name="maximumRecursiveRequests">The number of recursive requests.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sdk4meClient(AuthenticationToken authenticationToken, string accountID, EnvironmentType environment, EnvironmentRegion environmentRegion, int maximumRecursiveRequests = 10)
            : this(new AuthenticationTokenCollection(authenticationToken), accountID, EndpointUrlBuilder.GetDomainName(environmentRegion, environment), maximumRecursiveRequests)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="Sdk4meClient"/>.
        /// </summary>
        /// <param name="authenticationToken">The authentication token.</param>
        /// <param name="accountID">The 4me Account ID.</param>
        /// <param name="domainName">The 4me domain name.</param>
        /// <param name="maximumRecursiveRequests">The number of recursive requests.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sdk4meClient(AuthenticationToken authenticationToken, string accountID, string domainName, int maximumRecursiveRequests = 10)
            : this(new AuthenticationTokenCollection(authenticationToken), accountID, domainName, maximumRecursiveRequests)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="Sdk4meClient"/>.
        /// </summary>
        /// <param name="authenticationTokens">The authentication token collection.</param>
        /// <param name="accountID">The 4me Account ID.</param>
        /// <param name="environment">The 4me environment.</param>
        /// <param name="environmentRegion">The 4me environment region.</param>
        /// <param name="maximumRecursiveRequests">The number of recursive requests.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sdk4meClient(AuthenticationTokenCollection authenticationTokens, string accountID, EnvironmentType environment, EnvironmentRegion environmentRegion, int maximumRecursiveRequests = 10)
            : this(authenticationTokens, accountID, EndpointUrlBuilder.GetDomainName(environmentRegion, environment), maximumRecursiveRequests)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="Sdk4meClient"/>.
        /// </summary>
        /// <param name="authenticationTokens">The authentication token collection.</param>
        /// <param name="accountID">The 4me Account ID.</param>
        /// <param name="domainName">The 4me domain name.</param>
        /// <param name="maximumRecursiveRequests">The number of recursive requests.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sdk4meClient(AuthenticationTokenCollection authenticationTokens, string accountID, string domainName, int maximumRecursiveRequests = 10)
        {
            if (string.IsNullOrWhiteSpace(accountID))
                throw new ArgumentException($"'{nameof(accountID)}' cannot be null or empty.", nameof(accountID));

            if (authenticationTokens is null)
                ArgumentNullException.ThrowIfNull(authenticationTokens);

            if (!authenticationTokens.Any())
                throw new ArgumentException($"'{nameof(authenticationTokens)}' cannot be empty.", nameof(authenticationTokens));

            url = EndpointUrlBuilder.Get(domainName);
            restUrl = EndpointUrlBuilder.GetRest(domainName);
            oauth2Url = EndpointUrlBuilder.GetOAuth2(domainName);
            this.authenticationTokens = authenticationTokens;
            this.accountID = accountID;
            this.maximumRecursiveRequests = (maximumRecursiveRequests < 1 || maximumRecursiveRequests > 1000) ? 10 : maximumRecursiveRequests;

            jsonSerializer = new();
            jsonSerializer.Converters.Add(new ISO8601TimeJsonConverter());
            jsonSerializer.Converters.Add(new ISO8601TimestampJsonConverter());
            jsonSerializer.Converters.Add(new JsonEnumConverter());

            interfaceConverter = new();
            jsonSerializer.Converters.Add(interfaceConverter);

            client = new();
            client.SetUserAgent("Sdk4me.GraphQL");
            currentToken = authenticationTokens.Get();
        }

        /// <summary>
        /// Query the 4me web service as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TEntity">Any type implementing <see cref="Node"/>.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Sdk4meException"></exception>
        public async Task<DataList<TEntity>> Get<TEntity>(IQuery query) where TEntity : Node
        {
            return await Get<TEntity>(ExecutionQueryBuilder.BuildQuery(query, itemsPerRequest), maximumRecursiveRequests);
        }

        /// <summary>
        /// Query the 4me web service as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TEntity">Any type implementing <see cref="Node"/>.</typeparam>
        /// <param name="executionQuery">The execution query to execute.</param>
        /// <param name="recursiveRequest">The number of recursive requests.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Sdk4meException"></exception>
        private async Task<DataList<TEntity>> Get<TEntity>(ExecutionQuery executionQuery, int recursiveRequest) where TEntity : Node
        {
            if (recursiveRequest <= 0)
                return new();

            int highestDepthValue = executionQuery.GetHighestDepthValue() + 1;
            if (highestDepthValue > maximumQueryDepthLevelConnections)
                throw new Sdk4meException($"The query exceeds the maximum allowed depth level connections value. The maximum value is set to {maximumQueryDepthLevelConnections}, and the query contains {highestDepthValue} depth level connections.");

            DataList<TEntity> retval = new();
            using (HttpRequestMessage requestMessage = CreateHttpRequest())
            {
                string query = $"{{\"query\":{JsonConvert.SerializeObject($"query{{{ExecutionQueryBuilder.GetGraphQLQuery(executionQuery)}}}")}}}";
                JToken responseData = await SendHttpRequest(requestMessage, query, true);

                NodeCollection<TEntity> nodes = new();
                if (responseData[executionQuery.GetResponseObjectName()] is JToken responseObject)
                {
                    interfaceConverter.InterfaceTypeMappings = executionQuery.GetOnTypeQueries();

                    if (responseObject["nodes"] != null && responseObject.ToObject<NodeCollection<TEntity>>(jsonSerializer) is NodeCollection<TEntity> collection)
                        nodes = collection;
                    else if (responseObject.ToObject<TEntity>(jsonSerializer) is TEntity data)
                        nodes.Data = new() { data };

                    retval.AddRange(nodes.Data);

                    HashSet<QueryPageInfo> queryPagesInfo = nodes.GetQueryPageInfo(executionQuery.FieldName, 0);
                    switch (executionQuery.UpdateCursors(queryPagesInfo))
                    {
                        case 0:
                            retval.AddRange(await Get<TEntity>(executionQuery, recursiveRequest - 1));
                            break;

                        case > 0:
                            retval.AddRange(await Get<TEntity>(executionQuery, recursiveRequest));
                            break;
                    }
                }
            }
            return retval;
        }

        /// <summary>
        /// Execute a mutation on 4me web service as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TOutEntity">Any type implementing <see cref="Payload"/>.</typeparam>
        /// <typeparam name="TInEntity">Any type implementing <see cref="PropertyChangeSet"/>.</typeparam>
        /// <param name="input">The mutation input data.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Sdk4meException"></exception>
        internal async Task<TOutEntity> Mutation<TOutEntity, TInEntity>(Mutation<TOutEntity, TInEntity> input)
            where TOutEntity : Payload
            where TInEntity : PropertyChangeSet
        {
            return await Mutation(input, true);
        }

        /// <summary>
        /// Execute a mutation on 4me web service as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TOutEntity">Any type implementing <see cref="Payload"/>.</typeparam>
        /// <typeparam name="TInEntity">Any type implementing <see cref="PropertyChangeSet"/>.</typeparam>
        /// <param name="input">The mutation input data.</param>
        /// <param name="throwOnError">Throw an <see cref="Sdk4meException"/> when the mutation fails.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Sdk4meException"></exception>
        internal async Task<TOutEntity> Mutation<TOutEntity, TInEntity>(Mutation<TOutEntity, TInEntity> input, bool throwOnError)
            where TOutEntity : Payload
            where TInEntity : PropertyChangeSet
        {
            using (HttpRequestMessage requestMessage = CreateHttpRequest())
            {
                JsonSerializerSettings settings = new()
                {
                    ContractResolver = new MutationQueryContractResolver(input.Data.GetFields(), typeof(TInEntity))
                };
                settings.Converters.Add(new StringEnumConverter());
                settings.Converters.Add(new ISO8601TimeJsonConverter());
                settings.Converters.Add(new ISO8601TimestampJsonConverter());

                string query = $"{{\"query\":{JsonConvert.SerializeObject(ExecutionQueryBuilder.GetGraphQLQuery(input))},\"variables\":{{\"input\":{JsonConvert.SerializeObject(input.Data, settings)}}}}}";
                JToken responseData = await SendHttpRequest(requestMessage, query, true);
                TOutEntity retval = responseData?[input.FieldName]?.ToObject<TOutEntity>() ?? throw new Sdk4meException("Unprocessable response entity.");
                if (throwOnError && retval is Payload payload && payload.Errors?.First() is ValidationError error)
                    throw new Sdk4meException(error.Message);
                return retval;
            }
        }

        /// <summary>
        /// Upload a file to the 4me AWS S3 storage. 
        /// </summary>
        /// <param name="path">The file to upload.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>A <see cref="AttachmentUploadResponse"/> containing the 4me AWS S3 file storage reference key and file size.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Sdk4meException"></exception>
        public async Task<AttachmentUploadResponse> UploadAttachment(string path, string contentType)
        {
            return await UploadAttachment(new FileInfo(path), contentType);
        }

        /// <summary>
        /// Upload a file to the 4me AWS S3 storage. 
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>A <see cref="AttachmentUploadResponse"/> containing the 4me AWS S3 file storage reference key and file size.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Sdk4meException"></exception>
        public async Task<AttachmentUploadResponse> UploadAttachment(FileInfo file, string contentType)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            using (FileStream stream = file.OpenRead())
                return await UploadAttachment(stream as Stream, file.Name, contentType);
        }

        /// <summary>
        /// Upload a file to the 4me AWS S3 storage. 
        /// </summary>
        /// <param name="stream">The content to upload. The stream needs to support seeking in order to determine the HTTP Content-Length header.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>A <see cref="AttachmentUploadResponse"/> containing the 4me AWS S3 file storage reference key and file size.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Sdk4meException"></exception>
        public async Task<AttachmentUploadResponse> UploadAttachment(StreamReader stream, string fileName, string contentType)
        {
            return await UploadAttachment(stream.BaseStream, fileName, contentType);
        }

        /// <summary>
        /// Upload a file to the 4me AWS S3 storage. 
        /// </summary>
        /// <param name="stream">The content to upload. The stream needs to support seeking in order to determine the HTTP Content-Length header.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>A <see cref="AttachmentUploadResponse"/> containing the 4me AWS S3 file storage reference key and file size.</returns>
        /// <exception cref="Sdk4meException"></exception>
        public async Task<AttachmentUploadResponse> UploadAttachment(Stream stream, string fileName, string contentType)
        {
            if (!stream.CanSeek)
                throw new Sdk4meException("The stream needs to support seeking in order to determine the HTTP Content-Length header.");

            DataList<AttachmentStorage> attachmentStorages = await Get<AttachmentStorage>(Query.AttachmentStorage);
            if (attachmentStorages.FirstOrDefault() is AttachmentStorage attachmentStorage)
            {
                string fileExtension = Path.GetExtension(fileName).TrimStart('.');
                if (attachmentStorage.AllowedExtensions != null && (attachmentStorage.AllowedExtensions.Count == 0 || attachmentStorage.AllowedExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase)))
                {
                    if (stream.Length >= attachmentStorage.SizeLimit)
                        throw new Sdk4meException($"File size exceeded, the maximum size is {attachmentStorage.SizeLimit} byte.");

                    Dictionary<string, string> storageFacility = attachmentStorage?.ProviderParameters?.ToObject<Dictionary<string, string>>() ?? throw new Sdk4meException("File upload failed, invalid AttachmentStorage.ProviderParameters value.");
                    MultipartFormDataContent multipartContent = new()
                    {
                        { new StringContent(contentType), "Content-Type" },
                        { new StringContent(storageFacility["acl"]), "acl" },
                        { new StringContent(storageFacility["key"]), "key" },
                        { new StringContent(storageFacility["policy"]), "policy" },
                        { new StringContent(storageFacility["success_action_status"]), "success_action_status" },
                        { new StringContent(storageFacility["x-amz-algorithm"]), "x-amz-algorithm" },
                        { new StringContent(storageFacility["x-amz-credential"]), "x-amz-credential" },
                        { new StringContent(storageFacility["x-amz-date"]), "x-amz-date" },
                        { new StringContent(storageFacility["x-amz-server-side-encryption"]), "x-amz-server-side-encryption" },
                        { new StringContent(storageFacility["x-amz-signature"]), "x-amz-signature" },
                        { new StreamContent(stream), "file", fileName }
                    };

                    using (HttpRequestMessage requestMessage = new(HttpMethod.Post, attachmentStorage.UploadUri) { Content = multipartContent })
                    {
                        Guid logId = Guid.NewGuid();
                        WriteDebug(logId, requestMessage, "***");
                        Sleep.RegisterStartTime();

                        using (HttpResponseMessage responseMessage = await client.SendAsync(requestMessage))
                        {
                            WriteDebug(logId, requestMessage, "***", true);
                            Sleep.SleepRemainingTime(0);

                            using (StreamReader reader = new(responseMessage.Content.ReadAsStream()))
                            {
                                string data = reader.ReadToEnd();

                                Match match = Regex.Match(data, "(?<=<Key>)(.*?)(?=</Key>)");
                                if (match.Success)
                                {
                                    return new()
                                    {
                                        Key = match.Value,
                                        Size = stream.Length
                                    };
                                }
                                else
                                {
                                    throw new Sdk4meException(data);
                                }
                            }
                        }
                    }
                }
                if (fileExtension == string.Empty)
                    throw new Sdk4meException($"A file name without extension is not allowed on this 4me instance.");
                else
                    throw new Sdk4meException($"The '{fileExtension}' extension is not allowed on this 4me instance.");
            }
            throw new Sdk4meException("No AttachmentStorage object returned by the GraphQL API.");
        }

        /// <summary>
        /// <para>Create a new event using the <see href="https://developer.4me.com/v1/requests/events/">events API</see>.</para>
        /// <b>Important</b>: there might be missing values in the response as the REST API <c>request</c> response is coverted to a <see cref="Request">request</see> object.
        /// </summary>
        /// <param name="requestEventCreateInput">The event's data.</param>
        /// <returns>The newly created request or already existing request.</returns>
        public async Task<Request> CreateEvent(RequestEventCreateInput requestEventCreateInput)
        {
            using (HttpRequestMessage requestMessage = CreateHttpRequest($"{restUrl}/events"))
            {
                JToken responseData = await SendHttpRequest(requestMessage, requestEventCreateInput.HttpRequestBody, false);
                int? requestID = responseData.Value<int>("id");
                if (requestID != null)
                {
                    responseData = responseData.ToCamelCase();
                    responseData["requestId"] = requestID;
                    responseData["tags"] = null;
                    return responseData.ToObject<Request>() ?? throw new Sdk4meException("Unprocessable response entity.");
                }
                else
                {
                    throw new Sdk4meException("The response does not contain an id object.");
                }
            }
        }

        /// <summary>
        /// Configures the weight for prioritizing authentication tokens based on remaining API requests and cost.
        /// </summary>
        /// <param name="requestWeight">The weight for prioritizing remaining API requests. Default is 0.6.</param>
        /// <param name="costWeight">The weight for prioritizing remaining cost. Default is 0.4.</param>
        /// <remarks>
        /// Use this method to dynamically adjust the weighting of authentication tokens to influence their sorting and prioritization.
        /// By specifying new weight values, you can fine-tune the sorting behavior of tokens based on their remaining API requests and cost consumption.
        /// This flexibility enables you to optimize token selection to meet specific requirements or adhere to service quotas.
        /// <para>To learn more about GraphQL Service Quotas, refer to the <see href="https://developer.4me.com/graphql/#service-quotas-1">Service Quota</see> section, and for information on Rate Limiting, explore the <see href="https://developer.4me.com/v1/#rate-limiting">Rate Limiting</see> section in the 4me developer documentation.</para>
        /// </remarks>
        public void ConfigureAuthenticationTokenWeight(double requestWeight = 0.6, double costWeight = 0.4)
        {
            authenticationTokens.RequestWeight = requestWeight;
            authenticationTokens.CostWeight = costWeight;
        }

        /// <summary>
        /// Create a new <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <returns>The <see cref="HttpRequestMessage"/> object.</returns>
        private HttpRequestMessage CreateHttpRequest(string? url = null)
        {
            SetAuthenticationToken();
            HttpRequestMessage retval = new(HttpMethod.Post, url ?? this.url);
            retval.Headers.Authorization = new AuthenticationHeaderValue(currentToken.TokenType, currentToken.Token);
            retval.Headers.Add("x-4me-Account", accountID);
            return retval;
        }

        /// <summary>
        /// Sets the authentication method, which can be a Personal Access Token or a OAuth 2.0 Client Credential Grant.
        /// </summary>
        private void SetAuthenticationToken()
        {
            currentToken = authenticationTokens.Get();
            if (currentToken.IsTokenExpired())
            {
                using (HttpRequestMessage requestMessage = new(HttpMethod.Post, oauth2Url))
                {
                    requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "grant_type", "client_credentials" }, { "client_id", currentToken.ClientID }, { "client_secret", currentToken.ClientSecret } });

                    Guid logId = Guid.NewGuid();
                    WriteDebug(logId, requestMessage, "***");
                    Sleep.RegisterStartTime();

                    using (HttpResponseMessage responseMessage = client.Send(requestMessage))
                    {
                        WriteDebug(logId, requestMessage, "***", true);
                        Sleep.SleepRemainingTime(0);

                        responseMessage.EnsureSuccessStatusCode();
                        string content = responseMessage.Content.ReadAsStringAsync().Result;
                        AuthenticationOAuth2Reponse response = JsonConvert.DeserializeObject<AuthenticationOAuth2Reponse>(content) ?? throw new Sdk4meException("Invalid authentication response.");
                        currentToken.Token = response.AccessToken;
                        currentToken.TokenType = response.TokenType;
                        currentToken.AuthenticationTokenExpires = DateTime.Now.AddSeconds(response.ExpiresIn);
                    }
                }
            }
        }

        /// <summary>
        /// Send a <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="requestMessage">The http request message.</param>
        /// <param name="content">The data to send as http request content.</param>
        /// <param name="isGraphQLResponse">The response is a GraphQL query response.</param>
        /// <returns>The <see cref="JToken"/> response object.</returns>
        /// <exception cref="Sdk4meException"></exception>
        private async Task<JToken> SendHttpRequest(HttpRequestMessage requestMessage, string? content, bool isGraphQLResponse)
        {
            if (!string.IsNullOrEmpty(content))
                requestMessage.Content = new StringContent(content, Encoding.UTF8, applicationJsonMediaType);

            Guid logId = Guid.NewGuid();
            WriteDebug(logId, requestMessage, content);
            Sleep.RegisterStartTime();
            using (HttpResponseMessage responseMessage = await client.SendAsync(requestMessage))
            {
                WriteDebug(logId, requestMessage, content, true);
                Sleep.SleepRemainingTime();

                if (responseMessage.IsSuccessStatusCode && responseMessage.Content.Headers.ContentType?.MediaType == applicationJsonMediaType)
                {
                    JObject data = JObject.Parse(await responseMessage.Content.ReadAsStringAsync());
                    UpdateAccountRateLimits(responseMessage);

                    if (data.ContainsKey("errors"))
                    {
                        throw new Sdk4meException(data["errors"]);
                    }
                    else if (data.ContainsKey("data"))
                    {
                        if (data["data"] is JToken retval)
                        {
                            return retval;
                        }
                        throw new Sdk4meException("The response does not contain a data object.");
                    }
                    else if (!isGraphQLResponse)
                    {
                        return data;
                    }
                    else
                    {
                        throw new Sdk4meException($"The response could not be processed.\r\n{data.ToString(Formatting.Indented)}");
                    }
                }
                else
                {
                    if (responseMessage.Content.Headers.ContentType?.MediaType == applicationJsonMediaType)
                    {
                        using (StreamReader streamReader = new(responseMessage.Content.ReadAsStream()))
                            throw new Sdk4meException(streamReader.ReadToEnd());
                    }
                    else
                    {
                        throw new Sdk4meException(responseMessage.StatusCode.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Updates the current token rate limit.
        /// </summary>
        /// <param name="responseMessage">The <see cref="HttpRequestMessage"/> reference.</param>
        private void UpdateAccountRateLimits(HttpResponseMessage responseMessage)
        {
            if (responseMessage != null && currentToken != null)
            {
                if (responseMessage.Headers.TryGetValues("X-RateLimit-Limit", out IEnumerable<string>? values) && values != null && int.TryParse(values.FirstOrDefault(), out int result))
                    currentToken.RequestLimit = Convert.ToInt32(result);
                if (responseMessage.Headers.TryGetValues("X-RateLimit-Remaining", out values) && values != null && int.TryParse(values.FirstOrDefault(), out result))
                    currentToken.RequestsRemaining = Convert.ToInt32(result);
                if (responseMessage.Headers.TryGetValues("X-RateLimit-Reset", out values) && values != null && long.TryParse(values.FirstOrDefault(), out long dateResult))
                    currentToken.RequestLimitReset = DateTime.UnixEpoch.AddSeconds(dateResult).ToLocalTime();

                if (responseMessage.Headers.TryGetValues("X-CostLimit-Limit", out values) && values != null && int.TryParse(values.FirstOrDefault(), out result))
                    currentToken.CostLimit = result;
                if (responseMessage.Headers.TryGetValues("X-CostLimit-Remaining", out values) && values != null && int.TryParse(values.FirstOrDefault(), out result))
                    currentToken.CostLimitRemaining = result;
                if (responseMessage.Headers.TryGetValues("X-CostLimit-Reset", out values) && values != null && long.TryParse(values.FirstOrDefault(), out dateResult))
                    currentToken.CostLimitReset = DateTime.UnixEpoch.AddSeconds(dateResult).ToLocalTime();
            }
        }

        #region debug writers

        /// <summary>
        /// Writes a message to the trace listeners in the System.Diagnostics.Trace.Listeners collection.
        /// </summary>
        /// <param name="logIdentifier">The unique log group identifier.</param>
        /// <param name="requestMessage">The HTTP request message.</param>
        /// <param name="content">The HTTP request message content.</param>
        /// <param name="isResponse">Is the HTTP request been executed.</param>
        private void WriteDebug(Guid logIdentifier, HttpRequestMessage requestMessage, string? content = null, bool isResponse = false)
        {
            try
            {
                if (traceEnabled)
                {
                    Trace.WriteLine(new TraceMessage()
                    {
                        ID = logIdentifier,
                        AccountID = isResponse ? null : accountID,
                        Method = isResponse ? null : requestMessage.Method.ToString(),
                        URI = isResponse ? null : requestMessage.RequestUri?.AbsoluteUri,
                        Content = isResponse ? null : content,
                        ResponseTimeInMilliseconds = isResponse ? Sleep.ResponseTimeInMilliseconds : null
                    });
                    Trace.Flush();
                }
            }
            catch
            {
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client?.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Release the unmanaged resources and disposes the <see cref="HttpClient"/> used by the <see cref="Sdk4meClient"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
