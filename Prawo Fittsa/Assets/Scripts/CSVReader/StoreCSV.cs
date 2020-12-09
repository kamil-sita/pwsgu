using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using CsvHelper;
using UnityEngine;

public class StoreCSV 
{
    public static void saveCSV(List<float> areas, List<float> amplitudes, List<float> times, List<float> iods)
    /// Save list of areas and ampitudes to fittsData file
    /// </summary>
    /// <param name="areas"></param>
    /// <param name="amplitudes"></param>
    {
        var fittsList = new List<FittsData>();
        for (var i = 0; i < areas.Count; ++i)
        {
            if (amplitudes.Count < i) continue;
            var fittsElement = new FittsData(areas[i],amplitudes[i], times[i], iods[i]);
            fittsList.Add(fittsElement);
        }

        using (var writer = new StreamWriter(Application.streamingAssetsPath + "\\fittsData.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InstalledUICulture)) {

            csv.WriteRecords(fittsList);
        }
    }
}
