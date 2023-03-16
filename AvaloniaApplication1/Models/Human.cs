using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Models
{
    public class Human
    {
        private string _name;
        private int _age;   
        public string Name { get { return _name; } set { _name = value; } }
        public int Age { get { return _age;} set { _age = value; } }
        public Human(string name, int age)
        {
            _name = name;
            _age = age;
        }

    }
}
