using System;
using System.Collections.Generic;
using System.Text;

namespace LearnLINQAndConnection.Models
{
    class BaseModel
    {
        public int ID { get; set; }


        public BaseModel()
        {

        }
        public BaseModel(int ID)
        {
            this.ID = ID;
        }
    }
}
