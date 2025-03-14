﻿using AgOpenGPS;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RateController
{
    public partial class frmPrimedStart : Form
    {
        public FormStart mf;
        private bool FormEdited;
        private bool Initializing;

        public frmPrimedStart(FormStart CalledFrom)
        {
            InitializeComponent();
            #region // language
            lbOnTime.Text = Lang.lgOnTime;
            lbPrimedSpeed.Text = Lang.lgSpeed;
            lbDelay.Text = Lang.lgSwitchDelay;
            lbOnSeconds.Text = Lang.lgSeconds;
            lbDelaySeconds.Text = Lang.lgSeconds;

            #endregion

            mf = CalledFrom;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UpdateForm();
            btnOK.Focus();
            SetButtons(false);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FormEdited)
                {
                    this.Close();
                }
                else
                {
                    // save data

                    if (double.TryParse(tbSpeed.Text, out double Speed)) mf.SimSpeed = Speed;
                    if (double.TryParse(tbTime.Text, out double Time)) mf.PrimeTime = Time;
                    if (int.TryParse(tbDelay.Text, out int Delay)) mf.PrimeDelay = Delay;

                    SetButtons(false);
                    UpdateForm();
                }
            }
            catch (Exception ex)
            {
                mf.Tls.ShowHelp(ex.Message, this.Text, 3000, true);
            }
        }

        private void frmPrimedStart_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                mf.Tls.SaveFormData(this);
            }
        }

        private void frmPrimedStart_Load(object sender, EventArgs e)
        {
            mf.Tls.LoadFormData(this);

            if (mf.UseInches)
            {
                lbSpeed.Text = "MPH";
            }
            else
            {
                lbSpeed.Text = "KMH";
            }
            SetDayMode();
            UpdateForm();
        }

        private void SetButtons(bool Edited = false)
        {
            if (!Initializing)
            {
                if (Edited)
                {
                    btnCancel.Enabled = true;
                    btnOK.Image = Properties.Resources.Save;
                }
                else
                {
                    btnCancel.Enabled = false;
                    btnOK.Image = Properties.Resources.OK;
                    btnOK.Enabled = true;
                }
                FormEdited = Edited;
            }
        }

        private void SetDayMode()
        {
            if (Properties.Settings.Default.IsDay)
            {
                this.BackColor = Properties.Settings.Default.DayColour;
                foreach (Control c in this.Controls)
                {
                    c.ForeColor = Color.Black;
                }
            }
            else
            {
                this.BackColor = Properties.Settings.Default.NightColour;
                foreach (Control c in this.Controls)
                {
                    c.ForeColor = Color.White;
                }
            }
        }

        private void tbDelay_Enter(object sender, EventArgs e)
        {
            double tempD;
            double.TryParse(tbDelay.Text, out tempD);
            using (var form = new FormNumeric(0, 8, tempD))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tbDelay.Text = form.ReturnValue.ToString("N0");
                }
            }
        }

        private void tbDelay_Validating(object sender, CancelEventArgs e)
        {
            double tempD;
            double.TryParse(tbDelay.Text, out tempD);
            if (tempD < 0 || tempD > 8)
            {
                System.Media.SystemSounds.Exclamation.Play();
                e.Cancel = true;
            }
        }

        private void tbSpeed_Enter(object sender, EventArgs e)
        {
            double tempD;
            double.TryParse(tbSpeed.Text, out tempD);
            using (var form = new FormNumeric(0, 40, tempD))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tbSpeed.Text = form.ReturnValue.ToString("N1");
                }
            }
        }

        private void tbSpeed_TextChanged(object sender, EventArgs e)
        {
            SetButtons(true);
        }

        private void tbSpeed_Validating(object sender, CancelEventArgs e)
        {
            double tempD;
            double.TryParse(tbSpeed.Text, out tempD);
            if (tempD < 0 || tempD > 40)
            {
                System.Media.SystemSounds.Exclamation.Play();
                e.Cancel = true;
            }
        }

        private void tbTime_Enter(object sender, EventArgs e)
        {
            double tempD;
            double.TryParse(tbTime.Text, out tempD);
            using (var form = new FormNumeric(0, 30, tempD))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tbTime.Text = form.ReturnValue.ToString("N1");
                }
            }
        }

        private void tbTime_TextChanged(object sender, EventArgs e)
        {
            SetButtons(true);
        }

        private void tbTime_Validating(object sender, CancelEventArgs e)
        {
            double tempD;
            double.TryParse(tbTime.Text, out tempD);
            if (tempD < 0 || tempD > 30)
            {
                System.Media.SystemSounds.Exclamation.Play();
                e.Cancel = true;
            }
        }

        private void UpdateForm()
        {
            Initializing = true;

            tbSpeed.Text = mf.SimSpeed.ToString("N1");
            tbTime.Text = mf.PrimeTime.ToString("N0");
            tbDelay.Text = mf.PrimeDelay.ToString("N0");

            Initializing = false;
        }
    }
}