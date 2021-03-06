﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Blinkdown
{
    public partial class Form1 : Form
    {
        private string _fileName;
        private bool _modified;

        public Form1()
        {
            InitializeComponent();
            _fileName = "";
            if (_fileName == "")
            {
             this.Text = "Untitled - " + Text;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!_modified)
            {
                _modified = true;
                Text = "*" + Text;
            }
            string MdHtml = CommonMark.CommonMarkConverter.Convert(txtDocument.Text);
            webBrowser1.Document.OpenNew(false);
            webBrowser1.Document.Write(MdHtml);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader InputFile = new StreamReader(openFileDialog1.FileName))
            {
                string InputText = InputFile.ReadToEnd();
                txtDocument.Text = InputText;
                Text = openFileDialog1.FileName + " - Blinkdown";
                webBrowser1.Document.Title = new FileInfo(openFileDialog1.FileName).Name;
                _fileName = openFileDialog1.FileName;
                _modified = false;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            _fileName = dlgSave.FileName;
            using (StreamWriter OutputFile = new StreamWriter(_fileName)) {
                OutputFile.Write(txtDocument.Text);
                _modified = false;
                Text = _fileName + " - Blinkdown";
                webBrowser1.Document.Title = new FileInfo(_fileName).Name;

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                //Show the save as dialog.
                dlgSave.ShowDialog();
            } //if filename is empty
            else
            {
                dlgSave.FileName = _fileName;
                dlgSave_FileOk(this, new CancelEventArgs());
            } //normal save
        }

        private DialogResult _ShouldSave()
        {
            string SaveFile;
            if (string.IsNullOrEmpty(_fileName))
            {
                SaveFile = "untitled";
            } //If untitled file
            else
            {
                SaveFile = new FileInfo(_fileName).Name;
            } //else, we have an existing file name
            String MessageBoxText = String.Format("Save changes to {0}?", SaveFile);
            DialogResult Result = MessageBox.Show(MessageBoxText, "Blinkdown", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            return Result;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_modified)
            {
                DialogResult Dr = _ShouldSave();
                if (DialogResult.Yes == Dr)
                {
                    saveToolStripMenuItem_Click(this, new EventArgs());
                } //If yes
                if (DialogResult.Cancel == Dr)
                {
                    e.Cancel = true;
                }

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F6:
                    if (txtDocument.Focused)
                    {
                        webBrowser1.Focus();
                    } // input control
                    else 
                    {
                        txtDocument.Focus();
                    } //else, in web browser
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            } //switch
        }
    }
}
