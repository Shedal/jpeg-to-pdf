using System;
using org.pdfclown.documents;
using org.pdfclown.documents.contents.composition;
using org.pdfclown.documents.contents.entities;
using org.pdfclown.documents.interaction.viewer;
using org.pdfclown.documents.interchange.metadata;
using org.pdfclown.files;

namespace jpeg_to_pdf
{
  public class PdfManager
  {
    public PdfManager()
    {
    }

    public void SavePdf(string imagePath)
    {
      var file = new File();
      Document document = file.Document;

      var page = new Page(document);
      document.Pages.Add(page);

      var composer = new PrimitiveComposer(page);

      composer.ApplyMatrix(200, 0, 0, 200, (page.Size.Width - 200)/2, (page.Size.Height - 200)/2);

      Image image = Image.Get(imagePath);
      image.ToInlineObject(composer);

      composer.Flush();

      Serialize(file, "Inline image", "embedding an image within a content stream", "inline image");
    }

    private void ApplyDocumentSettings(
      Document document,
      string title,
      string subject,
      string keywords
      )
    {
      if (title == null)
        return;

      // Viewer preferences.
      var view = new ViewerPreferences(document);
      // Instantiates viewer preferences inside the document context.
      document.ViewerPreferences = view; // Assigns the viewer preferences object to the viewer preferences function.
      view.DisplayDocTitle = true;

      // Document metadata.
      Information info = document.Information;
      info.Clear();
      info.Author = "Karina Grekova";
      info.CreationDate = DateTime.Now;
      info.Creator = GetType().FullName;
      info.Title = "PDF Clown - " + title + " sample";
      info.Subject = "Sample about " + subject + " using PDF Clown";
      info.Keywords = keywords;
    }

    public string Serialize(
      File file,
      string title,
      string subject,
      string keywords
      )
    {
      return Serialize(file, null, title, subject, keywords);
    }

    public string Serialize(
      File file,
      SerializationModeEnum? serializationMode,
      string title,
      string subject,
      string keywords
      )
    {
      return Serialize(file, GetType().Name, serializationMode, title, subject, keywords);
    }

    public string Serialize(
      File file,
      string fileName,
      SerializationModeEnum? serializationMode,
      string title,
      string subject,
      string keywords
      )
    {
      ApplyDocumentSettings(file.Document, title, subject, keywords);

      Console.WriteLine();

      if (!serializationMode.HasValue)
      {
        if (file.Reader == null) // New file.
        {
          serializationMode = SerializationModeEnum.Standard;
        }
        else // Existing file.
        {
          Console.WriteLine("[0] Standard serialization");
          Console.WriteLine("[1] Incremental update");
          Console.Write("Please select a serialization mode: ");
          try
          {
            serializationMode = (SerializationModeEnum) int.Parse(Console.ReadLine());
          }
          catch
          {
            serializationMode = SerializationModeEnum.Standard;
          }
        }
      }

      string outputFilePath = @"C:\tmp\" + fileName + "." + serializationMode + ".pdf";

      try
      {
        file.Save(outputFilePath, serializationMode.Value);
      }
      catch (Exception e)
      {
        Console.WriteLine("File writing failed: " + e.Message);
        Console.WriteLine(e.StackTrace);
      }
      Console.WriteLine("\nOutput: " + outputFilePath);

      return outputFilePath;
    }
  }
}