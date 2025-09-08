# FreeToolsRealtimeUse

PDF Generation API Tool

This is a .NET 8 Web API that allows you to convert web pages (URLs) into PDF documents using the wkhtmltopdf command-line tool. The API supports rendering HTML, CSS, JavaScript, and images, making it suitable for dynamic websites.

Features

Convert any public URL to PDF.

Supports pages with JavaScript content.

Handles local file access for CSS and images.

Returns PDF as a downloadable file.

Asynchronous and non-blocking execution.

Includes error handling and detailed output for troubleshooting.

Uses temporary files to avoid overwriting existing PDFs.

Technology Stack

.NET 8 / ASP.NET Core Web API

wkhtmltopdf (external command-line tool)

C# Process API for executing external processes

Async/await for efficient I/O

Installation

Clone the repository:

git clone https://github.com/pradeepveeramaneni/FreeToolsRealtimeUse


Install wkhtmltopdf:

Download from https://wkhtmltopdf.org/downloads.html

Place wkhtmltopdf.exe in the wkhtmltopdf folder inside the project directory.

Build the project using Visual Studio or dotnet build.

Notes

Ensure wkhtmltopdf.exe is in a writable folder and the API process has permissions to execute it.

For complex pages, increase --javascript-delay to allow scripts to finish rendering.

This tool does not use third-party NuGet packages like NReco; it relies directly on wkhtmltopdf.exe.


# Paddle OCR API Integration

This project provides a .NET 8 Web API to extract card information (card number, expiry, cardholder name, CVC) from front and back images using PaddleOCR and OpenCvSharp.

Features

Upload front and back images of cards via API.

Extract structured information: Card Number, Expiry, Card Holder Name, CVC.

Normalize and format text for reliable processing.

Uses regex-based extraction for robust data parsing.

Returns JSON response with OCR results.

Technology Stack

.NET 8 / ASP.NET Core Web API

PaddleOCR (Sdcb.PaddleOCR, Sdcb.PaddleOCR.Models.Local)

Paddle Inference Runtime (Sdcb.PaddleInference)

OpenCvSharp (OpenCvSharp4, OpenCvSharp4.runtime.win)

Swagger / OpenAPI (Swashbuckle.AspNetCore)

C# Regex for parsing card details

Setup / Installation

Clone the repository

git clone https://github.com/pradeepveeramaneni/FreeToolsRealtimeUse
cd FreeToolsRealtimeUse


Install necessary NuGet packages

Run the following commands in your project directory:

dotnet add package OpenCvSharp4 --version 4.11.0.20250507
dotnet add package OpenCvSharp4.runtime.win --version 4.11.0.20250507
dotnet add package Sdcb.PaddleInference --version 3.0.1
dotnet add package Sdcb.PaddleInference.runtime.win64.openblas --version 3.1.0.54

OR use MKL runtime if preferred:
dotnet add package Sdcb.PaddleInference.runtime.win64.mkl --version 3.1.0.54
dotnet add package Sdcb.PaddleOCR --version 3.0.1
dotnet add package Sdcb.PaddleOCR.Models.Local --version 3.0.1
dotnet add package Swashbuckle.AspNetCore --version 6.6.2


⚠ Note: Only one Paddle Inference runtime is needed (openblas or mkl). OpenBLAS is fully open source.

Build the project

dotnet build


Run the API

dotnet run


The API will start on https://localhost:5001 (or your configured port).

Swagger UI is available at https://localhost:5001/swagger/index.html for testing endpoints.

Usage

Endpoint: POST /api/OCR/submit-card

Parameters: IFormFile frontFile, IFormFile backFile

Response Example:

{
  "CardNumber": " ",
  "Expiry": " ",
  "CardHolderName": " ", need regix modification
  "CVC": " ",
  "FrontText": "...",
  "BackText": "..."
}

Notes

PaddleOCR English models are included via Sdcb.PaddleOCR.Models.Local.

The OCR runs on CPU by default (OpenBLAS), GPU support is available with CUDA.

Ensure uploaded images are clear for accurate OCR results.

Regex parsing may need adjustments for unusual card formats.
