﻿namespace Sdk4me.GraphQL
{
    /// <summary>
    /// Creates a new configuration item. **Important**: To facilitate integrations with discovery tools, the create is treated as an update in case the provided <c>name</c> or <c>label</c> is already used by an inactive CI in the same account.
    /// </summary>
    internal class ConfigurationItemCreateMutation : Mutation<ConfigurationItemCreatePayload, ConfigurationItemCreateInput>
    {
        /// <summary>
        /// Initialize an new ConfigurationItemCreate mutation instance.
        /// </summary>
        internal ConfigurationItemCreateMutation(ConfigurationItemCreateInput data)
            : base("configurationItemCreate", "ConfigurationItemCreateInput!", data, new HashSet<IQuery>() { new ConfigurationItemQuery() { FieldName = "configurationItem", IsConnection = false }.Select("*") })
        {
        }
    }
}
