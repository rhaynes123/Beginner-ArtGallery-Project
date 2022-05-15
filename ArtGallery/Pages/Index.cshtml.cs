using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    [BindProperty]
    public Image ImageToUpload { get; set; }
    public List<Image> Gallery { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> OnGet()
    {
        Gallery = await _dbContext.Images.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,"Model In Invalid State");
            return Page();
        }
        if(ImageToUpload.FormFile is null)
        {
            ModelState.AddModelError(string.Empty, "File Name not Found");
            return Page();
        }
        try
        {
            string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string filename = $"{ImageToUpload.Guid}_{ImageToUpload.FormFile.FileName}";
            string filePath = Path.Combine(imageFolder, filename);
            ImageToUpload.FilePath = filename;
            if(!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    await ImageToUpload.FormFile.CopyToAsync(stream);
                }
                await _dbContext.Images.AddAsync(ImageToUpload);
                await _dbContext.SaveChangesAsync();
                Gallery = await _dbContext.Images.ToListAsync();
                return Page();
            }
            ModelState.AddModelError(string.Empty, "File Path Exists");
            return Page();

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            return RedirectToPage("Error");
        }
    }
}
