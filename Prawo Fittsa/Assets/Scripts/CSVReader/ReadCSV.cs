using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
public class FittsData {

   public FittsData(float area, float amplitude)
    {
        this.Area = area;
        this.Amplitude = amplitude;
    }
    public float Area { get; set; }
    public float Amplitude { get; set; }
}
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
