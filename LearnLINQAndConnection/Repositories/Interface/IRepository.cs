using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LearnLINQAndConnection.Repositories.Interface
{
    // Interface for Repository
    interface IRepository<T>
    {
        public void Create(T model);
        public T Update(T model);
        public T GetOrDelete(int ModelId, bool isDelete);
        public List<T> GetAll();

    }
}
