using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.CycleCounts;

public class UpdateCycleCountDetailFileDto
{
    [Required]
    public Guid CycleCountId { get; set; }
    [Required]
    public IFormFile File { get; set; }
}