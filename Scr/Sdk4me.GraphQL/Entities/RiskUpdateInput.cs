﻿namespace Sdk4me.GraphQL
{
    /// <summary>
    /// The <see href="https://developer.4me.com/graphql/input_object/riskupdateinput/">RiskUpdateInput</see> object.
    /// </summary>
    public class RiskUpdateInput : PropertyChangeSet
    {
        private string? source;
        private string? sourceID;
        private CustomFieldCollection? customFields;
        private List<AttachmentInput>? customFieldsAttachments;
        private string? uiExtensionId;
        private RiskClosureReason? closureReason;
        private string? managerId;
        private DateOnly? mitigationTargetAt;
        private string? note;
        private string? severity;
        private RiskStatus? status;
        private string? subject;
        private List<string>? organizationIds;
        private List<string>? projectIds;
        private List<string>? serviceIds;
        private string? id;
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
        /// Values for custom fields to be used by the UI Extension that is linked to the record.
        /// </summary>
        [JsonProperty("customFields")]
        public CustomFieldCollection? CustomFields
        {
            get => customFields;
            set => customFields = Set("customFields", value);
        }

        /// <summary>
        /// The attachments used in the custom fields' values.
        /// </summary>
        [JsonProperty("customFieldsAttachments")]
        public List<AttachmentInput>? CustomFieldsAttachments
        {
            get => customFieldsAttachments;
            set => customFieldsAttachments = Set("customFieldsAttachments", value);
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
        /// The appropriate closure reason for the risk when it has been closed.
        /// </summary>
        [JsonProperty("closureReason")]
        public RiskClosureReason? ClosureReason
        {
            get => closureReason;
            set => closureReason = Set("closureReason", value);
        }

        /// <summary>
        /// Identifier of the manager of the risk. This person is able to maintain the information about the risk.
        /// </summary>
        [JsonProperty("managerId")]
        public string? ManagerId
        {
            get => managerId;
            set => managerId = Set("managerId", value);
        }

        /// <summary>
        /// The date by which the risk should have been mitigated.
        /// </summary>
        [JsonProperty("mitigationTargetAt")]
        public DateOnly? MitigationTargetAt
        {
            get => mitigationTargetAt;
            set => mitigationTargetAt = Set("mitigationTargetAt", value);
        }

        /// <summary>
        /// The Note field is used to provide a detailed description of the risk and the actions that are planned or have already been taken to eliminate it, or to minimize its severity. This field is also used when the status is set to <c>closed</c> to explain why it was decided to close the risk.
        /// </summary>
        [JsonProperty("note")]
        public string? Note
        {
            get => note;
            set => note = Set("note", value);
        }

        /// <summary>
        /// The severity of the risk.
        /// </summary>
        [JsonProperty("severity")]
        public string? Severity
        {
            get => severity;
            set => severity = Set("severity", value);
        }

        /// <summary>
        /// The current status of the risk.
        /// </summary>
        [JsonProperty("status")]
        public RiskStatus? Status
        {
            get => status;
            set => status = Set("status", value);
        }

        /// <summary>
        /// The subject of the risk.
        /// </summary>
        [JsonProperty("subject")]
        public string? Subject
        {
            get => subject;
            set => subject = Set("subject", value);
        }

        /// <summary>
        /// Identifiers of organizations linked to the risk.
        /// </summary>
        [JsonProperty("organizationIds")]
        public List<string>? OrganizationIds
        {
            get => organizationIds;
            set => organizationIds = Set("organizationIds", value);
        }

        /// <summary>
        /// Identifiers of projects linked to the risk.
        /// </summary>
        [JsonProperty("projectIds")]
        public List<string>? ProjectIds
        {
            get => projectIds;
            set => projectIds = Set("projectIds", value);
        }

        /// <summary>
        /// Identifiers of services linked to the risk.
        /// </summary>
        [JsonProperty("serviceIds")]
        public List<string>? ServiceIds
        {
            get => serviceIds;
            set => serviceIds = Set("serviceIds", value);
        }

        /// <summary>
        /// The node ID of the record to update.
        /// </summary>
        [JsonProperty("id")]
        public string? ID
        {
            get => id;
            set => id = Set("id", value);
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
