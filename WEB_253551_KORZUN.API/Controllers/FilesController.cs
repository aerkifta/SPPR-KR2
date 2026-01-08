using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace WEB_253551_KORZUN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IWebHostEnvironment environment, ILogger<FilesController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            _logger.LogInformation("Загрузка файла: {FileName}", file?.FileName);

            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не выбран");
            }

            try
            {
                var imagesPath = Path.Combine(_environment.WebRootPath, "Images");

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                var extension = Path.GetExtension(file.FileName);
                var fileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                var filePath = Path.Combine(imagesPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("Файл сохранен: {FilePath}", filePath);

                var fileUrl = $"{Request.Scheme}://{Request.Host}/Images/{fileName}";
                return Ok(fileUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении файла");
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{fileName}")]
        public IActionResult Delete(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound($"Файл {fileName} не найден");
                }

                System.IO.File.Delete(filePath);
                _logger.LogInformation("Файл удален: {FilePath}", filePath);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении файла");
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}