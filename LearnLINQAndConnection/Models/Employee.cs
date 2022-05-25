using LearnLINQAndConnection.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnLINQAndConnection
{
    class Employee : BaseModel
    {
        public string Name { get; private set; }
        public string Gender { get; private set; }
        public string Phone { get; private set; }


        public Employee()
        {

        }
        public Employee(int ID, string Name, String Gender, String Phone)
            : base(ID)
        {
            this.Name = Name;
            this.Gender = Gender;
            this.Phone = Phone;
        }

        public Employee(string Name, String Gender, String Phone)
        {
            this.Name = Name;
            this.Gender = Gender;
            this.Phone = Phone;
        }
    }
}
