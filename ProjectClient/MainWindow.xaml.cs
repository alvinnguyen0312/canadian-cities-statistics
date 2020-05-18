using System;
using System.Globalization;
using System.Linq;
using System.Windows;

using ProjectLibrary;

namespace ProjectClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Member variable
        private const string CSV_FILE = @"..\..\..\data\Canadacities.csv";
        private const string JSON_FILE = @"..\..\..\data\Canadcities-JSON.json";
        private const string XML_FILE = @"..\..\..\data\Canadcities-XML.xml";
        private Statistics stats;
        private CityInfo cityInfo;
        private bool isDocumentChosen = false;
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

        public MainWindow()
        {
            // Automatically resize height and width relative to content
            this.SizeToContent = SizeToContent.WidthAndHeight;
            InitializeComponent();

            // Create a CityInfo object
            cityInfo = new CityInfo();
        }


        /// <summary>
        /// Terminate the application
        /// </summary>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Load the CSV file and show the message to the user that the file is loaded.
        /// </summary>
        private void ButtonParseCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stats = new Statistics(CSV_FILE, "csv");
                txtMessage.Text = "CSV loaded.";
                isDocumentChosen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Load the JSON file and show the message to the user that the file is loaded.
        /// </summary>
        private void ButtonParseJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stats = new Statistics(JSON_FILE, "json");
                txtMessage.Text = "JSON loaded.";
                isDocumentChosen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Load the XML file and show the message to the user that the file is loaded.
        /// </summary>
        private void ButtonParseXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stats = new Statistics(XML_FILE, "xml");
                txtMessage.Text = "XML loaded.";
                isDocumentChosen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Load the city information and display it to the user
        /// </summary>
        private void ButtonGetCityInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                //Check if inquired city has duplicate names in other provinces
                bool isDuplicateName = stats.CityWithSameName(txtCityInfoName.Text);
                if (isDuplicateName)
                    MessageBox.Show("Your input city has been found in more than one province. Please enter the city name with province name. For example: Windsor (Ontario)");
               cityInfo = stats.DisplayCityInformation(txtCityInfoName.Text);

                if (cityInfo == null) throw new Exception();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the city name. Please try it again!");
            }

            if (cityInfo != null)
            {
                txtCityInfoProvince.Text = cityInfo.Province;
                txtCityInfoPopulation.Text = cityInfo.Population.ToString();
                txtCityInfoLat.Text = cityInfo.Latitude.ToString();
                txtCityInfoLong.Text = cityInfo.Longitude.ToString();
            }            
        }

        /// <summary>
        /// Load the largest population city and display it to the user
        /// </summary>
        private void ButtonGetLargestPopulationCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                cityInfo = stats.DisplayLargestPopulationCity(txtLargestPopulationProvinceName.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the province name. Please try it again!");
            }

            txtLargestPopulationCityName.Text = cityInfo.CityName;
            txtLargestPopulationCityPopulation.Text = cityInfo.Population.ToString();

        }

        /// <summary>
        /// Load the smallest population city and display it to the user
        /// </summary>
        private void ButtonGetSmallestPopulationCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                cityInfo = stats.DisplaySmallestPopulationCity(txtSmallestPopulationProvinceName.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the province name. Please try it again!");
            }

            txtSmallestPopulationCityName.Text = cityInfo.CityName;
            txtSmallestPopulationCityPopulation.Text = cityInfo.Population.ToString();
        }

        /// <summary>
        /// Compare two given cities and display a larger population city to the user
        /// </summary>
        private void ButtonCompare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                // Prevent to shows the city named 'Selkirk', if the user input nothing,
                if (txtCompPopulationCity1.Text == "" || txtCompPopulationCity2.Text == "") throw new Exception();

                cityInfo = stats.CompareCitiesPopulation(txtCompPopulationCity1.Text, txtCompPopulationCity2.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the city name. Please try it again!");
            }

            txtCompPopulationLargarCity.Text = cityInfo.CityName;
            txtCompPopulationLargarCityPopulation.Text = cityInfo.Population.ToString();

        }


        /// <summary>
        /// Calculate the distance among two given cities and display it to the user
        /// </summary>
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            double distance = 0.0;
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                distance = stats.CalculateDistanceBetweenCities(txtCalcDistanceCity1.Text, txtCalcDistanceCity2.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the city name. Please try it again!");
            }

            txtCalcDistance.Text = distance.ToString() + " km";

        }

        /// <summary>
        /// Load the population of a given province and display it to the user
        /// </summary>
        private void ButtonGetProvincePopulation_Click(object sender, RoutedEventArgs e)
        {
            double population = 0.0;
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                population = stats.DisplayProvincePopulation(txtProvinceInfoName.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the province name. Please try it again!");
            }

            txtProvinceInfoPopulation.Text = population.ToString();
        }


        /// <summary>
        /// Load the population of a given province and display it to the user
        /// </summary>
        private void RadioBtnProvinceCities_Checked(object sender, RoutedEventArgs e)
        {
            // Reset
            txtProvinceFunctionList.Items.Clear();

            string list = "";
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                list = stats.DisplayProvinceCities(txtProvinceInfoName.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception)
            {
                MessageBox.Show("Can't find the province name. Please try it again!");
            }

            // Split the list
            string[] cities = list.Split('\n');

            // This is to enable scrollable view.
            foreach (string city in cities)
            {
                txtProvinceFunctionList.Items.Add(myTI.ToTitleCase(city));
            }
        }

        /// <summary>
        /// Load the list of provinces that is ranked by population and display it to the user
        /// </summary>
        private void RadioBtnRankPopulation_Checked(object sender, RoutedEventArgs e)
        {
            // Reset
            txtProvinceFunctionList.Items.Clear();

            string list = "";
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                list = stats.RankProvincesByPopulation();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Split the list
            string[] provinces = list.Split('\n');

            // This is to enable scrollable view.
            foreach (string province in provinces)
            {
                txtProvinceFunctionList.Items.Add(province);
            }
        }

        /// <summary>
        /// Load the list of provinces that is ranked by cities and display it to the user
        /// </summary>
        private void RadioBtnRankCities_Checked(object sender, RoutedEventArgs e)
        {
            // Reset
            txtProvinceFunctionList.Items.Clear();

            string list = "";
            try
            {
                // If the user didn't choose the document, fire up the error
                if (!isDocumentChosen) throw new ArgumentNullException();

                list = stats.RankProvincesByCities();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please choose the document first!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Split the list
            string[] provinces = list.Split('\n');

            // This is to enable scrollable view.
            foreach (string province in provinces)
            {
                txtProvinceFunctionList.Items.Add(province);
            }
        }
    }
}

