using System;
using System.Linq;
using System.Collections.Generic;

using Forum.Entities;
using Forum.Models;
using System.Data.SqlClient;
using Forum.Data;

namespace Forum.Services
{
    public class PostService
    {
        #region private members

        private ForumContext _forumContext = null;

        #endregion private members

        #region public members

        public PostService()
        {
            _forumContext = ForumContext.Create();
        }

        public List<PostViewModel> GetPostsByThreadId(string threadId)
        {
            List<PostViewModel> posts = new List<PostViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    //with subquery
                    SqlCommand command = new SqlCommand($@"SELECT Posts.*, 
                                                                  Threads.Title AS 'ThreadTitle',
                                                                  Threads.Inactive,
		                                                          (SELECT AspNetUsers.UserName FROM AspNetUsers WHERE AspNetUsers.Id = Posts.UserId) AS 'Nickname'
                                                           FROM Posts
                                                            LEFT JOIN Threads ON Threads.Id = Posts.ThreadId
                                                           WHERE Posts.ThreadId = @ThreadId AND Posts.DeletedAt IS NULL", connection);

                    command.Parameters.AddWithValue("ThreadId", threadId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        posts.Add(new PostViewModel
                        {
                            Id = reader["Id"].ToString(),
                            UserId = reader["UserId"].ToString(),
                            Body = reader["Body"].ToString(),
                            ThreadId = reader["ThreadId"].ToString(),
                            ThreadTitle = reader["ThreadTitle"].ToString(),
                            Nickname = reader["Nickname"].ToString(),
                            IsCurrentThreadInactive = (bool)reader["Inactive"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"],

                        });
                    }

                    connection.Close();

                    return posts.OrderByDescending(t => t.CreatedAt.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return posts;
        }

        public PostViewModel GetPostById(string postId)
        {
            PostViewModel postViewModel = new PostViewModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"GetPost", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Id", postId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        postViewModel.Id = reader["Id"].ToString();
                        postViewModel.UserId = reader["UserId"].ToString();
                        postViewModel.ThreadId = reader["ThreadId"].ToString();
                        postViewModel.ThreadTitle = reader["ThreadTitle"].ToString();
                        postViewModel.Body = reader["Body"].ToString();
                        postViewModel.UserInfo = new UserInfo();
                        postViewModel.UserInfo.Nickname = reader["UserName"].ToString();
                        postViewModel.UserInfo.City = reader["City"].ToString();
                        postViewModel.UserInfo.Country = reader["Country"].ToString();
                        postViewModel.UserInfo.CreatedAt = (DateTime)reader["UserJoinedDate"];
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return postViewModel;
        }

        public bool Delete(string id)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"UPDATE Posts SET DeletedAt = @DateTimeNow WHERE Id = @Id", connection);

                    command.Parameters.AddWithValue("DateTimeNow", DateTime.Now);
                    command.Parameters.AddWithValue("Id", id);

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

        public bool Post(PostViewModel postViewModel)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand(@"INSERT INTO 
                                                           Posts (Id, ThreadId, UserId, Body, CreatedAt, UpdatedAt) 
                                                           VALUES(@Id, @ThreadId, @UserId, @Body, @CreatedAt, @UpdatedAt)", connection);

                    command.Parameters.AddWithValue("Id", Guid.NewGuid());
                    command.Parameters.AddWithValue("UserId", postViewModel.UserId);
                    command.Parameters.AddWithValue("ThreadId", postViewModel.ThreadId);
                    command.Parameters.AddWithValue("Body", postViewModel.Body);
                    command.Parameters.AddWithValue("CreatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("UpdatedAt", DateTime.Now);

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

        public bool EditPost(PostViewModel postViewModel)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand(@"EditPost", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Id", postViewModel.Id);
                    command.Parameters.AddWithValue("Body", postViewModel.Body);

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

        public List<PostViewModel> GetAllPostsByUserId(string userId)
        {
            var topics = _forumContext.Topics.Where(t => !t.DeletedAt.HasValue);
            foreach (var item in topics)
            {
                _forumContext.Entry(item).Collection("UserId");
            }
            //todo: implement
            throw new NotImplementedException();
        }

        public int GetPostsCountByTopicId(string topicId)
        {
            var topic = _forumContext.Topics.Include("Threads.Posts").Where(k => !k.DeletedAt.HasValue && k.Id == topicId).FirstOrDefault();
            List<Post> posts = new List<Post>();

            if (topic != null)
            {
                foreach (var item in topic.Threads)
                {
                    posts.AddRange(item.Posts);
                }
            }

            return posts.Count;
        }

        public int GetPostsCountByThreadId(string threadId)
        {
            return _forumContext.Posts.Where(k => !k.DeletedAt.HasValue && k.ThreadId == threadId).Count();
        }

        public List<Post> GetAllPostsByThreadId(string threadId)
        {
            return _forumContext.Posts.Where(thread => thread.ThreadId == threadId).ToList();
        }

        public int GetPostsCountByUserId(string userId)
        {
            return _forumContext.Posts.Where(p => p.UserId == userId).Count();
        }

        public UserInfo GetUserInfoForPost(string topicId)
        {
            UserInfo userInfo = new UserInfo();
            return userInfo;
        }

        public PostViewModel GetThreadsLastPostInfoById(string threadId)
        {
            List<Thread> threads = _forumContext.Threads.Include("Posts").Where(t => !t.DeletedAt.HasValue && t.Id == threadId).ToList();
            List<Post> posts = new List<Post>();

            foreach (var item in threads)
            {
                Post lastPost = item.Posts.OrderByDescending(v => v.CreatedAt).FirstOrDefault();

                if (lastPost != null)
                {
                    return new PostViewModel
                    {
                        Id = lastPost.Id,
                        Body = lastPost.Body,
                        UserId = lastPost.UserId,
                        CreatedAt = lastPost.CreatedAt
                    };
                }
            }

            return new PostViewModel();
        }

        #endregion public members
    }
}