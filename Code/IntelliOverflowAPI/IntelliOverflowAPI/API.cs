using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntelliOverflowAPI
{
    public class StackExchangeRequest
    {
        public List<Post> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }

    public class Post

    {
        public List<string> tags { get; set; }
        public PostOwner owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int accepted_answer_id { get; set; }
        public int answer_count { get; set; }
        public int score { get; set; }
        public long last_activity_date { get; set; }
        public long creation_date { get; set; }
        public long last_edit_date { get; set; }
        public int question_id { get; set; }
        public string link { get; set; }
        public string title { get; set; }
    }

    public class PostOwner
    {
        public int account_id { get; set; }
        public int reputation { get; set; }
        public int user_id { get; set; }
        public string display_name { get; set; }
    }

    public class API
    {
        public StackExchangeRequest DoSearch(string query)
        {
            StackExchangeRequest request;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.stackexchange.com");
                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string apiCall = "search/advanced?site=stackoverflow.com&q=" + query;

                HttpResponseMessage response = client.GetAsync(apiCall).Result;
                response.EnsureSuccessStatusCode();

                request = response.Content.ReadAsAsync<StackExchangeRequest>().Result;
            }

            return request;
        }

        public List<Post> RankResults(StackExchangeRequest requestIn)
        {
            List<Post> posts = requestIn.items;

            return posts;
        }

        public string RetrievePostText(int postId)
        {
            string text = "";

            return text;
        }
    }
}
