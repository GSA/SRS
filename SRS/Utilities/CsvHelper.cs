using CsvHelper;
using CsvHelper.Configuration;
using SRS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Text;

namespace SRS.Utilities
{
    internal class SummaryFileGenerator
    {
        //Reference to logger
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly CultureInfo configuration;

        internal string GenerateSummaryFile<TClass, TMap>(string fileName, IEnumerable<TClass> summaryData)
            where TClass : class
            where TMap : ClassMap<TClass>
        {
            try
            {
                var summaryFileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss_FFFF") + ".csv";

                TextWriter summaryFile = File.CreateText(ConfigurationManager.AppSettings["SUMMARYFILEPATH"] + summaryFileName);
                 
                using (CsvWriter csvWriter = new CsvWriter(summaryFile, CultureInfo.CurrentCulture, false))
                {
                    csvWriter.Configuration.RegisterClassMap<TMap>();
                    csvWriter.WriteRecords(summaryData);
                }

                return summaryFileName;
            }
            catch (Exception ex)
            {
                _Log.Error("Error Writing Summary File: " + fileName + " - " + ex.Message + " - " + ex.InnerException);
                return string.Empty;
            }
        }
    }

    internal class FileReader
    {

        public List<TClass> GetFileData<TClass, TMap>(string filePath, out List<string> badRecords, ClassMap<Contractor> contractorMap = null)
           where TClass : class
           where TMap : ClassMap<TClass>
        {
            //fix errors in file before processing
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                var fileText = CsvFixer.FixRecord(new string(Encoding.UTF8.GetChars(buffer)));
                fs.SetLength(0);
                fs.Write(Encoding.UTF8.GetBytes(fileText), 0, fileText.Length);
                fs.Flush();
            }

            using (var sr = new StreamReader(filePath))
            {
                using (var CsvParser = new CsvParser(sr, CultureInfo.InvariantCulture))
                {
                    var csvReader = new CsvReader(CsvParser);
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.MissingFieldFound = null;
                    if (contractorMap != null)
                    {
                        csvReader.Configuration.RegisterClassMap(contractorMap);
                    }
                    else
                    {
                        csvReader.Configuration.RegisterClassMap<TMap>();
                    }
                    var good = new List<TClass>();
                    var bad = new List<string>();
                    var isRecordBad = false;
                    csvReader.Configuration.BadDataFound = context =>
                    {
                        isRecordBad = true;
                        bad.Add(context.RawRecord);
                    };

                    while (csvReader.Read())
                    {
                        var record = csvReader.GetRecord<TClass>();
                        if (!isRecordBad)
                        {
                            good.Add(record);
                        }

                        isRecordBad = false;
                    }
                    badRecords = bad;

                    return good;
                }
            }
        }
    } 
}
