using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        public enum SortType
        {
            RANKED,
            SCORE,
            ANSWERS,
            TAGS,
            DATE
        }
        public static async Task<StackExchangeRequest> DoSearchAsync(string query)
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

        public static List<Post> SortResults(StackExchangeRequest requestIn, SortType sortType = SortType.RANKED)
        {
            List<Post> posts = requestIn.items;

            // SortedDictionary used to order posts by increasing weighted score
            SortedDictionary<long, List<Post>> sortedPosts = new SortedDictionary<long, List<Post>>();

            foreach (Post post in posts)
            {
                long postRank = GetPostWeightedRank(post, sortType);

                if (!sortedPosts.ContainsKey(postRank))
                {
                    sortedPosts[postRank] = new List<Post>();
                }

                sortedPosts[postRank].Add(post);
            }

            posts.Clear();

            foreach (List<Post> postList in sortedPosts.Values)
            {
                // Add items of same rank in order they appear in list
                for (int i = 0; i < postList.Count; ++i)
                {
                    posts.Insert(i, postList[i]);
                }
            }

            return posts;
        }
		
        public static List<Post> SortResults(List<StackExchangeRequest> requestsIn, SortType sortType = SortType.RANKED)
        {
            StackExchangeRequest combinedRequest = new StackExchangeRequest();
            combinedRequest.items = new List<Post>();
            foreach (StackExchangeRequest req in requestsIn)
            {
                foreach (Post p in req.items)
                {
                    combinedRequest.items.Add(p);
                }
            }

            return SortResults(combinedRequest, sortType);
        }

        public static long GetPostWeightedRank(Post post, SortType sortType = SortType.RANKED)
        {
            const int ANSWERED_WEIGHT = 100;
            const int NUM_ANSWERS_WEIGHT = 2;
            const int SCORE_WEIGHT = 1;
            const int NUM_TAGS_WEIGHT = 2; // TODO: Maybe do some ranking based on tag content as well

            int weightedScore = 0;

            switch (sortType)
            {
                case SortType.RANKED:
                    // Want answered posts to usually come first, unless there is an unanswered post with a really high score for some reason
                    if (post.is_answered)
                    {
                        weightedScore += ANSWERED_WEIGHT;
                    }

                    weightedScore += post.answer_count * NUM_ANSWERS_WEIGHT;

                    weightedScore += post.score * SCORE_WEIGHT;

                    weightedScore += post.tags.Count * NUM_TAGS_WEIGHT;
                    break;
                case SortType.SCORE:
                    weightedScore = post.score;
                    break;
                case SortType.ANSWERS:
                    weightedScore = post.answer_count;
                    break;
                case SortType.TAGS:
                    weightedScore = post.tags.Count;
                    break;
                case SortType.DATE:
                    weightedScore = post.creation_date;
                    break;
                default:
                    break;
            }

            return weightedScore;
        }

        public static string RetrievePostText(int postId)
        {
            string text = "";

            return text;
        }
    }
}
