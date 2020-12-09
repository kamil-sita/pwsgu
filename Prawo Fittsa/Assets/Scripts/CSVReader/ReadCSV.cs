using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
public class FittsData {
    /// <summary>
    /// Store csv bean data
    /// </summary>
    /// <param name="area"></param>
    /// <param name="amplitude"></param>
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
    /// <summary>
    /// Read csv from file and return list of beans
    /// </summary>
    /// <returns></returns>
   public static List<FittsData> ReadData()
   {
       using (var reader = new StreamReader(Application.streamingAssetsPath + "\\fittsData.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {

            return csv.GetRecords<FittsData>().ToList();

        }
   }
}
