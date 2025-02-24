using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Primitives;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.DocIORenderer;
using System.Data;
using Syncfusion.Pdf;
using GYM_BE.Core.Dto;
using System.ComponentModel;

namespace GYM_BE.Core.Extentions
{
    public static class HttpRequestExtensions
    {
        public static string? Token(this HttpRequest request)
        {
            foreach (KeyValuePair<string, StringValues> header in request.Headers)
            {
                if (header.Key == "Authorization")
                {
                    try
                    {
                        return header.Value.ToString().Split(" ")[1];
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }
        public static DataTable ToDataTable<T>(IQueryable<T> query)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in query)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public static byte[] FillTemplatePDF(string templatePath, DataSet dataSet)
        {
            if(dataSet == null || dataSet.Tables.Count == 0)
            {
                return Array.Empty<byte>();
            }

            string outputPath = Path.Combine(Path.GetTempPath(), "Output.docx");

            // Tạo một tài liệu Word mới từ file mẫu
            File.Copy(templatePath, outputPath, true);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                // Lấy nội dung từ file mẫu
                var body = wordDoc.MainDocumentPart!.Document.Body;
                var templateContent = body!.InnerXml;

                // Xóa nội dung hiện tại trong tài liệu mới
                body.RemoveAllChildren();

                foreach(DataTable table in dataSet.Tables)
                {
                    if(table.TableName == "DATA") {
                        // Lặp qua các hàng của DataTable và thêm nội dung vào tài liệu
                        foreach (DataRow row in table.Rows)
                        {
                            string filledContent = templateContent;
                            // Thay thế các merge fields trong nội dung mẫu
                            foreach (DataColumn column in table.Columns)
                            {
                                string placeholder = $"[{column.ColumnName}]";
                                string replacement = row[column.ColumnName].ToString();
                                filledContent = filledContent.Replace(placeholder, replacement);
                            }
                            // Thêm nội dung đã được thay thế vào tài liệu
                            body.InnerXml += filledContent;

                            // Thêm ngắt trang để mỗi hàng trong DataTable sẽ xuất ra một trang riêng
                            body.Append(new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = BreakValues.Page })));
                        }
                    }
                    
                }
                wordDoc.MainDocumentPart.Document.Save();
            }

            // Chuyển đổi tài liệu Word đã được điền dữ liệu sang PDF
            using (FileStream inputFileStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Load the Word document
                WordDocument document = new WordDocument(inputFileStream, FormatType.Docx);

                // Initialize the DocIORenderer for Word to PDF conversion
                DocIORenderer render = new DocIORenderer();
                using (PdfDocument pdfDocument = render.ConvertToPDF(document))
                {
                    // Lưu tài liệu PDF vào bộ nhớ đệm
                    using (MemoryStream pdfStream = new MemoryStream())
                    {
                        pdfDocument.Save(pdfStream);
                        pdfStream.Position = 0;

                        // Trả file PDF về dưới dạng kết quả của API
                        return pdfStream.ToArray();
                    }
                }
            }
        }

    }

    
}
