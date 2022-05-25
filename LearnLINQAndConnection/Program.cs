using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using LearnLINQAndConnection.Repositories.Data;

namespace LearnLINQAndConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //int[] nums = new int[15]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            //var Odd =
            //    from num in nums
            //    where (num%2) == 1
            //    select num;
            //Console.WriteLine(nums.GetType());
            //Console.WriteLine(nums.Average());
            //foreach(var num in Odd)
            //{
            //    Console.WriteLine("{0}, {1}", num, num.GetType());
            //}

            //Dictionary<char, int> dt = new Dictionary<char, int>();
            //char TheKey = 'a';
            //for(int i=0; i<26; i++)
            //{
            //    dt.Add(TheKey, i+1);
            //    TheKey = (char)((int)TheKey+1);
            //}

            //var resultDict = from item in dt
            //                 where ((int)item.Key) < 100
            //                 select item;
            //foreach (var item in resultDict)
            //{
            //    Console.WriteLine("{0}, {1}", item.Key, item.Value);
            //}

            //List<string> ls = new List<string>();
            //ls.Add("Hello");
            //ls.Add("Hello");
            //ls.Add("I'm");
            //ls.Add("an IT Programmer");
            //ls.Add("at");
            //ls.Add("Metrodata");
            //var result = from s in ls
            //             where s.Contains("I")
            //             select s;
            //foreach (var item in result)
            //{
            //    Console.WriteLine("{0}, {1}", item, item.GetType());
            //}

            //List<string> resultTwo = (from s in ls
            //                where s.Contains("I")
            //                select s).ToList();
            //foreach (var item in result)
            //{
            //    Console.WriteLine("{0}, {1}", item, item.GetType());
            //}

            //var resultThree = ls.Any(s => s.Contains("H"));
            //Console.WriteLine(resultThree);
            List<Employee> employees = new List<Employee>();
            SqlConnection conn;
            string ConnectionString;
            ConnectionString = "Data Source=LAPTOP-BIMO;Initial Catalog=MCC66;User ID=sa;Password=123";
            //Database DB = new Database();
            conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Success Connect");
                EmployeeRepository<Employee> employeeRepository = new EmployeeRepository<Employee>(conn);
                bool isAppRunning = true;
                while (isAppRunning)
                {
                    Console.Clear();
                    Console.WriteLine(new Employee().GetType().Name);
                    Console.WriteLine("Welcome To Employees App");
                    Console.WriteLine("1. Create Employee");
                    Console.WriteLine("2. Update Employee");
                    Console.WriteLine("3. Delete Employee");
                    Console.WriteLine("4. See All Employees");
                    Console.WriteLine("5. Find Employee By ID");
                    Console.WriteLine("6. Exit");
                    int option = -1;
                    string input = "";
                    while(!int.TryParse(input, out option))
                    {
                        Console.Write("Select Option: ");
                        input = Console.ReadLine().Trim();
                    }
                    input = "";
                    switch (option)
                    {
                        case 1:
                            Console.Write("Insert Name: ");
                            string Name = Console.ReadLine().Trim();
                            Console.Write("Insert Gender: ");
                            string Gender = Console.ReadLine().Trim();
                            Console.Write("Insert Phone: ");
                            string Phone = Console.ReadLine().Trim();
                            Employee EmployeeToInsert = new Employee(Name, Gender, Phone);
                            //DB.InsertEmployees(conn, new List<Employee>() { emp });
                            employeeRepository.Create(EmployeeToInsert);
                            employeeRepository.SaveChanges();
                            break;
                        case 2:
                            //employees = DB.GetEmployees(conn, "SELECT * FROM EMPLOYEE");
                            employees = employeeRepository.GetAll();
                            for(int i=0; i<employees.Count; i++)
                            {
                                Console.WriteLine($"{i+1}. {employees[i].Name}");
                            }
                            int EmployeeSelected = -1;
                            while (!(int.TryParse(input, out EmployeeSelected) && EmployeeSelected >= 1 && EmployeeSelected <= employees.Count))
                            {
                                Console.Write($"Select Employee [1-{employees.Count}]: ");
                                input = Console.ReadLine().Trim();
                            }
                            input = "";
                            string NewName = "";
                            while (NewName.Length <= 0)
                            {
                                Console.Write("Insert Name: ");
                                NewName = Console.ReadLine().Trim();
                            }
                            string NewGender = "";
                            while (NewGender.Length != 1)
                            {
                                Console.Write("Insert Gender [M/F]: ");
                                NewGender = Console.ReadLine().Trim();
                                if(NewGender[0] != 'M' && NewGender[0] != 'F')
                                {
                                    Console.WriteLine("Gender Must Be M or F!!!");
                                    NewGender = "";
                                }
                            }
                            string NewPhone = "";
                            while (NewPhone.Length <= 0)
                            {
                                Console.Write("Insert Phone: ");
                                NewPhone = Console.ReadLine().Trim();
                            }
                            //DB.UpdateEmployee(conn, new Employee(employees[EmployeeSelected - 1].ID, NewName, NewGender, NewPhone));
                            int UpdatedEmployeeID =  employeeRepository.Update(new Employee(employees[EmployeeSelected - 1].ID, NewName, NewGender, NewPhone)).ID;
                            Console.WriteLine($"Employee with ID {UpdatedEmployeeID} Successfully Updated!!!");
                            employeeRepository.SaveChanges();
                            break;
                        case 3:
                            employees = employeeRepository.GetAll();
                            for (int i = 0; i < employees.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {employees[i].Name}");
                            }

                            int EmployeeDeleted = -1;
                            while (!(int.TryParse(input, out EmployeeDeleted) && EmployeeDeleted >= 1 && EmployeeDeleted <= employees.Count))
                            {
                                Console.Write($"Select Employee [1-{employees.Count}]: ");
                                input = Console.ReadLine().Trim();
                            }
                            //DB.DeleteEmployee(conn, employees[EmployeeDeleted - 1]);
                            EmployeeDeleted = employeeRepository.GetOrDelete(employees[EmployeeDeleted - 1].ID, true).ID;
                            Console.WriteLine($"Employee with ID {EmployeeDeleted} Successfully Deleted!!!");
                            employeeRepository.SaveChanges();
                            break;
                        case 4:
                            //employees = DB.GetEmployees(conn, "SELECT * FROM EMPLOYEE");
                            employees = employeeRepository.GetAll();
                            for (int i = 0; i < employees.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {employees[i].ID} - {employees[i].Name} - {employees[i].Gender} - {employees[i].Phone}");
                            }
                            break;
                        case 5:
                            int EmployeeID = -1;
                            while (!(int.TryParse(input, out EmployeeID) && EmployeeID >= 1))
                            {
                                Console.Write("Insert Employee ID: ");
                                input = Console.ReadLine().Trim();
                            }
                            //string QueryString = $"SELECT * FROM EMPLOYEE WHERE ID = '{EmployeeID}'";
                            //Employee employee = DB.GetEmployeeByID(conn, QueryString);
                            Employee employee = employeeRepository.GetOrDelete(EmployeeID, false);
                            if (employee != null)
                            {
                                Console.WriteLine($"Employee with ID {EmployeeID} is Found!!!");
                                Console.WriteLine($"Employee Name: {employee.Name}");
                                Console.WriteLine($"Employee Gender: {employee.Gender}");
                                Console.WriteLine($"Employee Phone: {employee.Phone}");
                            } else
                            {
                                Console.WriteLine($"Employee with ID {EmployeeID} is Not Found!!!");
                            }
                            break;
                        case 6:
                            isAppRunning = false;
                            Console.WriteLine("Thank you for using this App!!!");
                            break;
                        default:
                            Console.WriteLine("Wrong Input!!!");
                            Console.WriteLine("Input must be between 1 to 6");
                            break;
                    }
                    Console.ReadLine();
                }
                // ---------- INSERT ----------
                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append("INSERT INTO EMPLOYEE VALUES ");
                //stringBuilder.Append("(N'Rudy', N'M', N'08124938942'),");
                //stringBuilder.Append("(N'Wanda', N'F', N'081213237664')");

                // ---------- UPDATE ----------
                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append("UPDATE EMPLOYEE SET PHONE = N'081314953421' WHERE NAME = 'WANDA'");

                // ---------- DELETE ----------
                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append("DELETE FROM EMPLOYEE WHERE NAME = 'Rudy'");

                //using (SqlCommand cmd = new SqlCommand(stringBuilder.ToString(), conn))
                //{
                //    cmd.ExecuteNonQuery();
                //    Console.WriteLine("Query Executed");
                //}
                //conn.Close();

                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append("SELECT * FROM EMPLOYEE");

                //employees = DB.GetEmployees(conn, stringBuilder.ToString());

                ////foreach(Employee employee in employees)
                ////{
                ////    Console.WriteLine("Employee ID     : {0}", employee.ID);
                ////    Console.WriteLine("Employee Name   : {0}", employee.Name);
                ////    Console.WriteLine("Employee Gender : {0}", employee.Gender);
                ////    Console.WriteLine("Employee Phone  : {0}", employee.Phone);
                ////    Console.WriteLine("=========================================");
                ////}

                //var result = employees.Where(e => e.Gender == "M").OrderByDescending(emp => emp.Name);

                //foreach (var item in result)
                //{
                //    Console.WriteLine("Employee ID     : {0}", item.ID);
                //    Console.WriteLine("Employee Name   : {0}", item.Name);
                //    Console.WriteLine("Employee Gender : {0}", item.Gender);
                //    Console.WriteLine("Employee Phone  : {0}", item.Phone);
                //    Console.WriteLine("=========================================");
                //}

                conn.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                throw;
            }
        }
    }
}
