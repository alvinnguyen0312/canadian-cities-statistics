/*
 * File: CityInfo.cs
 * Descrption: Contains information for a city. Uses attributes specific to file-types
 *             such as: JSON, XML, and CSV
 * Date: 2020-02-03
 */
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ProjectLibrary
{
    [XmlType("CanadaCity")]
    public class CityInfo
    {
        // Class Properties
        [JsonProperty("id")]
        [XmlElement("id")]
        [Name( "id" )]
        public string CityID { get; set; }
        [JsonProperty( "city" )]
        [XmlElement( "city" )]
        [Name( "city" )]
        public string CityName { get; set; }
        [JsonProperty( "city_ascii" )]
        [XmlElement( "city_ascii" )]
        [Name( "city_ascii" )]
        public string CityAscii { get; set; }
        [JsonProperty( "population" )]
        [XmlElement( "population" )]
        [Name( "population" )]
        public long Population { get; set; }
        [JsonProperty( "admin_name" )]
        [XmlElement( "admin_name" )]
        [Name( "admin_name" )]
        public string Province { get; set; }
        [JsonProperty( "lat" )]
        [XmlElement( "lat" )]
        [Name( "lat" )]
        public double Latitude { get; set; }
        [JsonProperty( "lng" )]
        [XmlElement( "lng" )]
        [Name( "lng" )]
        public double Longitude { get; set; }

        /// <summary>
        /// Returns a string containing the provinces
        /// </summary>
        /// <returns></returns>
        public string GetProvince()
        {
            // Replace accented string with ascii string using the Encoder class
            return Encoder.AccentStringToASCII(Province);
        }

        /// <summary>
        /// Returns a long containing the Population
        /// </summary>
        /// <returns></returns>
        public long GetPopulation()
        {
            return Population;
        }

        /// <summary>
        /// Returns a string containing the Latitude and Longitude
        /// </summary>
        /// <returns></returns>
        public string GetLocation()
        {
            return $"Latitude: {Latitude} - Longitude: {Longitude}";
        }

        /// <summary>
        /// Displays all of the information of a CityInfo object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"City ID: {CityID}\nCity Name: {CityName}\nPopulation: {Population}\nCity ASCII: {CityAscii}\nProvince: {Province}\nLatitude: {Latitude}\nLongitude: {Longitude}";
        }
    }
}
