﻿namespace Sdk4me.GraphQL
{
    /// <summary>
    /// Root connection for retrieving Workflow records.
    /// </summary>
    public class WorkflowQuery : Query<WorkflowQuery, WorkflowField, WorkflowView, WorkflowOrderField>
    {
        /// <summary>
        /// Initialize a new workflow query instance.
        /// </summary>
        public WorkflowQuery()
            : base("workflows", typeof(Workflow), true)
        {
        }

        /// <summary>
        /// Initialize a new workflow query instance.
        /// <br>Additional filters and views will be ignored.</br>
        /// </summary>
        /// <param name="id">The ID of the workflow.</param>
        public WorkflowQuery(string id)
            : base("Workflow", id, typeof(Workflow), false)
        {
        }

        /// <summary>
        /// The account this record belongs to.
        /// </summary>
        public WorkflowQuery SelectAccount(AccountQuery query)
        {
            query.FieldName = "account";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// Automation rules associated with this record.
        /// </summary>
        public WorkflowQuery SelectAutomationRules(AutomationRuleQuery query)
        {
            query.FieldName = "automationRules";
            return Select(query);
        }

        /// <summary>
        /// Values of custom fields.
        /// </summary>
        public WorkflowQuery SelectCustomFields(CustomFieldQuery query)
        {
            query.FieldName = "customFields";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// Inline images linked to one of the custom fields.
        /// </summary>
        public WorkflowQuery SelectCustomFieldsAttachments(AttachmentQuery query)
        {
            query.FieldName = "customFieldsAttachments";
            return Select(query);
        }

        /// <summary>
        /// Invoices associated with this object.
        /// </summary>
        public WorkflowQuery SelectInvoices(InvoiceQuery query)
        {
            query.FieldName = "invoices";
            return Select(query);
        }

        /// <summary>
        /// Who is responsible for coordinating the implementation of the workflow. If a manager is not specified for a new workflow, the API user is selected in the Manager field by default.
        /// </summary>
        public WorkflowQuery SelectManager(PersonQuery query)
        {
            query.FieldName = "manager";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// Notes of the record.
        /// </summary>
        public WorkflowQuery SelectNotes(NoteQuery query)
        {
            query.FieldName = "notes";
            return Select(query);
        }

        /// <summary>
        /// Phases of the workflow.
        /// </summary>
        public WorkflowQuery SelectPhases(WorkflowPhaseQuery query)
        {
            query.FieldName = "phases";
            return Select(query);
        }

        /// <summary>
        /// All problems of this workflow.
        /// </summary>
        public WorkflowQuery SelectProblems(ProblemQuery query)
        {
            query.FieldName = "problems";
            return Select(query);
        }

        /// <summary>
        /// Linked project
        /// </summary>
        public WorkflowQuery SelectProject(ProjectQuery query)
        {
            query.FieldName = "project";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// The release that the workflow is a part of.
        /// </summary>
        public WorkflowQuery SelectRelease(ReleaseQuery query)
        {
            query.FieldName = "release";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// All requests of this workflow.
        /// </summary>
        public WorkflowQuery SelectRequests(RequestQuery query)
        {
            query.FieldName = "requests";
            return Select(query);
        }

        /// <summary>
        /// The service that will directly be affected by the workflow implementation, or in case of an emergency change, the service that was directly affected by the workflow implementation.
        /// </summary>
        public WorkflowQuery SelectService(ServiceQuery query)
        {
            query.FieldName = "service";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// All tasks of this workflow.
        /// </summary>
        public WorkflowQuery SelectTasks(TaskQuery query)
        {
            query.FieldName = "tasks";
            return Select(query);
        }

        /// <summary>
        /// The workflow template that was used to register the workflow.
        /// </summary>
        public WorkflowQuery SelectTemplate(WorkflowTemplateQuery query)
        {
            query.FieldName = "template";
            query.IsConnection = false;
            return Select(query);
        }

        /// <summary>
        /// <br>Filter the 'Workflow' by their custom fields that have been marked as 'Filterable' in their UI Extension.</br>
        /// <br>Additional information is available on the <see href="https://developer.4me.com/graphql/input_object/workflowcustomfilter/">4me developer site</see>.</br>
        /// </summary>
        /// <param name="name">The name of the custom filter (i.e. value of the data-filterable-name attribute of the field in the UI Extension).</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="values">The filter values.
        /// <br>Use value <c>null</c> to indicate "None".</br>
        /// <br></br>
        /// <br>For "text" fields:</br>
        /// <br>• One can start the value with <c>~</c> to get partial matches (start with <c>'~</c> to match on a value starting with <c>~</c>).</br>
        /// <br></br>
        /// <br>For "number" and "time" fields:</br>
        /// <br>• One can start the value with <c>&lt;</c>, <c>&gt;</c>, <c>&lt;=</c>, <c>&gt;=</c> to get relative matches.</br>
        /// <br>• Times should be formatted as HH:mm:ss (e.g. <c>16:00:00</c>). The seconds part, <c>:ss</c>, is optional so <c>16:00</c> is also valid.</br>
        /// <br>• <c>&gt;{lower}&lt;{upper}</c> can be used to get matches where the value is more than '{lower}' and less than '{upper}'.</br>
        /// <br>• <c>&gt;={lower}&lt;={upper}</c> can be used to get matches where the value is between '{lower}' and '{upper}'.</br>
        /// <br>• <c>&gt;={lower}&lt;{upper}</c> can be used to get matches where the value is at least '{lower}' and less than '{upper}'.</br>
        /// <br></br>
        /// <br>For "date" and "date-time" fields:</br>
        /// <br>• One must supply a single value (and <c>null</c> is not supported).</br>
        /// <br>• Matches are always relative with an optional upper bound.</br>
        /// <br>• Dates should be formatted as <c>yyyy-MM-dd</c> (e.g. <c>2020-05-20</c>).</br>
        /// <br>• Date and time should be formatted as <c>yyyy-MM-ddTHH:mm:ssZ</c> (e.g. <c>2020-05-20T16:00:00Z</c>).</br>
        /// <br>• <c>&gt;={lower}</c> should be used to get matches where the value is at least '{lower}'.</br>
        /// <br>• <c>&gt;={lower}&lt;{upper}</c> can be used to get matches where the value is at least '{lower}' and before '{upper}'.</br>
        /// <br></br>
        /// <br>For "select" fields:</br>
        /// <br>• One must supply labels to match in the language of the account (i.e. not the value attributes of the HTML options).</br>
        /// <br>• Matches are case-sensitive.</br>
        /// </param>
        /// <exception cref="NullReferenceException"></exception>
        public WorkflowQuery CustomFilter(string name, FilterOperator filterOperator, params string?[] values)
        {
            return AddCustomFilter(name, filterOperator, values);
        }
    }
}
