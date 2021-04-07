using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace TodoList
{
    class Program
    {

        public static void Main()
        {
            Database database = new Database("Host=127.0.0.1;Username=todolist;Password=andrii;Database=todolist");
            List<TodoItem> todoList = database.Read();
            Console.WriteLine(todoList);
        }
    }
}