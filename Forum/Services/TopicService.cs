using Forum.Data;
using Forum.Entities;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace Forum.Services
{
    public class TopicService
    {
        #region private members

        private ForumContext _forumContext = null;
        private PostService _postService = null;

        #endregion private members

        #region public members

        public TopicService()
        {
            _forumContext = ForumContext.Create();
            _postService = new PostService();
        }

        public List<TopicViewModel> GetTopics()
        {
            List<TopicViewModel> topicViewModels = new List<TopicViewModel>();
            List<Post> posts = new List<Post>();

            var topics = _forumContext.Topics.Include("Threads.Posts").Where(k => !k.DeletedAt.HasValue).ToList();

            topicViewModels.AddRange(topics.Select(h => new TopicViewModel
            {
                Id = h.Id,
                UserId = h.UserId,
                Title = h.Title,
                LastPost = GetLatestPost(h.Id),
                PostCount = _postService.GetPostsCountByTopicId(h.Id)
            }));

            return topicViewModels;
        }

        public TopicViewModel GetTopic(string id)
        {
            TopicViewModel topicViewModel = new TopicViewModel();
            try
            {


                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"SELECT *
                                                        FROM Topics 
                                                        WHERE Id = @Id AND DeletedAt IS NULL", connection);

                    command.Parameters.AddWithValue("Id", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        topicViewModel.Id = reader["Id"].ToString();
                        topicViewModel.UserId = reader["UserId"].ToString();
                        topicViewModel.Title = reader["Title"].ToString();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return topicViewModel;
        }

        public bool Save(TopicViewModel topicViewModel)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    if (topicViewModel.Id == null || topicViewModel.Id == "")
                    {
                        SqlCommand command = new SqlCommand(@"INSERT INTO 
                                                                Topics(Id, UserId, Title, CreatedAt, UpdatedAt) 
                                                                VALUES(@Id, @UserId, @Title, @CreatedAt, @UpdatedAt)", connection);
                        topicViewModel.Id = Guid.NewGuid().ToString();
                        topicViewModel.CreatedAt = topicViewModel.UpdatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("Id", topicViewModel.Id);
                        command.Parameters.AddWithValue("UserId", topicViewModel.UserId);
                        command.Parameters.AddWithValue("Title", topicViewModel.Title);
                        command.Parameters.AddWithValue("CreatedAt", topicViewModel.CreatedAt);
                        command.Parameters.AddWithValue("UpdatedAt", topicViewModel.UpdatedAt);
                        connection.Open();
                        result = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand(@"UPDATE Topics
                                                            SET Title = @Title,
                                                                UpdatedAt = @UpdatedAt
                                                            WHERE Topics.Id = @Id", connection);

                        topicViewModel.UpdatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("Id", topicViewModel.Id);
                        command.Parameters.AddWithValue("Title", topicViewModel.Title);
                        command.Parameters.AddWithValue("UpdatedAt", topicViewModel.UpdatedAt);
                        connection.Open();
                        result = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return result;
        }

        public bool Delete(string id)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"DeleteTopic", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("TopicId", id);

                    connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return result;
        }

        public static string GetTopicTitleById(string topicId)
        {
            string threadTitle = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"SELECT Topics.Title
                                                            FROM Topics 
                                                            WHERE Id = @Id AND DeletedAt IS NULL", connection);

                    command.Parameters.AddWithValue("Id", topicId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        threadTitle = reader["Title"].ToString();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return threadTitle;
        }

        public Topic GetTopicById(string topicId)
        {
            return _forumContext.Topics.Where(h => h.Id == topicId && !h.DeletedAt.HasValue).Include("Threads.Posts").Where(k => !k.DeletedAt.HasValue).First();
        }

        public PostViewModel GetLatestPost(string topicId)
        {
            var threads = GetTopicById(topicId).Threads;

            foreach (var item in threads)
            {
                var lastPost = item.Posts.OrderByDescending(c => c.CreatedAt).FirstOrDefault();

                if (lastPost != null)
                {
                    return new PostViewModel
                    {
                        Id = lastPost.Id,
                        Body = lastPost.Body,
                        CreatedAt = lastPost.CreatedAt,
                    };
                }
            }

            return new PostViewModel();
        }

        #endregion public members
    }
}