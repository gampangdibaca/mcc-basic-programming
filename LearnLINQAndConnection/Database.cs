using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LearnLINQAndConnection
{
    class Database
    {
        public Database()
        {
            // Empty on Purpose
        }

        public bool ExecuteQuery(SqlConnection conn, string QueryString)
        {
            using (SqlCommand cmd = new SqlCommand(QueryString, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Query Executed");
            }
            return true;
        }

        public List<Employee> GetEmployees(SqlConnection conn, string QueryString)
        {
            SqlCommand cmd = new SqlCommand(QueryString, conn);
            List<Employee> employees = new List<Employee>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }

            return employees;
        }

        public Employee GetEmployeeByID(SqlConnection conn, string QueryString)
        {
            SqlCommand cmd = new SqlCommand(QueryString, conn);
            Employee employee = new Employee();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Employee Not Found");
                    return employee;
                }
                while (reader.Read())
                {
                    employee = new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }

            return employee;
        }

        public void InsertEmployees(SqlConnection conn, List<Employee> employees) 
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO EMPLOYEE VALUES ");
            for(int i=0; i<employees.Count; i++)
            {
                stringBuilder.Append($"(N'{employees[i].Name}', N'{employees[i].Gender}', N'{employees[i].Phone}')");
                if(i != employees.Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }

            bool isSuccess = ExecuteQuery(conn, stringBuilder.ToString());
            if (isSuccess)
            {
                Console.WriteLine("Success Inserting Employee");
            }
        }

        public void UpdateEmployee(SqlConnection conn, Employee employee)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"UPDATE EMPLOYEE SET NAME = N'{employee.Name}'");
            stringBuilder.Append($", GENDER = N'{employee.Gender}', PHONE = N'{employee.Phone} '");
            stringBuilder.Append($"WHERE ID = '{employee.ID}'");

            bool isSuccess = ExecuteQuery(conn, stringBuilder.ToString());
            if (isSuccess)
            {
                Console.WriteLine("Success Updating Employee");
            }
        }

        public void DeleteEmployee(SqlConnection conn, Employee employee)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"DELETE FROM EMPLOYEE WHERE ID = '{employee.ID}'");
            bool isSuccess = ExecuteQuery(conn, stringBuilder.ToString());
            if (isSuccess)
            {
                Console.WriteLine("Success Deleting Employee");
            }
        }

        public Employee GetOrDeleteEmployeeByID(SqlConnection conn, int id, bool isDelete)
        {
            SqlCommand cmd = new SqlCommand($"SELECT * FROM EMPLOYEE WHERE ID = '{id}'", conn);
            Employee employee = new Employee();
            StringBuilder stringBuilder = new StringBuilder();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Employee Not Found");
                    return employee;
                }
                while (reader.Read())
                {
                    employee = new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }

            if (!isDelete)
            {
                return employee;
            }
            stringBuilder.Append($"DELETE FROM EMPLOYEE WHERE ID = '{employee.ID}'");
            bool isSuccess = ExecuteQuery(conn, stringBuilder.ToString());
            if (isSuccess)
            {
                Console.WriteLine("Success Deleting Employee");
                return employee;
            }

            return employee;
        }
    }
}
