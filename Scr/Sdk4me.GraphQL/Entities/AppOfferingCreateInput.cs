﻿namespace Sdk4me.GraphQL
{
    /// <summary>
    /// The <see href="https://developer.4me.com/graphql/input_object/appofferingcreateinput/">AppOfferingCreateInput</see> object.
    /// </summary>
    public class AppOfferingCreateInput : PropertyChangeSet
    {
        private string? source;
        private string? sourceID;
        private string? uiExtensionId;
        private string? compliance;
        private string? description;
        private List<AttachmentInput>? descriptionAttachments;
        private bool? disabled;
        private string? features;
        private string? name;
        private WebhookPolicyJwtAlg? policyJwtAlg;
        private string? policyJwtAudience;
        private long? policyJwtClaimExpiresIn;
        private string? reference;
        private string? serviceInstanceId;
        private string? uiExtensionVersionId;
        private string? configurationUriTemplate;
        private string? webhookUriTemplate;
        private List<AppOfferingScopeInput>? newScopes;
        private string? clientMutationId;

        /// <summary>
        /// An identifier for the client application submitting the resource or the name of an external system.
        /// </summary>
        [JsonProperty("source")]
        public string? Source
        {
            get => source;
            set => source = Set("source", value);
        }

        /// <summary>
        /// The unique identifier of the resource in an external system.
        /// </summary>
        [JsonProperty("sourceID")]
        public string? SourceID
        {
            get => sourceID;
            set => sourceID = Set("sourceID", value);
        }

        /// <summary>
        /// UI extension that is to be applied to the record.
        /// </summary>
        [JsonProperty("uiExtensionId")]
        public string? UiExtensionId
        {
            get => uiExtensionId;
            set => uiExtensionId = Set("uiExtensionId", value);
        }

        /// <summary>
        /// Compliance description of the app offering.
        /// </summary>
        [JsonProperty("compliance")]
        public string? Compliance
        {
            get => compliance;
            set => compliance = Set("compliance", value);
        }

        /// <summary>
        /// Description of the app offering.
        /// </summary>
        [JsonProperty("description")]
        public string? Description
        {
            get => description;
            set => description = Set("description", value);
        }

        /// <summary>
        /// The attachments used in the <c>description</c> field.
        /// </summary>
        [JsonProperty("descriptionAttachments")]
        public List<AttachmentInput>? DescriptionAttachments
        {
            get => descriptionAttachments;
            set => descriptionAttachments = Set("descriptionAttachments", value);
        }

        /// <summary>
        /// Whether the app offering may not be used for new instances.
        /// </summary>
        [JsonProperty("disabled")]
        public bool? Disabled
        {
            get => disabled;
            set => disabled = Set("disabled", value);
        }

        /// <summary>
        /// Feature description of the app offering.
        /// </summary>
        [JsonProperty("features")]
        public string? Features
        {
            get => features;
            set => features = Set("features", value);
        }

        /// <summary>
        /// Name of the app offering.
        /// </summary>
        [JsonProperty("name")]
        public string? Name
        {
            get => name;
            set => name = Set("name", value);
        }

        /// <summary>
        /// The algorithm used for generating the policy for the app offering's webhook.
        /// </summary>
        [JsonProperty("policyJwtAlg")]
        public WebhookPolicyJwtAlg? PolicyJwtAlg
        {
            get => policyJwtAlg;
            set => policyJwtAlg = Set("policyJwtAlg", value);
        }

        /// <summary>
        /// The audience for the policy for the app offering's webhook.
        /// </summary>
        [JsonProperty("policyJwtAudience")]
        public string? PolicyJwtAudience
        {
            get => policyJwtAudience;
            set => policyJwtAudience = Set("policyJwtAudience", value);
        }

        /// <summary>
        /// The claim expiry time for the policy for the app offering's webhook.
        /// </summary>
        [JsonProperty("policyJwtClaimExpiresIn")]
        public long? PolicyJwtClaimExpiresIn
        {
            get => policyJwtClaimExpiresIn;
            set => policyJwtClaimExpiresIn = Set("policyJwtClaimExpiresIn", value);
        }

        /// <summary>
        /// This reference can be used to link the app offering to an instance using the 4me APIs or the 4me Import functionality.
        /// </summary>
        [JsonProperty("reference")]
        public string? Reference
        {
            get => reference;
            set => reference = Set("reference", value);
        }

        /// <summary>
        /// Identifier of the the service instance this app offering is linked to.
        /// </summary>
        [JsonProperty("serviceInstanceId")]
        public string? ServiceInstanceId
        {
            get => serviceInstanceId;
            set => serviceInstanceId = Set("serviceInstanceId", value);
        }

        /// <summary>
        /// Identifier of the UI extension version that is linked to the app offering.
        /// </summary>
        [JsonProperty("uiExtensionVersionId")]
        public string? UiExtensionVersionId
        {
            get => uiExtensionVersionId;
            set => uiExtensionVersionId = Set("uiExtensionVersionId", value);
        }

        /// <summary>
        /// The URI where the app can be configured. The placeholder {account} can be used to include the customer account id in the URI.
        /// </summary>
        [JsonProperty("configurationUriTemplate")]
        public string? ConfigurationUriTemplate
        {
            get => configurationUriTemplate;
            set => configurationUriTemplate = Set("configurationUriTemplate", value);
        }

        /// <summary>
        /// The URI for the app offering's webhook. The placeholder {account} can be used to include the customer account id in the URI.
        /// </summary>
        [JsonProperty("webhookUriTemplate")]
        public string? WebhookUriTemplate
        {
            get => webhookUriTemplate;
            set => webhookUriTemplate = Set("webhookUriTemplate", value);
        }

        /// <summary>
        /// Scopes of this app offering
        /// </summary>
        [JsonProperty("newScopes")]
        public List<AppOfferingScopeInput>? NewScopes
        {
            get => newScopes;
            set => newScopes = Set("newScopes", value);
        }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        [JsonProperty("clientMutationId")]
        public string? ClientMutationId
        {
            get => clientMutationId;
            set => clientMutationId = Set("clientMutationId", value);
        }
    }
}
