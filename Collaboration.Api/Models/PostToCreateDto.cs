﻿using System;

namespace Collaboration.Api.Models
{
    public class PostToCreateDto
    {
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public int ThreadId { get; set; }
    }
}