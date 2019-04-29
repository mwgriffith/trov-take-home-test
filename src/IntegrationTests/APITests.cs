using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xunit;

namespace IntegrationTests
{
    /// <summary>
    /// Performs the Integration test for the GuildedRoseAPI
    /// </summary>
    public class APITests : IClassFixture<CustomWebApplicationFactory<GuildedRoseAPI.Startup>>
    {

        private readonly CustomWebApplicationFactory<GuildedRoseAPI.Startup> _factory;

        public APITests(CustomWebApplicationFactory<GuildedRoseAPI.Startup> factory)
        {
            _factory = factory;
        }


        /// <summary>
        /// Tests the /api/item/getinventory/ call.
        /// </summary>        
        [Fact]        
        public async Task Test_Api_Item_GetInventory()
        {
            var client = _factory.CreateClient();                                                
            var response = await client.GetAsync("/api/item/getinventory/");

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());            
        }



        /// <summary>
        /// Tests the /api/authentication/gettoken call with a passed in username and password.
        /// </summary>        
        [Fact]
        public async Task Test_Api_Authentication_GetToken()
        {
            var client = _factory.CreateClient();
                        
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();

            pairs.Add(new KeyValuePair<string, string>("username", "username"));
            pairs.Add(new KeyValuePair<string, string>("password", "password"));

            var postContent = new FormUrlEncodedContent(pairs);

            var response = await client.PostAsync("/api/authentication/gettoken", postContent);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();
            Assert.NotNull(token);
        }



        /// <summary>
        /// Tests the /api/item/buyitem/ call and make sure it needs the auth token.
        /// </summary>        
        [Fact]
        public async Task Test_Api_Item_BuyItem_BadAuth()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/item/buyitem/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }



        /// <summary>
        /// Tests the /api/item/buyitem/ call, also calls the /api/authentication/gettoken call to get a good token for the buyitem call.
        /// </summary>        
        [Fact]
        public async Task Test_Api_Item_BuyItem()
        {
            var client = _factory.CreateClient();

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();

            pairs.Add(new KeyValuePair<string, string>("username", "username"));
            pairs.Add(new KeyValuePair<string, string>("password", "password"));

            var postContent = new FormUrlEncodedContent(pairs);

            var response = await client.PostAsync("/api/authentication/gettoken", postContent);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = await client.GetAsync("/api/item/buyitem/1");
            response2.EnsureSuccessStatusCode();
            var result = await response2.Content.ReadAsStringAsync();
            Assert.True(Convert.ToBoolean(result));
        }
    }
}
