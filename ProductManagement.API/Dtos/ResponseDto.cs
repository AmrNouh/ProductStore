using System.Collections.Generic;

namespace ProductManagement.API.Dtos
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public bool IsSucceed { get; set; }
        public List<string> Messages { get; set; }
    }
}
