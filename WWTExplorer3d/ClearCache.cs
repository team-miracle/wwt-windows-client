using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TerraViewer
{
    public partial class ClearCache : Form
    {
        public ClearCache()
        {
            InitializeComponent();
            SetUiStrings();
        }

        private void SetUiStrings()
        {
            this.progressText.Text = Language.GetLocalizedText(152, "Calculating Cache Use");
            this.catalogs.Text = Language.GetLocalizedText(153, "Catalogs");
            this.tours.Text = Language.GetLocalizedText(154, "Cached Tours");
            this.imagery.Text = Language.GetLocalizedText(155, "Imagery");
            this.Cancel.Text = Language.GetLocalizedText(157, "Cancel");
            this.Purge.Text = Language.GetLocalizedText(158, "Purge");
            this.label1.Text = Language.GetLocalizedText(159, "Select Data to Purge");
            this.Text = Language.GetLocalizedText(160, "Manage Data Cache");
            this.warningText.Text = Language.GetLocalizedText(146, "WorldWide Telescope keeps copies of the imagery, tours, and catalogs that you view. Keeping this data enables you to view tours and imagery again quickly even if your computer is not connected to the Internet. Selecting options and clicking Purge frees up disk space but the data is not available until it is downloaded again.");
        }


        private void catalogSize_Click(object sender, EventArgs e)
        {

        }
        long totalImagery = 0;
        long totalCatalog = 0;
        long totalTours = 0;
        long grandTotal = 0;
        public long totalDeleted = 0;
        bool purgeMode = false;
        bool purgeImagery = false;
        bool purgeTours = false;
        bool purgeCatalogs = false;

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!purgeMode)
            {
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"Imagery\", ref totalImagery, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"dem\", ref totalImagery, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"tourCache\", ref totalTours, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"Temp\", ref totalTours, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"TourRatings\", ref totalTours, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"thumbnails\", ref totalCatalog, 0);
                TotalDirecotry(Properties.Settings.Default.CahceDirectory + @"data\", ref totalCatalog, 0);
                grandTotal = totalCatalog + totalImagery + totalTours;
            }
            else
            {
                long totalDeleted = 0;
                if (purgeImagery)
                {
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"Imagery\", ref totalDeleted);
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"dem\", ref totalDeleted);
                }
                if (purgeTours)
                {
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"tourCache\", ref totalDeleted);
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"Temp\", ref totalDeleted);
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"TourRatings\", ref totalDeleted); 
                }
                if (purgeCatalogs)
                {
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"data\", ref totalDeleted);
                    PurgeDirecotry(Properties.Settings.Default.CahceDirectory + @"thumbnails\", ref totalDeleted);
                }
                scanCompleted = true;
            }            
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            imagerySize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalImagery / 1024.0 / 1024.0).ToString("f") + "MB";
            tourSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalTours / 1024.0 / 1024.0).ToString("f") + "MB";
            catalogSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalCatalog / 1024.0 / 1024.0).ToString("f") + "MB";
            progressBar.Value = Math.Min(100,e.ProgressPercentage);
            this.Refresh();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (purgeMode)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            imagerySize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalImagery / 1024.0 / 1024.0).ToString("f") + "MB";
            tourSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalTours / 1024.0 / 1024.0).ToString("f") + "MB";
            catalogSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalCatalog / 1024.0 / 1024.0).ToString("f") + "MB";
            progressText.Text = Language.GetLocalizedText(491, "Scan complete");
            progressBar.Value = 100;
            scanCompleted = true;
        }
        int percentage = 0;

        public void TotalDirecotry(string parent, ref long totalBytes, int level)
        {
            level++;
            if (!Directory.Exists(parent))
            {
                return;
            }
            foreach (string dir in Directory.GetDirectories(parent))
            {
                TotalDirecotry(dir, ref totalBytes, level);
            }
            foreach (string file in Directory.GetFiles(parent))
            {
                FileInfo fi = new FileInfo(file);
                totalBytes += fi.Length;
            }

            if (level < 3)
            {
                percentage++;
                percentage = percentage % 100;
                backgroundWorker.ReportProgress(percentage);
            }
        }

        public static void TotalDir(string parent, ref long totalBytes, int level)
        {
            level++;
            if (!Directory.Exists(parent))
            {
                return;
            }
            foreach (string dir in Directory.GetDirectories(parent))
            {
                TotalDir(dir, ref totalBytes, level);
            }
            foreach (string file in Directory.GetFiles(parent))
            {
                FileInfo fi = new FileInfo(file);
                totalBytes += fi.Length;
            }
        }

        private void PurgeDirecotry(string parent, ref long totalBytes)
        {
            //Directory.Delete(dir, true);
            foreach (string dir in Directory.GetDirectories(parent))
            {
                PurgeDirecotry(dir, ref totalBytes);
                Directory.Delete(dir, false);
            }
            foreach (string file in Directory.GetFiles(parent))
            {
                FileInfo fi = new FileInfo(file);
                totalBytes += fi.Length;
                File.Delete(file);

            }
            backgroundWorker.ReportProgress((int)(100 * totalBytes / grandTotal));
        }
        static public void PurgeDirecotryNoProgress(string parent, ref long totalBytes)
        {
            //Directory.Delete(dir, true);
            foreach (string dir in Directory.GetDirectories(parent))
            {
                PurgeDirecotryNoProgress(dir, ref totalBytes);
                if (Directory.Exists(dir))
                {
                    try
                    {
                        Directory.Delete(dir, false);
                    }
                    catch
                    {
                    }
                }
            }
            foreach (string file in Directory.GetFiles(parent))
            {
                FileInfo fi = new FileInfo(file);
                try
                {
                    long fileBytes = fi.Length;
                    File.Delete(file);
                    totalBytes += fileBytes;
                }
                catch
                {
                }

            }
            try
            {
                Directory.Delete(parent, false);
            }
            catch
            {
            }
        }  
        bool scanStarted = false;

        bool scanCompleted = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!scanStarted)
            {
                backgroundWorker.RunWorkerAsync();
            }
            scanStarted = true;

            imagerySize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalImagery / 1024.0 / 1024.0).ToString("f") + "MB";
            tourSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalTours / 1024.0 / 1024.0).ToString("f") + "MB";
            catalogSize.Text = Language.GetLocalizedText(147, "Size : ") + ((double)totalCatalog / 1024.0 / 1024.0).ToString("f") + "MB";

            purgeImagery = imagery.Checked;
            purgeCatalogs = catalogs.Checked;
            purgeTours = tours.Checked;

            Purge.Enabled = (purgeCatalogs || purgeImagery || purgeTours) && scanCompleted;
            Refresh();
        }

        private void ClearCache_Load(object sender, EventArgs e)
        {
            progressText.Text = Language.GetLocalizedText(148, "Scanning Cache Size");
            timer.Enabled = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            purgeImagery = imagery.Checked;
            purgeCatalogs = catalogs.Checked;
            purgeTours = tours.Checked;

            if (purgeCatalogs || purgeImagery || purgeTours)
            {
                if (purgeCatalogs)
                {
                    if (UiTools.ShowMessageBox(Language.GetLocalizedText(149, "This will delete custom constellation figures as well as other selected data and close the application. Do you want to continue?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    if (UiTools.ShowMessageBox(Language.GetLocalizedText(150, "This will delete cached data and close the application. Do you want to continue?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                purgeMode = true;
                timer.Enabled = true;
                progressText.Text = Language.GetLocalizedText(151, "Purging Cache");
                progressBar.Value = 0;
                Purge.Enabled = false;
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            this.Close();
        }
    }
}