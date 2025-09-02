using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NReco.PdfGenerator;
using System.Diagnostics;

namespace FreeToolsRealtimeUse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        [HttpGet("GeneratePdf")]
        public async Task<IActionResult> GeneratePdf()
        {
            string url = "https://leaddev.com/career-development";
            string wkhtmlExePath = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltopdf", "wkhtmltopdf.exe");
            if (!System.IO.File.Exists(wkhtmlExePath))
                return BadRequest(new { error = "wkhtmltopdf.exe not found at: " + wkhtmlExePath });

            string tempPdfPath = Path.Combine(Path.GetTempPath(), $"output_{Guid.NewGuid()}.pdf");

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = wkhtmlExePath,
                    Arguments = $"--enable-local-file-access --javascript-delay 3000 --no-stop-slow-scripts \"{url}\" \"{tempPdfPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                await process.WaitForExitAsync();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                if (process.ExitCode != 0)
                {
                    return BadRequest(new
                    {
                        errorMessage = "wkhtmltopdf failed",
                        exitCode = process.ExitCode,
                        output,
                        errorOutput = error
                    });
                }

                byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(tempPdfPath);
                System.IO.File.Delete(tempPdfPath);
                return File(pdfBytes, "application/pdf", "output.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("TestExe")]
        public IActionResult TestExe()
        {
            try
            {
                var exePath = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltopdf", "wkhtmltopdf.exe");
                if (!System.IO.File.Exists(exePath))
                    return BadRequest("EXE not found at: " + exePath);

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = "https://www.google.com test.pdf",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltopdf")
                };

                var process = System.Diagnostics.Process.Start(psi);
                process.WaitForExit();

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                return Ok(new { exitCode = process.ExitCode, output, error });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

       


    }
}

