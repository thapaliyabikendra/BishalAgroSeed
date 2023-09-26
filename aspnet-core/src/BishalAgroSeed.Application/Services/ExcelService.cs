using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace BishalAgroSeed.Services;
[RemoteService(false)]
public class ExcelService : IExcelService, ITransientDependency
{
    private readonly ILogger<ExcelService> _logger;
    public ExcelService(ILogger<ExcelService> logger)
    {
        _logger = logger;
    }
        
    public async Task<byte[]> ExportAsync<TData>(List<TData> data,
        Dictionary<string, Func<TData, object>> mappers,
        string sheetName = "Sheet1")
    {
        try
        {
            _logger.LogInformation($"ExcelService.ExportAsync - Started");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "BishalAgroSeed";
            p.Workbook.Worksheets.Add(sheetName);
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            foreach (var item in data)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, data.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            _logger.LogInformation($"ExcelService.ExportAsync - Ended");
            return await p.GetAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"ExcelService.ExportAsync - Exception : {ex.ToString()}");
            throw;
        }
    }
}
