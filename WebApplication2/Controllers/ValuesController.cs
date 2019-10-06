using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace WebApplication2.Controllers
{

  public class Author
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }

  public class Blog
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public Author Author { get; set; }
    public ICollection<Post> Posts { get; set; }

    public Blog(Guid id, string name, DateTime createdOn, Author author, ICollection<Post> posts = null)
    {
      Id = id;
      Name = name;
      CreatedOn = createdOn;
      Author = author;
      Posts = posts ?? new List<Post>();
    }
  }

  public class Post
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOn { get; set; }
  }

  public class CreateBlogRequest
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public Author Author { get; set; }
  }
  
  [Route("api/[controller]")]
  [ApiController]
  public class BlogController : ControllerBase
  {
    private readonly IMongoCollection<Blog> _collection;

    public BlogController()
    {
      var client = new MongoClient("mongodb://localhost:27017");
      var database = client.GetDatabase("blog");
      _collection = database.GetCollection<Blog>("blog");
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      return new string[] {"value1", "value2"};
    }
    
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
      return "value";
    }
    
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateBlogRequest blogRequest)
    {
      if (blogRequest.CreatedOn < DateTime.Now.AddYears(-1))
      {
        return BadRequest();
      };
      
      var blog = new Blog(
        blogRequest.Id,
        blogRequest.Name,
        blogRequest.CreatedOn,
        blogRequest.Author
      );

      await _collection.InsertOneAsync(blog);
      return Ok();
    }
    
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}