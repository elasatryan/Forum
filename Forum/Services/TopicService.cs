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
        private ForumContext _forumContext = null;

        public TopicService()
        {
            _forumContext = new ForumContext();
        }

        public List<Topic> GetTopics()
        {
            return _forumContext.Topics.Where(t => !t.DeletedAt.HasValue).ToList();
        }

        public TopicViewModel GetTopic(string id)
        {
            TopicViewModel topicViewModel = new TopicViewModel();
            using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
            {
                SqlCommand command = new SqlCommand($@"SELECT *
                                                        FROM Topics 
                                                        WHERE Id = @Id AND DeletedAt IS NULL", connection);
                command.Parameters.AddWithValue("Id", id);
                connection.Open();
                var topic = command.ExecuteReader();
                if (topic.Read())
                {
                    topicViewModel.Id = topic["Id"].ToString();
                    topicViewModel.UserId = topic["UserId"].ToString();
                    topicViewModel.Title = topic["Title"].ToString();
                }
                connection.Close();

                return topicViewModel;
            }
        }

        public void Save(TopicViewModel topicViewModel)
        {
            using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Topics(Id, UserId, Title, CreatedAt, UpdatedAt) VALUES(@Id, @UserId, @Title, @CreatedAt, @UpdatedAt)", connection);
                command.Parameters.AddWithValue("Id", Guid.NewGuid());
                command.Parameters.AddWithValue("UserId", topicViewModel.UserId);
                command.Parameters.AddWithValue("Title", topicViewModel.Title);
                command.Parameters.AddWithValue("CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("UpdatedAt", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(string id)
        {
            using (SqlConnection connection = new SqlConnection(Defines.DefaultConnection))
            {
                SqlCommand command = new SqlCommand($"UPDATE Topics SET DeletedAt = '{DateTime.Now}' WHERE Id = '{id}'", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }
}