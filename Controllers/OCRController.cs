using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleInference;

namespace FreeToolsRealtimeUse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
        private readonly PaddleOcrAll _ocr;

        public OCRController()
        {
            // Initialize PaddleOCR with English model and CPU device
            _ocr = new PaddleOcrAll(LocalFullModels.EnglishV3, PaddleDevice.Openblas());
        }

        [HttpPost("submit-card")]
        public IActionResult SubmitCard(IFormFile frontFile, IFormFile backFile)
        {
            if (frontFile == null || frontFile.Length == 0)
                return BadRequest("Front card image is required.");
            if (backFile == null || backFile.Length == 0)
                return BadRequest("Back card image is required.");

            // Load images
            Mat frontImg = LoadImageFromFormFile(frontFile);
            Mat backImg = LoadImageFromFormFile(backFile);

            // Run OCR
            var frontResult = _ocr.Run(frontImg);
            var backResult = _ocr.Run(backImg);

            // Join detected texts
       //     string frontText = string.Join(" ", frontResult.Regions.Select(r => r.Text)).ToUpper();
            string backText = string.Join(" ", backResult.Regions.Select(r => r.Text)).ToUpper();


            string frontText = frontResult.Text.ToUpper();

            // Normalize spaces &  lines
            //frontText = Regex.Replace(frontText, @"\s+", " ");
            backText = Regex.Replace(backText, @"\s+", " ");

            // Extract card details
            string cardNumberRaw = Regex.Match(frontText, @"\b\d{13,19}\b").Value;

            // Insert space every 4 digits
            string cardNumber = Regex.Replace(cardNumberRaw, ".{4}", "$0 ").Trim();

            var expiryMatch = Regex.Match(frontText, @"\b(0[1-9]|1[0-2])[\/\- ]?([0-9]{2,4})\b");
            string expiry = expiryMatch.Success ? expiryMatch.Value : "";

            // Allow 2 or 3 word names, ignore keywords like CARD, DEBIT
            var nameMatch = Regex.Match(frontText,
            @"^(?!CARD|DEBIT|VALID|EXPIRES|FUNDS|DCARD|THIS|TO|\$|\d)[A-Z]{2,}(?:\s[A-Z]{2,}){0,2}$",
            RegexOptions.Multiline);

            string cardHolderName = nameMatch.Success ? nameMatch.Value.Trim() : "";


            //var nameMatch = Regex.Match(frontText, @"\b(?!CARD|DEBIT|VALID|EXPIRES|FUNDS)[A-Z]{2,}\s+[A-Z]{2,}\b");
           // string cardHolderName = nameMatch.Success ? nameMatch.Value : "";


            string cvc = Regex.Match(backText, @"\b\d{3,4}\b").Value;




            return Ok(new
            {
                CardNumber = cardNumber,
                Expiry = expiry,
                CardHolderName = cardHolderName,
                CVC = cvc,
                FrontText = frontText,
                BackText = backText
            });
        }

        private Mat LoadImageFromFormFile(IFormFile file)
        {
            using var ms = new System.IO.MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            return Cv2.ImDecode(bytes, ImreadModes.Color);
        }
    }
}
