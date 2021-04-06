using System;
using System.Threading.Tasks;
using Npgsql;

namespace TodoList
{
    class Program
    {

        public static void Main()
        {
            var connString = "Host=127.0.0.1;Username=todolist;Password=andrii;Database=todolist";

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Create("random title");
            Delete(12);
            // Delete(11);
            // Delete(10);

            // Update(2, "Update");

            Read();
            // Retrieve all todo item rows
            void Read()
            {
                using (var cmd = new NpgsqlCommand("SELECT title, done FROM todolist", conn))
                using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    var title = reader.GetString(0);
                    var done = reader.GetBoolean(1);
                    char doneFlag = done ? 'x' : ' ';
                    Console.WriteLine($"- [{doneFlag}] {title}");
                }
            }

            // Insert one todo item
            void Create(TodoItem item)
            {
                using (var insertCmd = new NpgsqlCommand("INSERT INTO todolist (title, done) VALUES (@title, false)", conn))
                {
                    insertCmd.Parameters.AddWithValue("title", item.Title);
                    insertCmd.Parameters.AddWithValue("id", item.Id);
                    insertCmd.Parameters.AddWithValue("description", item.Description ?? "");
                    insertCmd.Parameters.AddWithValue("due_date", NpgsqlTypes.NpgsqlDbType.Date, (object)item.DueDate ?? DBNull.Value);
                    insertCmd.ExecuteNonQuery();
                }
            }

            void Update(TodoItem item)
            {
                using (var insertCmd = new NpgsqlCommand("UPDATE todolist SET description=@description title=@title, done=@done, due_date=@due_date WHERE id=@id", conn))
                {
                    insertCmd.Parameters.AddWithValue("title", item.Title);
                    insertCmd.Parameters.AddWithValue("id", item.Id);
                    insertCmd.Parameters.AddWithValue("description", item.Description ?? "");
                    insertCmd.Parameters.AddWithValue("due_date", NpgsqlTypes.NpgsqlDbType.Date, (object)item.DueDate ?? DBNull.Value);
                    insertCmd.ExecuteNonQuery();
                }
            }

            void Delete(int id)
            {
                using (var insertCmd = new NpgsqlCommand("DELETE FROM todolist WHERE id=@id", conn))
                {
                insertCmd.Parameters.AddWithValue("id", id);
                insertCmd.ExecuteNonQuery();
                }
            }
        }
    }
}