using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;

/*
 * Copyright (c) 2023 e1z0 e1z0@vintage2000.org
 */

namespace CpuTempCli
{
    class Program
    {

        static String ExtractValue(string input, string pattern)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.Match match = regex.Match(input);
            if (match.Success)
            {
                return match.Value;
            } else
            {
                return null;
            }
        }
        static String ReturnName(String req)
        {
            string pattern = @"(?<=\\)[A-Za-z0-9]+(?=\\)";
            string cut_name = ExtractValue(req, pattern);
            if (cut_name == null)
            {
                return req;
            }
            
            switch (cut_name)
            {
                // TODO: these names and descriptions should be updated ASAP
                case "QCOM248C":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM248D":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM248E":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM248F":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2470":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM24AD":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM24AE":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM24A8":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM24A9":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2471":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2472":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2473":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2474":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2475":
                    return "Qualcomm Temperature Sensor Device";
                case "QCOM2491":
                    return "Qualcomm ADC Temperature Monitor Device";
                case "QCOM2492":
                    return "Qualcomm ADC Temperature Monitor Device";
                case "QCOM2493":
                    return "Qualcomm ADC Temperature Monitor Device";
                case "QCOM2486":
                    return "Qualcomm Battery Current Limit Monitor Device";
                case "QCOM2487":
                    return "Qualcomm WWAN Coexistence Manager ";
                case "QCOM24AF":
                    return "Qualcomm Temperature Sensor Device";
                case "MSHW1011":
                    return "Icaros_KMD_ESP_Thermal Device";
            }
            
            return QueryDevice(cut_name);
        }

        static String QueryDevice(String acpiDevice)
        {
            string query = $"SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%{acpiDevice}%'";
            string dev = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string deviceName = obj["Name"] as string;
                    string deviceId = obj["DeviceID"] as string;
                    string description = obj["Description"] as string;
                    dev = deviceName + " " + deviceId + " " + description;
                    Console.WriteLine("Device Name: " + deviceName);
                    Console.WriteLine("Device ID: " + deviceId);
                    Console.WriteLine("Description: " + description);
                    Console.WriteLine();
                }
                return dev;
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "CPU Temperature for x86/x86_64/ARM64 Systems";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                while (true)
                {
                    Console.WriteLine("-------------> REFRESH <----------------------");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        if (queryObj == null) continue;
                        double temp = Convert.ToDouble(queryObj["CurrentTemperature"].ToString());
                        double temp_cel = (temp / 10 - 273.15);
                        string name = ReturnName(queryObj["InstanceName"].ToString());
       
                        Console.WriteLine("Unit: {0} Temp: {1}C",name,temp_cel);
                    }
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
    }
}
