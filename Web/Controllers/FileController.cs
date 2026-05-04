using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Interfaces.Services.Files;
using HealthInvoice.Core.Dtos.Files;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/file")]
public class FileController(
    IFileStorageService storageService) : ControllerBase
{
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveFilesAsync([FromBody] FilesRemoveRequest request)
    {
        //foreach (var fileMeta in request.FilesToRemove)
        //{
        //    if (string.IsNullOrEmpty(fileMeta.FilePath))
        //        return BadRequest(
        //            $"Ошибка при попытке удалить файлы. Путь для файла {fileMeta.Filename ?? "NONE"} не был передан. Обратитесь к разработчику.");

        //    await storageService.RemoveFileAsync(fileMeta.FilePath);
        //}

        return Ok();
    }
}
