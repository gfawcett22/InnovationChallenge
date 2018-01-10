﻿using System.Collections.Generic;

namespace Collaboration.Api.Models
{
    public class ThreadToCreateDto
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }
        IEnumerable<PostToCreateDto> Posts { get; set; }
    }
}