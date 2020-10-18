using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Webszolgaltatas_MNB.Entities;
using Webszolgaltatas_MNB.MnbServiceReference;

namespace Webszolgaltatas_MNB
{
    public partial class Form1 : Form
    {

        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        public Form1()
        {
            InitializeComponent();
            //GetCurrencies();
            RefreshData();

            CurrencycomboBox.DataSource = Currencies;
            
        }

        public string CallWebservice()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = CurrencycomboBox.SelectedIndex.ToString(),
                startDate = StartdateTimePicker.Value.ToString(),
                endDate = EnddateTimePicker.Value.ToString()

            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            return result;
        }

        public void XMLProcess(string result) 
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null)
                    continue;
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        public void MakeChart()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";


            series.BorderWidth = 2;
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;
            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        public void RefreshData()
        {
            Rates.Clear();

            string result = CallWebservice();

            XMLProcess(result);

            MakeChart();

            dataGridView1.DataSource = Rates;
        }

        private void StartdateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void EnddateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void CurrencycomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        public void GetCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var cur_request = new GetCurrenciesRequestBody()
            {
                
            };

            var cur_response = mnbService.GetCurrencies(cur_request);

            // Ebben az esetben a "var" a GetExchangeRatesResult property alapján kapja a típusát.
            // Ezért a result változó valójában string típusú.
            var cur_result = cur_response.GetCurrenciesResult;


            var xml = new XmlDocument();
            xml.LoadXml(cur_result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null)
                    continue;
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }
    }
}
