

using VOD.Common.DTOs;

namespace VOD.Common.HttpClients;

public class UserHttpClient
{
    public HttpClient Client;

    public UserHttpClient(HttpClient client)
    {
        Client = client;
    }

    public async Task CreateUser(CreateUserModel model)
    {
        try
        {
            if (model == null) { throw new ArgumentException("CreatedUserModel is null"); }
            List<string> roles = new List<string> { UserRole.Registered };
            if (model.IsCustomer) { roles.Add(UserRole.Customer); }
            if (model.IsAdmin) { roles.Add(UserRole.Admin); }

            RegisterUserDTO user = new(model.Email, model.Password, roles);

            StringContent jsonContent = new(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");


            var response = await Client.PostAsync("users/register", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

        }
        catch (Exception)
        {
            throw;
        }



    }
    public async Task PayingCustomer(PaidCustomerDTO model)
    {
        try
        {
            if (model == null) { throw new ArgumentException("PaidCustomerDTO is null"); }


            StringContent jsonContent = new(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");


            var response = await Client.PostAsync("users/paid", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

        }
        catch (Exception)
        {
            throw;
        }



    }


}
