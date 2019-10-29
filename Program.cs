using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");

            bool cont = true;

            do
            {
                //Menu
                Console.WriteLine("Press 1 to display all Blogs: ");
                Console.WriteLine("Press 2 to add a Blog: ");
                Console.WriteLine("Press 3 to Create a post: ");
                Console.WriteLine("Press 4 to Exit Program:");
                int input = 0;
                string fix = Console.ReadLine();
                if (!int.TryParse(fix,out input))
                {
                    logger.Error("failed to parse int");
                }

                try
                {

                    if (input == 1)
                    {
                        var db = new BloggingContext();
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (input == 2)
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    else if (input == 3)
                    {
                        //Create and save new Post
                        var db = new BloggingContext();

                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        Console.WriteLine("Select the blog you would like to post to: ");
                        var blog = Console.ReadLine();
                        var query1 = db.Blogs.Where(b => b.Name.ToLower() == blog.ToLower()).First();
                        Console.WriteLine("Enter the title of your Post: ");
                        var title = Console.ReadLine();

                        Console.Write("Enter your Post Content: ");
                        var content = Console.ReadLine();

                        var post = new Post { Title = title, Content = content, BlogId = query1.BlogId };

                        db.AddPost(post);
                        logger.Info("Post added - {content}", content);
                    }
                    else if (input == 4)
                    {
                        cont = false;
                    }


                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
                logger.Info("Program ended");
            } while (cont);
        }
    }
}