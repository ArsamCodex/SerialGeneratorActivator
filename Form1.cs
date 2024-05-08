using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace SerialGeneratorActivator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var x = GetHardDriveInfo();
            var y = GetCPUInfo();
            var z = GetGPUInfo();
            var c = GetBIOSVersion();
            var ff = ComputeSHA256Hash(x + y + z + c);
            richTextBox1.AppendText(ff);



        }

        /*
         * Here we generate the name of hard drive to convert to SHA 256 hash for activation
         * */
        static string GetHardDriveInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            ManagementObjectCollection drives = searcher.Get();

            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject drive in drives)
            {
                sb.AppendLine($"Model: {drive["Model"]}, Manufacturer: {drive["Manufacturer"]}, Size: {drive["Size"]}");
            }
            return sb.ToString();
        }

        /*
         * Here we get CPu hardware Name for activation in SHA 256 key 
         * */
        static string GetCPUInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            ManagementObjectCollection cpus = searcher.Get();

            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject cpu in cpus)
            {
                sb.AppendLine($"Name: {cpu["Name"]}, Manufacturer: {cpu["Manufacturer"]}, Max Clock Speed: {cpu["MaxClockSpeed"]} MHz");
            }
            return sb.ToString();
        }
        /* 
         * Here we get GPU hardware info for activation in SHA 256
         * */
        static string GetGPUInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            ManagementObjectCollection gpus = searcher.Get();

            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject gpu in gpus)
            {
                sb.AppendLine($"Name: {gpu["Name"]}, Adapter RAM: {gpu["AdapterRAM"]} bytes, Driver Version: {gpu["DriverVersion"]}");
            }
            return sb.ToString();
        }
        /* 
         * Here we get Bios version name for activation in Sha 256
         * */
        static string GetBIOSVersion()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection biosCollection = searcher.Get();

            string biosVersion = "";
            foreach (ManagementObject bios in biosCollection)
            {
                biosVersion = bios["Version"].ToString();
                break; // Assuming there's only one BIOS, so we break after the first iteration
            }
            return biosVersion;
        }
        /*
         * Here we have function accept string return sha 256
         * Now we have 4 string inabove functions now we make 1 string and pass it here
         */
        static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}