using LearnLINQAndConnection.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LearnLINQAndConnection.Repositories.Data
{
    class EmployeeRepository<T> : GeneralRepository<T> where T : Employee
    {

        public EmployeeRepository(SqlConnection Connection)
            :base(Connection)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM EMPLOYEE", Connection);
            ListT = new List<T>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ListT.Add((T)new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
        }

        public override void Create(T t)
        {
            base.Create(t);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO EMPLOYEE VALUES ");
            stringBuilder.Append($"(N'{t.Name}', N'{t.Gender}', N'{t.Phone}')");
            ListQuery.Add(stringBuilder.ToString());
        }

        public override T Update(T UpdatedModel)
        {
            base.Update(UpdatedModel);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"UPDATE EMPLOYEE SET NAME = N'{UpdatedModel.Name}'");
            stringBuilder.Append($", GENDER = N'{UpdatedModel.Gender}', PHONE = N'{UpdatedModel.Phone} '");
            stringBuilder.Append($"WHERE ID = '{UpdatedModel.ID}'");
            ListQuery.Add(stringBuilder.ToString());
            return UpdatedModel;
        }

        public override T GetOrDelete(int ModelId, bool isDelete)
        {
            if (isDelete)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"DELETE FROM EMPLOYEE WHERE ID = '{ModelId}'");
                ListQuery.Add(stringBuilder.ToString());
            }
            return base.GetOrDelete(ModelId, isDelete);
        }

        public void SaveChanges()
        {
            foreach (string query in ListQuery)
            {
                bool isSuccess = ExecuteQuery(Connection, query);
                if (isSuccess)
                {
                    if (query.Contains("INSERT"))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('EMPLOYEE')", Connection))
                        {
                            object obj = cmd.ExecuteScalar();
                            ListT[ListT.Count - 1].ID = int.Parse(obj.ToString());
                        }
                    }
                    Console.WriteLine("Success Saving Data");
                }
                else
                {
                    Console.WriteLine("Failed Saving Data");
                }
            }
            ListQuery.Clear();
            
        }
    }
}
