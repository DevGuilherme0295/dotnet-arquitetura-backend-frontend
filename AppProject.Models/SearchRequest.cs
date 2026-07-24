using System;

namespace AppProject.Models;

public class SearchRequest : IRequest
{
    public int? Take { get; set; }

    public string? SerchText { get; set; }
}
