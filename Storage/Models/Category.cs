using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(Category other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Name.Equals(Name);
        }
    }
}
