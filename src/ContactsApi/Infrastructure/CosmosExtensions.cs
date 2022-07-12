using Microsoft.Azure.Cosmos;

namespace ContactsApi.Infrastructure;

public static class CosmosExtensions
{
    public static async Task<List<T>> Query<T>(this Container container, QueryDefinition queryDefinition, PartitionKey partitionKey = default)
    {
        var list = new List<T>();
        FeedIterator<T> feedIterator = container.GetItemQueryIterator<T>(
            queryDefinition,
            null,
            partitionKey == default ? null : new QueryRequestOptions() { PartitionKey = partitionKey }
            );

        while (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync().ConfigureAwait(false))
            {
                list.Add(item);
            }
        }

        return list;
    }

    public static async Task<List<T>> Query<T>(this Container container, QueryDefinition queryDefinition, string partitionKey)
        => await container.Query<T>(queryDefinition, new PartitionKey(partitionKey));

    public static async Task<T> QueryItem<T>(this Container container, QueryDefinition queryDefinition, string partitionKey = null)
    {
        var results = await container.Query<T>(queryDefinition, partitionKey).ConfigureAwait(false);
        return results.FirstOrDefault();
    }

    public static async Task<T> Get<T>(this Container container, string id, string partitionKeyValue = null)
    {
        try
        {
            var partitionKeyVal = partitionKeyValue ?? id;
            return await container.ReadItemAsync<T>(id, new PartitionKey(partitionKeyVal)).ConfigureAwait(false);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return default!;
        }
    }

    public static async Task<T> UpdateItem<T>(this Container container, string id, string paritionKey, T item)
    {
        var response = await container.ReplaceItemAsync(item, id, new PartitionKey(paritionKey)).ConfigureAwait(false);
        return response;
    }

    public static async Task<T> InsertItem<T>(this Container container, T item)
    {
        var response = await container.CreateItemAsync(item).ConfigureAwait(false);
        return response;
    }

    public static async Task<T> UpsertItem<T>(this Container container, T item)
    {
        return await container.UpsertItemAsync(item).ConfigureAwait(false);
    }

    public static async Task<T> DeleteItem<T>(this Container container, string id, string partitionKeyValue = null)
    {
        var partitionKeyVal = partitionKeyValue ?? id;
        return await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKeyVal)).ConfigureAwait(false);
    }

    public static async Task BulkCreateItemAsync<T>(this Container container, IEnumerable<T> items)
    {
        var createTasks = items.Select(i => container.CreateItemAsync<T>(i));

        var bulkTask = Task.WhenAll(createTasks);
        try
        {
            await bulkTask.ConfigureAwait(false);
        }
        catch
        {
            if (bulkTask?.Exception?.InnerExceptions != null && bulkTask.Exception.InnerExceptions.Any())
            {
                var exceptionMessages = string.Join(Environment.NewLine, bulkTask.Exception.InnerExceptions.Select(c => c.Message));
                throw new InvalidOperationException($"Bulk Import failed with exceptions : {exceptionMessages}");
            }
        }
    }

    public static async Task BulkUpsertItemAsync<T>(this Container container, IEnumerable<T> items)
    {
        var createTasks = items.Select(i => container.UpsertItemAsync<T>(i));

        var bulkTask = Task.WhenAll(createTasks);
        try
        {
            await bulkTask.ConfigureAwait(false);
        }
        catch
        {
            if (bulkTask?.Exception?.InnerExceptions != null && bulkTask.Exception.InnerExceptions.Any())
            {
                throw new InvalidOperationException($"Bulk Import failed with exceptions : {bulkTask.Exception.InnerExceptions.Select(c => c.Message + "\n")}");
            }
        }
    }
}
