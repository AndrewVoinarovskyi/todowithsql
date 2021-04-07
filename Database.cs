using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace TodoList
{
    class Database
    {
        NpgsqlConnection conn;
        List<TodoItem> todoList;
        public Database(string connString)
        {
            this.conn = new NpgsqlConnection(connString);
            this.conn.Open();
            todoList = new List<TodoItem>();
        }

        public List<TodoItem> Read()
        {
            using (var cmd = new NpgsqlCommand("SELECT id, title, done, description, due_date FROM todolist", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    TodoItem item = new TodoItem()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Done = reader.GetBoolean(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    };
                    if (!reader.IsDBNull(4))
                    {
                        item.DueDate = reader.GetDateTime(4);
                    };

                    // char doneFlag = item.Done ? 'x' : ' ';
                    // Console.WriteLine($"- [{doneFlag}]\t{item.Id}\t{item.Title}\n" +
                    //                     $"\t{item.Description}");
                    todoList.Add(item);
                }
            return todoList;
        }

        public void Create(TodoItem item)
        {
            using (var insertCmd = new NpgsqlCommand("INSERT INTO todolist (title, done) VALUES (@title, false)", conn))
            {
                insertCmd.Parameters.AddWithValue("id", item.Id);

                insertCmd.Parameters.AddWithValue("title", item.Title);
                insertCmd.Parameters.AddWithValue("description", item.Description ?? "");
                insertCmd.Parameters.AddWithValue("due_date", NpgsqlTypes.NpgsqlDbType.Date, (object)item.DueDate ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("done", item.Done);
                insertCmd.ExecuteNonQuery();
            }
        }

        public void Update(TodoItem item)
        {
            using (var insertCmd = new NpgsqlCommand("UPDATE todolist SET description=@description title=@title, done=@done, due_date=@due_date WHERE id=@id", conn))
            {
                insertCmd.Parameters.AddWithValue("id", item.Id);

                insertCmd.Parameters.AddWithValue("title", item.Title);
                insertCmd.Parameters.AddWithValue("description", item.Description ?? "");
                insertCmd.Parameters.AddWithValue("due_date", NpgsqlTypes.NpgsqlDbType.Date, (object)item.DueDate ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("done", item.Done);
                insertCmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var insertCmd = new NpgsqlCommand("DELETE FROM todolist WHERE id=@id", conn))
            {
                insertCmd.Parameters.AddWithValue("id", id);
                insertCmd.ExecuteNonQuery();
            }
        }
    }
}