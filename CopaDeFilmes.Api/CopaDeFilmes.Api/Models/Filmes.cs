﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CopaDeFilmes.Api.Models
{
    public class Filmes
    {
        public string id { get; set; }
        public string titulo { get; set; }
        public int ano { get; set; }
        public float nota { get; set; }
        public string erro { get; set; }
    }
}