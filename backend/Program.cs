using System.Text;
using System.Xml;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseRouting();
app.UseAuthorization();

// API Endpoints
app.MapPost("/api/xml/validate", async (HttpContext context) =>
{
    try
    {
        var request = await context.Request.ReadFromJsonAsync<XmlValidationRequest>();
        if (request?.XmlContent == null)
        {
            return Results.BadRequest(new { isValid = false, message = "XML content is required" });
        }

        var isValid = ValidateXml(request.XmlContent, out string errorMessage);
        
        return Results.Ok(new 
        { 
            isValid, 
            message = isValid ? "XML is valid" : errorMessage 
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { isValid = false, message = $"Error: {ex.Message}" });
    }
});

app.MapPost("/api/xml/format", async (HttpContext context) =>
{
    try
    {
        var request = await context.Request.ReadFromJsonAsync<XmlFormatRequest>();
        if (request?.XmlContent == null)
        {
            return Results.BadRequest(new { success = false, message = "XML content is required" });
        }

        var formattedXml = FormatXml(request.XmlContent, out string errorMessage);
        
        if (formattedXml == null)
        {
            return Results.BadRequest(new { success = false, message = errorMessage });
        }

        return Results.Ok(new 
        { 
            success = true, 
            formattedXml,
            message = "XML formatted successfully" 
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, message = $"Error: {ex.Message}" });
    }
});

app.MapPost("/api/xml/parse", async (HttpContext context) =>
{
    try
    {
        var request = await context.Request.ReadFromJsonAsync<XmlParseRequest>();
        if (request?.XmlContent == null)
        {
            return Results.BadRequest(new { success = false, message = "XML content is required" });
        }

        var treeStructure = ParseXmlToTree(request.XmlContent, out string errorMessage);
        
        if (treeStructure == null)
        {
            return Results.BadRequest(new { success = false, message = errorMessage });
        }

        return Results.Ok(new 
        { 
            success = true, 
            treeStructure,
            message = "XML parsed successfully" 
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, message = $"Error: {ex.Message}" });
    }
});

app.MapPost("/api/xml/save", async (HttpContext context) =>
{
    try
    {
        var request = await context.Request.ReadFromJsonAsync<XmlSaveRequest>();
        if (request?.XmlContent == null || string.IsNullOrEmpty(request.FileName))
        {
            return Results.BadRequest(new { success = false, message = "XML content and filename are required" });
        }

        // Create a directory for saved files if it doesn't exist
        var saveDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var filePath = Path.Combine(saveDirectory, request.FileName);
        await File.WriteAllTextAsync(filePath, request.XmlContent, Encoding.UTF8);
        
        return Results.Ok(new 
        { 
            success = true, 
            message = $"File saved successfully: {request.FileName}",
            filePath = filePath
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, message = $"Error saving file: {ex.Message}" });
    }
});

app.MapGet("/api/xml/files", async (HttpContext context) =>
{
    try
    {
        var saveDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles");
        if (!Directory.Exists(saveDirectory))
        {
            return Results.Ok(new { files = new string[0] });
        }

        var files = Directory.GetFiles(saveDirectory, "*.xml")
            .Select(Path.GetFileName)
            .ToArray();
        
        return Results.Ok(new { files });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, message = $"Error retrieving files: {ex.Message}" });
    }
});

app.MapGet("/api/xml/load/{fileName}", async (HttpContext context) =>
{
    try
    {
        var fileName = context.Request.RouteValues["fileName"]?.ToString();
        if (string.IsNullOrEmpty(fileName))
        {
            return Results.BadRequest(new { success = false, message = "Filename is required" });
        }

        var saveDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles");
        var filePath = Path.Combine(saveDirectory, fileName);
        
        if (!File.Exists(filePath))
        {
            return Results.NotFound(new { success = false, message = "File not found" });
        }

        var content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        
        return Results.Ok(new 
        { 
            success = true, 
            xmlContent = content,
            fileName = fileName
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, message = $"Error loading file: {ex.Message}" });
    }
});

app.Run();

// Helper methods
static bool ValidateXml(string xmlContent, out string errorMessage)
{
    try
    {
        var document = new XmlDocument();
        document.LoadXml(xmlContent);
        errorMessage = string.Empty;
        return true;
    }
    catch (XmlException ex)
    {
        errorMessage = $"XML Error: {ex.Message}";
        return false;
    }
    catch (Exception ex)
    {
        errorMessage = $"Error: {ex.Message}";
        return false;
    }
}

static string? FormatXml(string xmlContent, out string errorMessage)
{
    try
    {
        var document = XDocument.Parse(xmlContent);
        var formattedXml = document.ToString();
        errorMessage = string.Empty;
        return formattedXml;
    }
    catch (XmlException ex)
    {
        errorMessage = $"XML Error: {ex.Message}";
        return null;
    }
    catch (Exception ex)
    {
        errorMessage = $"Error: {ex.Message}";
        return null;
    }
}

static object? ParseXmlToTree(string xmlContent, out string errorMessage)
{
    try
    {
        var document = XDocument.Parse(xmlContent);
        var tree = ParseElementToTree(document.Root, "root");
        errorMessage = string.Empty;
        return tree;
    }
    catch (XmlException ex)
    {
        errorMessage = $"XML Error: {ex.Message}";
        return null;
    }
    catch (Exception ex)
    {
        errorMessage = $"Error: {ex.Message}";
        return null;
    }
}

static object ParseElementToTree(XElement element, string path)
{
    var result = new
    {
        id = path,
        name = element.Name.LocalName,
        type = 1, // Element node
        value = "",
        attributes = element.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value),
        children = element.Elements().Select((child, index) => ParseElementToTree(child, $"{path}.{index}")).ToArray(),
        textContent = element.Nodes().OfType<XText>().FirstOrDefault()?.Value?.Trim() ?? ""
    };

    return result;
}

// Request models
public record XmlValidationRequest(string XmlContent);
public record XmlFormatRequest(string XmlContent);
public record XmlParseRequest(string XmlContent);
public record XmlSaveRequest(string XmlContent, string FileName);
