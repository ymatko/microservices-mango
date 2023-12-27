using System.Runtime.InteropServices.Marshalling;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public string ApiType { get; set; } = "GET";
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
