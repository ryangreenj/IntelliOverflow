﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntelliOverflowAPI
{
    public class Owner
    {
        public int account_id { get; set; }
        public int reputation { get; set; }
        public int user_id { get; set; }
        public string user_type { get; set; }
        public string profile_image { get; set; }
        public string display_name { get; set; }
        public string link { get; set; }
        public int? accept_rate { get; set; }
    }

    public class Post
    {
        public List<string> tags { get; set; }
        public Owner owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int answer_count { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int creation_date { get; set; }
        public int question_id { get; set; }
        public string content_license { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public int? last_edit_date { get; set; }
        public int? bounty_amount { get; set; }
        public int? bounty_closes_date { get; set; }
        public int? accepted_answer_id { get; set; }
        public int? closed_date { get; set; }
        public string closed_reason { get; set; }
    }

    public class StackExchangeRequest
    {
        public List<Post> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }

    public class API
    {
        public static async System.Threading.Tasks.Task<StackExchangeRequest> DoSearchAsync(string query)
        {
            StackExchangeRequest request;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://api.stackexchange.com");
                //client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                query = query.Replace(' ', '+');

                string apiCall = "https://api.stackexchange.com/search/advanced?site=stackoverflow.com&q=" + query;

                //HttpResponseMessage response = client.GetAsync(apiCall).Result;
                //response.EnsureSuccessStatusCode();

                // StackExchange API sends compressed response, need to decompress it after reading as bytes
                var rawBytes = await client.GetByteArrayAsync(apiCall);
                string result = new StreamReader(new GZipStream(new MemoryStream(rawBytes), CompressionMode.Decompress)).ReadToEnd();

                request = JsonConvert.DeserializeObject<StackExchangeRequest>(result);
            }

            return request;
        }

        public static List<Post> RankResults(StackExchangeRequest requestIn)
        {
            List<Post> posts = requestIn.items;

            return posts;
        }

        public static string RetrievePostText(int postId)
        {
            string text = "";

            return text;
        }
    }
}
