﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using static Library.Models.LibraryModels;
using Library.utils;

namespace Library.Controllers
{

    public class LibraryController : Controller
    {

        LibraryRepository libRes = new LibraryRepository("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Work\\TestingRep\\Library\\Library\\App_Data\\LibraryDB.mdf;Integrated Security=True;Connect Timeout=30");
        RandomGenerator rgen = new RandomGenerator();

        public string Index()//IActionResult
        {
            List<Book> bks = libRes.GetBooks();
            string s = "Books:\n";
            foreach(Book bk in bks)
            {
                if(bk.AuthorRefId!=null)
                    s +=bk.BookId +" " + bk.Title + " " + bk.AuthorRefId + " "+ bk.Author.Name+"\n";
                else
                    s += bk.BookId + " " + bk.Title + "\n";
            }
            List<Author> aths = libRes.GetAuthors();
            s += "\n\n\nBooks:\n";
            foreach (Author ath in aths)
            {
                s += ath.AuthorId + " " + ath.Name + "\n";
            }
            return s;
        }

        public string Add(string title = null,int? author = null)
        {
            Book bk = new Book();
            bk.Title = title == null ? rgen.GetRandomTitle() : title;
            bk.AuthorRefId = author;
            libRes.Create(bk);
            return "Created " + title + " " + bk.AuthorRefId;
        }

        public string AddAuthor(string name = null)
        {
            Author ath = new Author();
            ath.Name = name==null?rgen.GetRandomName():name;
            libRes.Create(ath);
            return "Borned " + name;
        }

        public string Show(int id = 3)
        {
            Book bk = libRes.GetBook(id);
            if (bk != null)
                return "Book " + id + ": " + bk.Title + " " + bk.AuthorRefId + " " + bk.Author;
            else return "No such book";
        }
    }
}