/*
 * File: Statistics.cs
 * Descrption: Contains a Dictionary which stores a collection of City information, indexed by city name.
 *             This class also contains methods which display statistics about the cities, and also provinces.
 * Date: 2020-02-03
 */
using System.Collections.Generic;
using System.Linq;
using System.Device.Location;

namespace ProjectLibrary
{
    public class Statistics
    {
        // Data members
        public Dictionary<string, CityInfo> CityCatalogue { get; set; }

        /// <summary>
        /// 2-arg constructor: Parses the passed file upon initialization, 
        /// storing it in the CityCatalogue
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        public Statistics(string fileName, string fileType)
        {
            //get all values of all cities in Canada
            DataModeler dataModeler = new DataModeler();
            CityCatalogue = dataModeler.ParseFile( fileName, fileType );
        }

        //Check cities with same name
        public bool CityWithSameName(string CityName)
        {
            if (CityCatalogue.Where(c => c.Key.Contains(CityName.ToLower())).ToList().Count > 1)
                return true;
            return false;
        }

        // City Methods
        /// <summary>
        /// Get city info by its name
        /// </summary>
        /// <param name="CityName"></param>
        /// <returns>object CityInfo</returns>
        public CityInfo DisplayCityInformation(string CityName)
        {
            var selectedCity = CityCatalogue.Where(c => c.Key.Contains(CityName.ToLower())).FirstOrDefault();
            return selectedCity.Value;
        }

        /// <summary>
        /// display the city with largest population in a province
        /// </summary>
        /// <param name="Province"></param>
        /// <returns>object CityInfo</returns>
        public CityInfo DisplayLargestPopulationCity(string Province)
        {
            var citiesInProvince = CityCatalogue.Where(c => c.Value.GetProvince().ToLower() == Province.ToLower()).ToList();
            return citiesInProvince.OrderByDescending(c => c.Value.GetPopulation()).First().Value;
        }

        /// <summary>
        /// display the city with smallest population in a province
        /// </summary>
        /// <param name="Province"></param>
        /// <returns>object CityInfo</returns>
        public CityInfo DisplaySmallestPopulationCity(string Province)
        {
            var citiesInProvince = CityCatalogue.Where(c => c.Value.GetProvince().ToLower() == Province.ToLower()).ToList();
            return citiesInProvince.OrderBy(c => c.Value.GetPopulation()).First().Value;
        }

        /// <summary>
        /// compare population of two specific cities
        /// </summary>
        /// <param name="CityName1"></param>
        /// <param name="CityName2"></param>
        /// <returns>CityInfo object with higher population</returns>
        public CityInfo CompareCitiesPopulation(string CityName1, string CityName2)
        {
            var CityInfo1 = CityCatalogue.Where(c => c.Key.Contains(CityName1.ToLower())).FirstOrDefault().Value;
            var CityInfo2 = CityCatalogue.Where(c => c.Key.Contains(CityName2.ToLower())).FirstOrDefault().Value;
            return CityInfo1.GetPopulation() > CityInfo2.GetPopulation() ? CityInfo1 : CityInfo2;
        }

        /// <summary>
        /// Visually displays the location of the passed city on a map
        /// </summary>
        /// <param name="cityName"></param>
        public void ShowCityOnMap(string cityName)
        {
            //ToDO
        }

        /// <summary>
        /// calculate the distance between two cities (in Kilometers)
        /// </summary>
        /// <param name="CityName1"></param>
        /// <param name="CityName2"></param>
        /// <returns>double</returns>
        public double CalculateDistanceBetweenCities(string CityName1, string CityName2)
        {
            var CityInfo1 = CityCatalogue.Where(c => c.Key.Contains(CityName1.ToLower())).FirstOrDefault().Value;
            var CityInfo2 = CityCatalogue.Where(c => c.Key.Contains(CityName2.ToLower())).FirstOrDefault().Value;
            GeoCoordinate c1 = new GeoCoordinate(CityInfo1.Latitude, CityInfo1.Longitude);
            GeoCoordinate c2 = new GeoCoordinate(CityInfo2.Latitude, CityInfo2.Longitude);

            return c1.GetDistanceTo(c2) / 1000; //in KM
        }

        /// <summary>
        /// display population of the whole province
        /// </summary>
        /// <param name="Province"></param>
        /// <returns>double</returns>
        public double DisplayProvincePopulation(string Province)
        {
            var citiesInProvince = CityCatalogue.Where(c => c.Value.GetProvince().ToLower() == Province.ToLower()).ToList();
            double provincePopulation = 0.0;
            foreach (var city in citiesInProvince)
            {
                provincePopulation += city.Value.GetPopulation();
            }
            return provincePopulation;
        }

        /// <summary>
        /// display all cities in a province
        /// </summary>
        /// <param name="Province"></param>
        /// <returns>string</returns>
        public string DisplayProvinceCities(string Province)
        {
            var citiesInProvince = CityCatalogue.Where(c => c.Value.GetProvince().ToLower() == Province.ToLower()).ToList();
            string cityList = "";
            foreach (var city in citiesInProvince)
            {
                cityList += $"{city.Key}\n";
            }
            return cityList;
        }

        /// <summary>
        /// display a list of all provinces and their populations in ascending order
        /// </summary>
        /// <returns>string</returns>
        public string RankProvincesByPopulation()
        {
            SortedList<double, string> sortedPopulationList = new SortedList<double, string>();

            // get province list (each province will have one representative city)
            var provinceList = CityCatalogue.GroupBy(c => c.Value.GetProvince()).Select(group => group.First()).ToList();
            //get population of each province and add to sorted list
            foreach (var province in provinceList)
            {
                sortedPopulationList.Add(DisplayProvincePopulation(province.Value.GetProvince()), province.Value.GetProvince());
            }
            //display the list and return that list in string
            string display = "";
            int index = 1;
            foreach (var p in sortedPopulationList)
            {
                display += $"{index}. {p.Value} ({p.Key})\n";
                index++;
            }
            return display;
        }

        /// <summary>
        /// display a list of all provinces and their number of cities in ascending order
        /// </summary>
        /// <returns></returns>
        public string RankProvincesByCities()
        {
            Dictionary<string, double> CityCountList = new Dictionary<string, double>();

            // get province list (each province will have one representative city)
            var provinceList = CityCatalogue.GroupBy(c => c.Value.GetProvince()).Select(group => group.First()).ToList();

            //get city count by name of province and add to the list
            foreach (var province in provinceList)
            {
                int cityCountByProvince = CityCatalogue.Where(c => c.Value.GetProvince() == province.Value.GetProvince()).ToList().Count;
                CityCountList.Add(province.Value.GetProvince(), cityCountByProvince);
            }
            //display the list after sorting and return that list in string
            string display = "";
            int index = 1;
            foreach (var p in CityCountList.OrderBy(c => c.Value))
            {
                display += $"{index}. {p.Key} ({p.Value})\n";
                index++;
            }
            return display;
        }
    }
}