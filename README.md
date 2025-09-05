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

License

This project is open-source under the MIT License
