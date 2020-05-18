/*
 * File: DataModeler.cs
 * Descrption: Contains methods to parse JSON, XML, and CSV files to get the information
 *             for all cities stored within the file
 * Date: 2020-02-03
 * 
 */
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace ProjectLibrary
{
    public class DataModeler
    {
        // Data members
        Dictionary<string, CityInfo> catalogue;
        private delegate void Parser( string fileName );

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataModeler()
        {
            catalogue = new Dictionary<string, CityInfo>();
        }

        /// <summary>
        /// Cleans a list of CityInfo objects which is the result
        /// of parsing a file. Assigns the clean list to catalogue.
        /// </summary>
        /// <param name="cities"></param>
        private void CleanList(ref List<CityInfo> cities)
        {
            foreach ( CityInfo cityInfo in cities )
            {
                if ( cityInfo != null )
                {
                    // If there is a duplicate city name, append the province name for clarity
                    if ( catalogue.ContainsKey( cityInfo.CityName.ToLower() ) )
                    {
                        // Replace accented string with ascii string using the Encoder class
                        string cityName = Encoder.AccentStringToASCII(cityInfo.CityName.ToLower());
                        catalogue.Add($"{cityName} ({cityInfo.Province})", cityInfo);
                    }
                    else if ( cityInfo.CityName == "" || cityInfo.CityName is null )
                    {
                        continue;
                    }
                    else
                    {
                        // Replace accented string with ascii string using the Encoder class
                        string cityName = Encoder.AccentStringToASCII(cityInfo.CityName.ToLower());
                        catalogue.Add(cityName, cityInfo);
                    }
                        
                }
            }
        }

        /// <summary>
        /// Parses through the passed JSON file,
        /// storing all cities in a Dictionary
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseJSON(string fileName)
        {
            string json;
            try
            {
                // Read JSON file data 
                json = File.ReadAllText( fileName );
                // populate list of cities
                List<CityInfo> cities = JsonConvert.DeserializeObject<List<CityInfo>>( json );
                CleanList( ref cities );
            }
            catch (Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// Parses through the passed XML file,
        /// storing all cities in a Dictionary
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseXML(string fileName)
        {
            try
            {
                // read XML file
                using ( var reader = new StreamReader( fileName ) )
                {
                    var deserializer = new XmlSerializer( typeof( List<CityInfo> ), new XmlRootAttribute( "CanadaCities" ) );
                    List<CityInfo> cities = (List<CityInfo>) deserializer.Deserialize( reader );
                    CleanList( ref cities );
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Parses through the passed CSV file,
        /// storing all cities in a Dictionary
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseCSV(string fileName)
        {
            try
            {
                using ( var reader = new StreamReader( fileName ) )
                using ( var csv = new CsvReader( reader, CultureInfo.InvariantCulture ) )
                {
                    var records = csv.GetRecords<CityInfo>();
                    List<CityInfo> cities = new List<CityInfo>();
                    foreach ( CityInfo cityInfo in records )
                    {
                        cities.Add( cityInfo );
                    }
                    CleanList( ref cities );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// Determines the file type to be parsed, sets a delegate
        /// to the according file parsing method, and then returns
        /// the resulting Dictionary
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            Parser parser;
            if ( fileType.ToLower() == "json" )
                parser = new Parser( ParseJSON );
            else if ( fileType.ToLower() == "xml" )
                parser = new Parser( ParseXML );
            else if (fileType.ToLower() == "csv" )
                parser = new Parser( ParseCSV );
            else
                parser = null;

            parser( fileName );
            return catalogue;
        }
    }
}
