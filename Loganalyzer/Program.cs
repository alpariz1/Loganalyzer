 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class LogAnalyzer
{
    static void Main(string[] args)
    {
        string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, "logs.txt"); // Log dosyasının tam yolu

        Console.WriteLine("Program başlatıldı.");

        try
        {
            Console.WriteLine("Dosyanın varlığı kontrol ediliyor: " + filePath);

            // Dosyanın var olup olmadığını kontrol et
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Belirtilen dosya bulunamadı.", filePath);
            }

            Console.WriteLine("Dosya bulundu, içerik okunuyor.");

            // Log dosyasını oku
            var logLines = File.ReadAllLines(filePath);

            Console.WriteLine("Dosya okundu, anahtar kelime aranıyor.");

            string keyword = "aslanbey"; // Aranacak anahtar kelime
            var keywordLines = logLines.Where(line => line.Contains(keyword)).ToList();

            if (keywordLines.Any())
            {
                Console.WriteLine($"Anahtar kelime '{keyword}' içeren satırlar:");

                foreach (var line in keywordLines)
                {
                    Console.WriteLine(line);
                }

                // Log satırlarını tarihe göre sırala
                var sortedLines = keywordLines.OrderBy(line => ParseDateFromLogLine(line)).ToList();

                Console.WriteLine("\nTarihe göre sıralanmış satırlar:");

                foreach (var line in sortedLines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine($"Anahtar kelime '{keyword}' içeren satır bulunamadı.");
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Dosya hatası: {ex.Message}");
            Console.WriteLine($"Dosya yolu: {ex.FileName}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Tarih formatı hatası: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel hata: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Program sonlandı.");
        }
    }

    static DateTime ParseDateFromLogLine(string logLine)
    {
        // Log satırındaki tarih ve saat bilgisini çıkar
        string dateString = logLine.Substring(11, 10); // "2024-06-07" formatında tarih
        string timeString = logLine.Substring(27, 8); // "19:15:48" formatında saat

        string dateTimeString = $"{dateString} {timeString}"; // "2024-06-07 19:15:48" formatında

        if (DateTime.TryParse(dateTimeString, out DateTime logDateTime))
        {
            return logDateTime;
        }

        throw new FormatException("Geçersiz tarih formatı: " + dateTimeString);
    }
}

