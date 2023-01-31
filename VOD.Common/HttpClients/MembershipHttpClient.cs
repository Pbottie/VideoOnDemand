namespace VOD.Common.HttpClients;

public class MembershipHttpClient
{
    public HttpClient Client;

	public MembershipHttpClient(HttpClient client)
	{
		Client= client;
	}


}
