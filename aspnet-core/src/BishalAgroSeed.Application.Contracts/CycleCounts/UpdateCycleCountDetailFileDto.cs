using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BishalAgroSeed.CycleCounts;

public class UpdateCycleCountDetailFileDto
{
    public UpdateCycleCountDetailFileDto()
    {
    }

    public UpdateCycleCountDetailFileDto(Guid cycleCountId, IFormFile file)
    {
        CycleCountId = cycleCountId;
        File = file;
    }

    [Required]
    public Guid CycleCountId { get; set; }
    [Required]
    public IFormFile File { get; set; }
}