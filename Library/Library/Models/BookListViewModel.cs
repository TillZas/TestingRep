using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookListViewModel
    {
        public List<Book> list = null;
        public int total = 0;

        public BookListViewModel() { }
        public BookListViewModel(List<Book> list) 
        {
            this.list = list;
            total = list.ToArray().Length;
        }
    }
}
