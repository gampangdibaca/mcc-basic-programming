using LearnLINQAndConnection.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LearnLINQAndConnection.Repositories.Interface
{
    class GeneralRepository<T> : IRepository<T> where T: BaseModel
    {

        protected SqlConnection Connection;
        protected List<T> ListT;
        protected List<string> ListQuery;

        public GeneralRepository()
        {

        }
        public GeneralRepository(SqlConnection Connection)
        {
            this.Connection = Connection;
            ListT = new List<T>();
            ListQuery = new List<string>();
        }

        public virtual void Create(T t)
        {
            ListT.Add(t);
        }

        public virtual List<T> GetAll()
        {
            return ListT;
        }

        public virtual T GetOrDelete(int ModelId, bool isDelete)
        {
            if (ListT.Count <= 0)
            {
                Console.WriteLine("Repository is Empty!!!");
                return null;
            }

            if (!isDelete)
            {
                foreach (T item in ListT)
                {
                    if(item.ID == ModelId)
                    {
                        return item;
                    }
                }
            }
            int DeletedIndex = -1;
            for(int i=0; i<ListT.Count; i++)
            {
                if(ListT[i].ID == ModelId)
                {
                    DeletedIndex = i;
                    break;
                }
            }
            if(DeletedIndex == -1)
            {
                return null;
            }
            T TModel = ListT[DeletedIndex];
            ListT.RemoveAt(DeletedIndex);
            return TModel;
        }

        public virtual T Update(T UpdatedModel)
        {
            for(int i=0; i<ListT.Count; i++)
            {
                if(ListT[i].ID == UpdatedModel.ID)
                {
                    ListT[i] = UpdatedModel;
                }
            }
            return UpdatedModel;
        }

        public bool ExecuteQuery(SqlConnection conn, string QueryString)
        {
            using (SqlCommand cmd = new SqlCommand(QueryString, conn))
            {
                cmd.ExecuteNonQuery();
            }
            return true;
        }

    }
}
