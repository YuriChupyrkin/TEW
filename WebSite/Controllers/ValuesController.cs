using System.Collections.Generic;
using System.Web.Http;

namespace TewCloud.Controllers
{
  public class ValuesController : ApiController
  {
    // GET api/values
    public IEnumerable<string> Get(int length)
    {
      return new string[length];
    }
  }
}
