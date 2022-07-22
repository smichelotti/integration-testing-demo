using ContactsApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace ContactsApi.IntegrationTests.Controllers;

[TestCaseOrderer(AlphabeticalOrderer.Name, AlphabeticalOrderer.Assembly)]
public class ContactsCrudOrdered : IntegrationContext
{
    private static string contactId;

    public ContactsCrudOrdered(WebApiFixture fixture) : base(fixture) { }

    [Fact]
    public async Task T01_invalid_contact_should_return_400()
    {
        // arrange
        var contact = new ContactPostCommand
        {
            Address = new(null, "Baltimore", "MD", "21202")
        };

        // act
        var result = await this.Host.Scenario(_ =>
        {
            _.Post.Json(contact).ToUrl("/api/contacts");
            _.StatusCodeShouldBe(HttpStatusCode.BadRequest);
        });

        // assert
        var resp = result.ReadAsJson<ProblemDetailsExt>();
        resp.Title.Should().Be("One or more validation errors occurred.");
        resp.Errors["LastName"].First().Should().Be("Last Name is required.");
        resp.Errors["FirstName"].First().Should().Be("First Name is required.");
        resp.Errors["Address.Street"].First().Should().Be("The Street field is required.");
    }

    [Fact]
    public async Task T02_should_generate_new_contact()
    {
        // arrange
        var contact = new ContactPostCommand
        {
            FirstName = "John",
            LastName = "Smith",
            Address = new("501 E Pratt St", "Baltimore", "MD", "21202")
        };
        this.GeoLocationStub.Given(Request.Create().WithPath("/maps/api/geocode/xml").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBody(File.ReadAllText("Data/geo-location-resp.xml")));

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
        resp.Address.Latitude.Should().BeGreaterThan(39);
        resp.Address.Longitude.Should().BeLessThan(-76);
    }

    [Fact]
    public async Task T03_should_retrieve_contact()
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
        resp.Address.Latitude.Should().BeGreaterThan(39);
        resp.Address.Longitude.Should().BeLessThan(-76);
    }

    [Fact]
    public async Task T04_should_retrieve_contacts_list()
    {
        var result = await this.Host.Scenario(_ =>
        {
            _.Get.Url($"/api/contacts");
            _.StatusCodeShouldBeOk();
        });

        // assert
        var resp = result.ReadAsJson<List<ContactsGetResult>>();
        resp.Count.Should().BeGreaterThan(0);
        resp.Should().Contain(x => x.FirstName == "John");
    }

    [Fact]
    public async Task T05_should_update_existing_contact()
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
        resp.Address.Latitude.Should().BeGreaterThan(39);
        resp.Address.Longitude.Should().BeLessThan(-76);

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
        resp.Address.Latitude.Should().BeGreaterThan(39);
        resp.Address.Longitude.Should().BeLessThan(-76);
    }

    [Fact]
    public async Task T06_should_delete_existing_contact()
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

