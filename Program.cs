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
                Console.WriteLine("Press 4 to display post: ");
                Console.WriteLine("Enter q to quit: ");
                var input = Console.ReadLine();
               
                try
                {

                    if (input == "1")
                    {
                        var db = new BloggingContext();
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);
                        int num = query.Count();


                        Console.WriteLine($"{num} blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (input == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        //blank blog entry error
                        if (name.Length == 0)
                        {
                            logger.Error("Blog name cannot be null");
                        }
                        else
                        {
                            var blog = new Blog { Name = name };

                            var db = new BloggingContext();
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                            logger.Error("Blog name cannot be null");
                        }
                    }

                    else if (input == "3")
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
                        var query1 = db.Blogs.Where(b => b.Name.ToLower() == blog.ToLower()).FirstOrDefault();
                        //Invalid Blog Name or no blog name entered error
                        if (blog.Length == 0)
                        {
                            logger.Error("Blog Name is not found");
                        }
                        else if (query1 == null)
                        {
                            logger.Error("Blog Name is not found");
                        }
                        else
                        {
                            Console.WriteLine("Enter the title of your Post: ");
                            var title = Console.ReadLine();
                            //Error when no title entered
                            if (title.Length == 0)
                            {
                                logger.Error("Post title cannot be null");

                            }
                            else
                            {
                                Console.Write("Enter your Post Content: ");
                                var content = Console.ReadLine();
                                if (content.Length == 0)
                                {
                                    logger.Error("Post content cannot be null");


                                }
                                else
                                {
                                    var post = new Post { Title = title, Content = content, BlogId = query1.BlogId };



                                    db.AddPost(post);
                                    logger.Info("Post added - {content}", content);
                                }
                            }
                        }
                    }
                    else if (input == "4")
                    {
                        //display Post
                        var db1 = new BloggingContext();

                        var query = db1.Posts.OrderBy(p => p.Content);
                        Console.WriteLine("Press 0 to Post from all Blogs: ");

                        var query1 = db1.Blogs;


                        foreach (var item in query1)
                        {
                            Console.WriteLine("Press {0} for {1} Post:",item.BlogId,item.Name);

                        }

                            int input1 = 0;
                        string fix1 = Console.ReadLine();
                        if (!int.TryParse(fix1, out input1))
                        {
                            logger.Error("failed to parse int");
                        }
                        if (input1 == 0)
                        {
                            Console.WriteLine("All Posts in the database:");
                            int num = query.Count();


                            Console.WriteLine($"{num} Post(s) returned: ");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Content);
                            }
                        }
                        else
                        {
                            var query2 = db1.Posts.Where(p => p.BlogId == input1);
                            int num = query2.Count();


                            Console.WriteLine($"{num} Post(s) returned: ");
                            foreach (var item in query2)
                            {
                                Console.WriteLine(item.Content);
                            }
                        }


                    }
                    else if (input == "q")
                    {
                        cont = false;
                        logger.Info("Program ended");

                    }

                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            } while (cont);
        }
    }
}