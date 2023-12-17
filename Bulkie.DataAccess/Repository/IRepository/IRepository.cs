using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.DataAccess.Repository.IRepository
{
    /* Repository mediates between the domain and data mapping layers,
     * acting like an in-memory collection of domain objects
     
       BENEFITS
       -- It minimizes duplicate query logic
       -- Decouples your application from persistence frameworks e.g Entity Framework
    
       WHAT IS A PERSISTENT FRAMEWORK?
       -- A persistent framework is middleware that assists in the storage and retrieval
          of information between applications and databases, especially relational databases. 
          It acts as a layer of abstraction for persisted data, bridging conceptual and technical
          differences between storage and utilization.

       There has been persistent frameworks (O/RMs being released every 2 years) such as:
       1. ADO.NET                                         8. Dapper
       2. LINQ to SQL                                     9. ORMLite
       3. Entity Framework v1                            10. PetaPocos
       4. nHibernate
       5. Entity Framework v4
       6. Entity Framework v4.1: DbContext
       7. Entity Framework v7 (a complete re-write)

       Frameworks are constantly changing and evolving, and if you want to have the freedom to explore 
       a different persistence framework with minimal impact in your application, that's when you should 
       use the repository pattern.

      -- Promotes testability (make it easier to unit test your application, but that's partially true 
         if you are using an older version of Entity Framework)
    
       In a nutshell, the repository should look like: The repository should act like the collection of objects in memory.
        Add()
        Remove()
        Get(id)
        GetAll()
        Find(predicate)

       Note that we do not have update in in-memory objects because we simply get it from the collection and change it.
       E.g. var course = collection.Get(1);
            course.Name = "New Name";

       N.B. The repository should not have the semantics of the Database. It shoudn't have methods like update and save
      
       A lot of people asks themselves the question 'If the repository acts as a collection of objects in memory, how are 
       we going to save these objects and changes them to the database? That's when the unit of work pattern comes into the picture.

       Unit of Work
       -- Maintains a list of objects affected by a business transaction and coordinates the writing out of changes.

      Clean Architecture
      -- The architecture should be independent of frameworks
      This allows you to use such frameworks as tools rather than having to cram your system into their limited constraints.
      So that is why using patterns like the repository pattern help you decouple from frameworks such as entity framework.

      What is an IEnumerable in C#?
      -- Think of IEnumerable as the bread and butter of collection traversal in C#. 
      -- It is an interface provided by .NET, IEnumerable allows backward and forward traversal of a collection in the most
         hassle-free manner. Below is the interface of IEnumerable in C#:

         public interface IEnumerable
         {
            IEnumerator GetEnumerator();
         }

      How does IEnumerable work in C#?
      IEnumerable basically works by returning an IEnumerator which provides the ability to iterate through the collection by
      exposing a Current property and MoveNext and Reset methods. 

      Benefits of Using IEnumerable in C#
      www.bytehide.com/blog/ienumerable-csharp

      1. Improve Performance and Efficiency with IEnumerable
         -- When using an IEnumerable, the data is not loaded until it is enumerated. This phenomenon, known as deffered loading,
            lends a serious lift to performance and efficiency. E.g.

            // Define some data
            int[] years = {2001, 2002, 2003, 2004, 2005}

            // Create an IEnumerable where each number is raised to the power of 3
            IEnumerable<int> cubeQuery = years.Select(y =>y*y*y);

            // Now let's enumerate our data and see deffered execution in action
            foreach(int yearCube in cubeQuery)
            {
               Console.WriteLine(yearCube);
            }

        In the above example, our years array isn't actually transformed until we start looping over cubeQuery.
        The actual computations aren't performed until they're required in the foreach loop. By deferring execution,
        unnecessary evaluations are avoided, contributing to a leaner, more efficient program. 

      2. Flexibility in Data Manipulation with IEnumerable
         -- Variables are like clay in the hands of a craftsman, aren't they? And nothing makes them quite so malleable
            as IEnumerable. This interface is unbelievably versatile, enabling operations over a variety of data collections 
            without knowing their specific types.

            If you were explaining this concept to a non-programmer, imagine trying to explain to an eight-year-old why their
            favourite toy transformer is so cool. "You see, it can be a car, a robot, or anything you want it to be!" Similarly,
            whether you're working with an array, list, dictionary, set, or any other collection, IEnumerable can help you traverse
            through them all with ease.

            var shapes = new List<object> { new {Figure = "Circle", Radius = 5},
                                            new {Figure = "Square", Side = 4},
                                            new {Figure = "Rectangle", Width = 5, Height = 10}};

            foreach(var shape in shapes)
            {
                Console.WriteLine(shape);
            }

       In this example, we've got a list of objects of different shapes and sizes, literally! Though these objects vary, IEnumerable 
       lets us loop through them with the same ease of a walk in the park.

      3. Better Memory Management with IEnumerable
         -- When dealing with large data sets, the memory can easily get overloaded, taking a severe toll on your program's performance.
            With IEnumerable, it's like having a personal memory manager for your data. It's 'fetch-on-demand' approach ensures data is 
            only retrieved as and when required, thus preventing wastage of memory. Out-of-memory? Thanks to IEnumerable, they are in danger 
            of becoming extinct!

            var hugeDataset = Enumerable.Range(0,1_000_000);
            IEnumerable<int> smallerDataset = hugeDataset.Where(x => x >500_000);

            foreach(int data in smallerDataset)
            {
                Console.WriteLine(data);
            }

            In this scenario, suppose we're dealing with a massive data set of one million numbers(yeap, you heard that right). However, we're
            only interested in numbers greater than 500,000. Thanks to IEnumerable even dealing with such massive datasets won't cause your system 
            to beg for mercy. Only the smaller, filtered pieces of data are loaded when needed; the rest of the million numbers sit back, relaxing
            in storage.

    THINGS TO LEARN
    1. Linq
    2. Lambda
    3. Data Structures
    4. OOP Classes and Interface implementation and use cases
    5. Design Patterns
    6. Implicit and explicit type casting.

     */

    // When you implement the generic interface, we do not know what the class type will be, so it will be a generic class T, where T will be a class.
    // 
    public interface IRepository<T> where T : class 
    {
        /*When the IRepository will be implemented, at that time we will know on what class the implementation will be. Because right now,
          we have Category on which we want to implement repository. We will have Product, orderdetail and because of that we are making this 
          generic here.

          Then we have to think about all of the methods that we want right now

          T - Category or any other generic model on which we want to perform the CRUD operation or rather we want to interact with the DBContext

        */ 
        IEnumerable<T> GetAll(); // we need to retrieve all the category(or any other model) where we are displaying all of them.
        
        // The reason we will not use Find is because that only works on the id. But if you want some other condition to get on record, you can pass that
        // using Linq operator. Basically in parameter we will be getting a Linq operation like this -- (u => u.Id==id). So for that we pass as parameter
        // 'Expression<Func<T, bool>>'.
        T GetFirstOrDefault(Expression<Func<T,bool>> filter); // what we will be getting is a function, and the out result will be a boolean. We can call
                                                              // We can call that as filter when we are fetching an individual record.

        void add(T entity);
        //  update is a bit complicated because when you are updating a Category, logic might be different to when you are updating a Product
        // Sometimes you want to update few properties or you have some other logic.
        // void update(T entity);
        void Remove(T entity);
        void removeRange(IEnumerable<T> entity);
                                

       
    }
}
