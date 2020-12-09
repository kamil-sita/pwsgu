using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

/// <summary>
/// Represents a single row in CSV (single part of experiment)
/// </summary>
public class FittsData {

   public FittsData(float area, float amplitude, float time)
    {
        this.Area = area;
        this.Amplitude = amplitude;
        this.Time = time;
    }
    public float Area { get; set; }
    public float Amplitude { get; set; }
    public float Time { get; set; }
}

/// <summary>
/// Class responsible for reading CSV data
/// </summary>
public static class ReadCSV
{
   public static List<FittsData> ReadData()
   {
       using (var reader = new StreamReader(Application.streamingAssetsPath + "\\fittsData.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {

            return csv.GetRecords<FittsData>().ToList();

        }
   }
}
