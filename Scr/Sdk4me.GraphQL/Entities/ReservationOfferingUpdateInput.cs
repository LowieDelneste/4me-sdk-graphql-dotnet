﻿namespace Sdk4me.GraphQL
{
    /// <summary>
    /// The <see href="https://developer.4me.com/graphql/input_object/reservationofferingupdateinput/">ReservationOfferingUpdateInput</see> object.
    /// </summary>
    public class ReservationOfferingUpdateInput : PropertyChangeSet
    {
        private string? source;
        private string? sourceID;
        private string? name;
        private bool? disabled;
        private string? serviceInstanceId;
        private string? calendarId;
        private List<string>? filters;
        private string? timeZone;
        private ReservationStatus? initialStatus;
        private long? minDuration;
        private long? maxDuration;
        private long? stepDuration;
        private long? preparationDuration;
        private bool? multiDay;
        private bool? allowRepeat;
        private long? minAdvanceDuration;
        private long? maxAdvanceDuration;
        private List<string>? configurationItemIds;
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
        /// A short description of the reservation offering
        /// </summary>
        [JsonProperty("name")]
        public string? Name
        {
            get => name;
            set => name = Set("name", value);
        }

        /// <summary>
        /// Whether the reservation offering may not be used to register requests for reservation.
        /// </summary>
        [JsonProperty("disabled")]
        public bool? Disabled
        {
            get => disabled;
            set => disabled = Set("disabled", value);
        }

        /// <summary>
        /// Identifier of the service instance for which the reservations may be requested.
        /// </summary>
        [JsonProperty("serviceInstanceId")]
        public string? ServiceInstanceId
        {
            get => serviceInstanceId;
            set => serviceInstanceId = Set("serviceInstanceId", value);
        }

        /// <summary>
        /// Calendar that defines the hours in which reservations may start and end.
        /// </summary>
        [JsonProperty("calendarId")]
        public string? CalendarId
        {
            get => calendarId;
            set => calendarId = Set("calendarId", value);
        }

        /// <summary>
        /// The filters of the reservation offering.
        /// </summary>
        [JsonProperty("filters")]
        public List<string>? Filters
        {
            get => filters;
            set => filters = Set("filters", value);
        }

        /// <summary>
        /// <br>The time zone that applies to the selected calendar.</br>
        /// <br>The complete list is available on the <see href="https://developer.4me.com/graphql/scalar/timezone/">4me developer site</see>.</br>
        /// </summary>
        [JsonProperty("timeZone")]
        public string? TimeZone
        {
            get => timeZone;
            set => timeZone = Set("timeZone", value);
        }

        /// <summary>
        /// The initial status of the reservation.
        /// </summary>
        [JsonProperty("initialStatus")]
        public ReservationStatus? InitialStatus
        {
            get => initialStatus;
            set => initialStatus = Set("initialStatus", value);
        }

        /// <summary>
        /// The minimum duration of the reservation within the hours of the calendar.
        /// </summary>
        [JsonProperty("minDuration")]
        public long? MinDuration
        {
            get => minDuration;
            set => minDuration = Set("minDuration", value);
        }

        /// <summary>
        /// The maximum duration of the reservation within the hours of the calendar.
        /// </summary>
        [JsonProperty("maxDuration")]
        public long? MaxDuration
        {
            get => maxDuration;
            set => maxDuration = Set("maxDuration", value);
        }

        /// <summary>
        /// The increments with which the reservation may be prolonged.
        /// </summary>
        [JsonProperty("stepDuration")]
        public long? StepDuration
        {
            get => stepDuration;
            set => stepDuration = Set("stepDuration", value);
        }

        /// <summary>
        /// The duration required to prepare the asset before the reservation starts.
        /// </summary>
        [JsonProperty("preparationDuration")]
        public long? PreparationDuration
        {
            get => preparationDuration;
            set => preparationDuration = Set("preparationDuration", value);
        }

        /// <summary>
        /// Whether or not the reservation may span over multiple calendar days.
        /// </summary>
        [JsonProperty("multiDay")]
        public bool? MultiDay
        {
            get => multiDay;
            set => multiDay = Set("multiDay", value);
        }

        /// <summary>
        /// Whether it is allowed to create recurrent reservations for this offering.
        /// </summary>
        [JsonProperty("allowRepeat")]
        public bool? AllowRepeat
        {
            get => allowRepeat;
            set => allowRepeat = Set("allowRepeat", value);
        }

        /// <summary>
        /// The minimum duration between the creation time of a request for reservation and the requested start of the reservation.
        /// </summary>
        [JsonProperty("minAdvanceDuration")]
        public long? MinAdvanceDuration
        {
            get => minAdvanceDuration;
            set => minAdvanceDuration = Set("minAdvanceDuration", value);
        }

        /// <summary>
        /// The maximum duration between the creation time of a request for reservation and the requested start of the reservation.
        /// </summary>
        [JsonProperty("maxAdvanceDuration")]
        public long? MaxAdvanceDuration
        {
            get => maxAdvanceDuration;
            set => maxAdvanceDuration = Set("maxAdvanceDuration", value);
        }

        /// <summary>
        /// Identifiers of the configuration items that may be reserved using this offering.
        /// </summary>
        [JsonProperty("configurationItemIds")]
        public List<string>? ConfigurationItemIds
        {
            get => configurationItemIds;
            set => configurationItemIds = Set("configurationItemIds", value);
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
