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
    public class TopicService
    {
        #region private members

        private ForumContext _forumContext = null;

        #endregion private members

        #region public members

        public TopicService()
        {
            _forumContext = ForumContext.Create();
        }

        public List<Topic> GetTopics()
        {
            return _forumContext.Topics.Where(t => !t.DeletedAt.HasValue).ToList();
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
                    if (topicViewModel.Id == null && topicViewModel.Id == "")
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
                    SqlCommand command = new SqlCommand($"UPDATE Topics SET DeletedAt = @DateTimeNow WHERE Id = @Id", connection);

                    command.Parameters.AddWithValue("DeletedAt", DateTime.Now);
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

        public List<PostViewModel> GetAllPostsByUserId(string userId)
        {
            //todo: implement
            throw new NotImplementedException();
        }

        public int GetPostsCountByUserId(string userId)
        {
            throw new NotImplementedException();
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

        #endregion public members
    }
}