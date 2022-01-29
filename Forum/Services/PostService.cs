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

        #endregion public members
    }
}