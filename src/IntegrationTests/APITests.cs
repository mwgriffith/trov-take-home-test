using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace IntegrationTests
{
    public class APITests : IClassFixture<CustomWebApplicationFactory<GuildedRoseAPI.Startup>>
    {

        private readonly CustomWebApplicationFactory<GuildedRoseAPI.Startup> _factory;

        public APITests(CustomWebApplicationFactory<GuildedRoseAPI.Startup> factory)
        {
            _factory = factory;
        }


        [Fact]        
        public async Task Test_Api_Item_GetInventory()
        {
            var client = _factory.CreateClient();                                                
            var response = await client.GetAsync("/api/item/getinventory/");

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());            
        }



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



        [Fact]
        public async Task Test_Api_Item_BuyItem_BadAuth()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/item/buyitem/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }



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
