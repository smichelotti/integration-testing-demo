using Microsoft.Azure.Cosmos;

namespace ContactsApi.Infrastructure;

public class CosmosContext
{
    private readonly CosmosClient cosmosClient;
    private readonly Database db;

    public CosmosContext(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
        this.db = this.cosmosClient.GetDatabase("contacts-management");
        this.Contacts = this.cosmosClient.GetContainer("contacts-management", "entities");
    }

    public Container Contacts { get; private set; }
}
