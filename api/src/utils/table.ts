import { output, input, TableInputOptions } from "@azure/functions";

const TableName = 'whattoeat'
const ConnectionName = 'AzureWebJobsStorage' //'AZURE_STORAGE_CONNECTION'
type PartitionType = 'restaurant' | 'user' | 'vote'
export type BaseEntity<T> = {
    PartitionKey: PartitionType,
    RowKey: string,
    Timestamp?: string,
    ClientData?: T,
}

export const TableOutput = output.table({
    connection: ConnectionName,
    tableName: TableName,
})

export const GetTableInput = (partitionKey: PartitionType, opts?: Partial<TableInputOptions>) => 
{
    const config = (opts ?? {}) as TableInputOptions
    config.connection = ConnectionName
    config.tableName = TableName
    config.partitionKey = partitionKey

    return input.table(config);
}
