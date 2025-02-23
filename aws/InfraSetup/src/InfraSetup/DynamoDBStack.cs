using System.Runtime.CompilerServices;
using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;

namespace InfraSetup
{
    public static class DynamoDBStack
    {
        public static void Setup(Stack stack, EnvironmentDetails envDetails)
        {
            if (envDetails.Type == EnvironmentType.DevOps)
                return;
            var tableItems = new[] {(tableName: "Account", partitionKeyName:"AccountID", sortKeyName:"ItemID")};
            foreach (var tableItem in tableItems)
            {
                var tableName = tableItem.tableName;
                var partitionKeyName = tableItem.partitionKeyName;
                var sortKeyName = tableItem.sortKeyName;

                var idName = $"{envDetails.AppPrefix}-{tableName}-{envDetails.EnvSuffix}";
                var table = new Table(stack, idName, new TableProps()
                {
                    TableName = idName,
                    PartitionKey = new Attribute
                    {
                        Type = AttributeType.STRING,
                        Name = partitionKeyName
                    },
                    SortKey = new Attribute
                    {
                        Type = AttributeType.STRING,
                        Name = sortKeyName,
                    },
                    BillingMode = BillingMode.PROVISIONED,
                    RemovalPolicy = RemovalPolicy.RETAIN,
                    PointInTimeRecovery = true,
                    ContributorInsightsEnabled = true
                });
            }
        }
    }
}