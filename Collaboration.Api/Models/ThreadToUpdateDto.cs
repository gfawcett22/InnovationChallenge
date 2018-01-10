using System.Collections.Generic;

namespace Collaboration.Api.Models
{
    public class ThreadToUpdateDto
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }
        IEnumerable<PostToUpdateDto> Posts { get; set; }
    }
}