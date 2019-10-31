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
                        int num = query.Count();


                        Console.WriteLine($"{num} blogs in the database:");
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
                    else if (input == 4)
                    {
                        //display Post
                        var db = new BloggingContext();

                        var query = db.Posts.OrderBy(p => p.Content);

                    }
                    else if (input == 'q')
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