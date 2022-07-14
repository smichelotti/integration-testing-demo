using ContactsApi.Controllers;

namespace ContactsApi.IntegrationTests.Controllers;

[TestCaseOrderer(AlphabeticalOrderer.Name, AlphabeticalOrderer.Assembly)]
public class ContactsCrudOrdered : IntegrationContext
{
    private static string contactId;

    public ContactsCrudOrdered(WebApiFixture fixture) : base(fixture) { }

    [Fact]
    public async Task T01_should_generate_new_contact()
    {
        // arrange
        var contact = new ContactPostCommand
        {
            FirstName = "John",
            LastName = "Smith",
            Address = new("501 E Pratt St", "Baltimore", "MD", "21202")
        };

        // act
        var result = await this.Host.Scenario(_ =>
        {
            _.Post.Json(contact).ToUrl("/api/contacts");
            _.StatusCodeShouldBe(HttpStatusCode.Created);
        });

        // assert
        var resp = result.ReadAsJson<ContactPostResult>();
        resp.Id.Should().NotBeNullOrEmpty();
        contactId = resp.Id;
        resp.FirstName.Should().Be("John");
        resp.LastName.Should().Be("Smith");
        resp.Address.Street.Should().Be("501 E Pratt St");
        resp.Address.Latitude.Should().NotBe(0);
        resp.Address.Longitude.Should().NotBe(0);
    }

    [Fact]
    public async Task T02_should_retrieve_contact()
    {
        var result = await this.Host.Scenario(_ =>
        {
            _.Get.Url($"/api/contacts/{contactId}");
            _.StatusCodeShouldBeOk();
        });

        // assert
        var resp = result.ReadAsJson<ContactGetResult>();
        resp.Id.Should().NotBeNullOrEmpty();
        contactId = resp.Id;
        resp.FirstName.Should().Be("John");
        resp.LastName.Should().Be("Smith");
        resp.Address.Street.Should().Be("501 E Pratt St");
        resp.Address.Latitude.Should().NotBe(0);
        resp.Address.Longitude.Should().NotBe(0);
    }

    [Fact]
    public async Task T03_should_update_existing_contact()
    {
        // arrange/act
        var existing = await this.Host.GetAsJson<ContactGetResult>($"/api/contacts/{contactId}");
        var updated = new
        {
            FirstName = "Johnx",
            LastName = "Smithx",
            Address = new { Street = "123 Main Street", existing.Address.City, existing.Address.State, existing.Address.PostalCode }
        };

        var result = await this.Host.Scenario(_ =>
        {
            _.Put.Json(updated).ToUrl($"/api/contacts/{contactId}");
            _.StatusCodeShouldBeOk();
        });

        // assert (response)
        var resp = result.ReadAsJson<ContactPutResult>();
        resp.Id.Should().Be(contactId);
        resp.FirstName.Should().Be("Johnx");
        resp.LastName.Should().Be("Smithx");
        resp.Address.Street.Should().Be("123 Main Street");
        resp.Address.Latitude.Should().NotBe(0);
        resp.Address.Longitude.Should().NotBe(0);

        // Let's make absolutely sure a GET request has updated data and the PUT didn't just echo it back on the response
        var getResult = await this.Host.Scenario(_ =>
        {
            _.Get.Url($"/api/contacts/{contactId}");
            _.StatusCodeShouldBeOk();
        });

        // assert (fresh GET)
        var getResp = getResult.ReadAsJson<ContactPutResult>();
        getResp.Id.Should().Be(contactId);
        getResp.FirstName.Should().Be("Johnx");
        getResp.LastName.Should().Be("Smithx");
        getResp.Address.Street.Should().Be("123 Main Street");
    }

    [Fact]
    public async Task T04_should_delete_existing_contact()
    {
        // act/assert
        await this.Host.Scenario(_ =>
        {
            _.Delete.Url($"/api/contacts/{contactId}");
            _.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });

        // Make sure GET is now 404
        await this.Host.Scenario(_ =>
        {
            _.Get.Url($"/api/contacts/{contactId}");
            _.StatusCodeShouldBe(HttpStatusCode.NotFound);
        });
    }
}

