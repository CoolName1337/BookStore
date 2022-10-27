using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BAL.Services
{
    public class ActionResult<T> where T : class
    {
        public T? Value { get; set; }
        public List<string> Errors = new List<string>();
        public bool Succeed
        {
            get => Errors.Count == 0;
        }
    }
}
