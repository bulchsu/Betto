using System.Collections.Generic;

namespace Betto.RapidApiCommunication.Parsers
{
    public interface IParser<out T> where T : class
    {
        IEnumerable<T> Parse(string rawJson);
    }
}
