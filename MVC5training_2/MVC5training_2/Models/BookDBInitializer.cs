using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVC5training_2.Models
{
    public class BookDbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext db)
        {
            db.Books.Add(new Book { Name = "Шмяк", Author = "Т. Пратчетт", Price = 220 });
            db.Books.Add(new Book { Name = "Гарри Поттер(Сборник)", Author = "Д. Роулинг", Price = 1200 });
            db.Books.Add(new Book { Name = "Муму", Author = "И. Тургенев", Price = 2.5 });

            base.Seed(db);
        }
    }
}