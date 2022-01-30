using Forum.Data;
using Forum.Entities;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Forum.Services
{
    public class ThreadService
    {
        #region private members

        private ForumContext _forumContext = null;
        private PostService _postService = null;

        #endregion private members

        #region public members

        public ThreadService()
        {
            _forumContext = ForumContext.Create();
            _postService = new PostService();
        }

        public List<ThreadViewModel> GetThreadsByTopicId(string topicId)
        {
            List<ThreadViewModel> threads = new List<ThreadViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"SELECT Threads.*,
		                                                        (SELECT Topics.Title FROM Topics WHERE Topics.Id = Threads.TopicId) AS 'TopicTitle'
                                                           FROM Threads
                                                           WHERE Threads.TopicId = @TopicId AND Threads.DeletedAt IS NULL", connection);

                    command.Parameters.AddWithValue("TopicId", topicId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        threads.Add(new ThreadViewModel
                        {
                            Id = reader["Id"].ToString(),
                            UserId = reader["UserId"].ToString(),
                            TopicId = reader["TopicId"].ToString(),
                            Title = reader["Title"].ToString(),
                            Inactive = (bool)reader["Inactive"],
                            Description = reader["Description"].ToString(),
                            TopicTitle = DBNull.Value == reader["TopicTitle"] ? "" : reader["TopicTitle"].ToString(), //it could bring an error 
                            LastPost = _postService.GetThreadsLastPostInfoById(reader["Id"].ToString()),
                            PostCount = _postService.GetPostsCountByThreadId(reader["Id"].ToString())
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return threads;
        }

        public ThreadViewModel GetThread(string id)
        {
            ThreadViewModel threadViewModel = new ThreadViewModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"SELECT *
                                                            FROM Threads 
                                                            WHERE Id = @Id AND DeletedAt IS NULL", connection);
                    command.Parameters.AddWithValue("Id", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        threadViewModel.Id = reader["Id"].ToString();
                        threadViewModel.UserId = reader["UserId"].ToString();
                        threadViewModel.TopicId = reader["TopicId"].ToString();
                        threadViewModel.Title = reader["Title"].ToString();
                        threadViewModel.Inactive = (bool)reader["Inactive"];
                        threadViewModel.Description = reader["Description"].ToString();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return threadViewModel;
        }

        public bool Save(ThreadViewModel threadViewModel)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    if (threadViewModel.Id == null || threadViewModel.Id == "")
                    {
                        SqlCommand command = new SqlCommand(@"INSERT INTO 
                                                               Threads (Id, UserId, TopicId, Title, Description, Inactive, CreatedAt, UpdatedAt) 
                                                               VALUES(@Id, @UserId, @TopicId, @Title, @Description, @Inactive, @CreatedAt, @UpdatedAt)", connection);
                        threadViewModel.Id = Guid.NewGuid().ToString();
                        threadViewModel.CreatedAt = threadViewModel.UpdatedAt = DateTime.Now;

                        command.Parameters.AddWithValue("Id", threadViewModel.Id);
                        command.Parameters.AddWithValue("UserId", threadViewModel.UserId);
                        command.Parameters.AddWithValue("TopicId", threadViewModel.TopicId);
                        command.Parameters.AddWithValue("Title", threadViewModel.Title);
                        command.Parameters.AddWithValue("Description", threadViewModel.Description);
                        command.Parameters.AddWithValue("Inactive", threadViewModel.Inactive);
                        command.Parameters.AddWithValue("CreatedAt", threadViewModel.CreatedAt);
                        command.Parameters.AddWithValue("UpdatedAt", threadViewModel.UpdatedAt);

                        connection.Open();
                        result = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand(@"UPDATE Threads
                                                            SET Title = @Title,
                                                                Description = @Description,
                                                                Inactive = @Inactive,
                                                                UpdatedAt = @UpdatedAt
                                                            WHERE Threads.Id = @Id", connection);
                        threadViewModel.UpdatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("Id", threadViewModel.Id);
                        command.Parameters.AddWithValue("Title", threadViewModel.Title);
                        command.Parameters.AddWithValue("Description", threadViewModel.Description);
                        command.Parameters.AddWithValue("Inactive", threadViewModel.Inactive);
                        command.Parameters.AddWithValue("UpdatedAt", threadViewModel.UpdatedAt);

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
                    SqlCommand command = new SqlCommand($@"UPDATE Threads SET DeletedAt = @DateTimeNow WHERE Id = @Id
                                                            UPDATE Posts SET DeletedAt = @DateTimeNow WHERE ThreadId = @Id", connection);

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


        //todo: change
        public static ThreadViewModel GetThreadInfoById(string threadId)
        {
            ThreadViewModel threadViewModel = new ThreadViewModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
                {
                    SqlCommand command = new SqlCommand($@"SELECT Threads.Title, Threads.Inactive, Threads.Description
                                                        FROM Threads 
                                                        WHERE Id = @Id AND DeletedAt IS NULL", connection);
                    command.Parameters.AddWithValue("Id", threadId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        threadViewModel.Title = reader["Title"].ToString();
                        threadViewModel.Inactive = (bool)reader["Inactive"];
                        threadViewModel.Description = reader["Description"].ToString();
                    }

                    connection.Close();

                    return threadViewModel;
                }
            }
            catch (Exception ex)
            {
                //todo: add log
            }

            return threadViewModel;
        }
       
        #endregion public members
    }
}