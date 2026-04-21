using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SumCheckApp
{
    public partial class MainForm : Form
    {
        private TextBox? txtFilePath;
        private ComboBox? cmbHashAlgorithm;
        private TextBox? txtComputedHash;
        private TextBox? txtExpectedHash;
        private Label? lblResult;
        private Button? btnBrowse;
        private Button? btnCompute;
        private Button? btnVerify;
        private Button? btnExit;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SumCheck - Verificare Hash Fișier";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Label pentru calea fișierului
            var lblFilePath = new Label
            {
                Text = "Cale fișier:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblFilePath);

            // TextBox pentru calea fișierului
            txtFilePath = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(450, 25),
                ReadOnly = true
            };
            this.Controls.Add(txtFilePath);

            // Button Browse
            btnBrowse = new Button
            {
                Text = "Caută...",
                Location = new Point(480, 43),
                Size = new Size(90, 27)
            };
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);

            // Label pentru algoritmul de hash
            var lblAlgorithm = new Label
            {
                Text = "Algoritm hash:",
                Location = new Point(20, 85),
                AutoSize = true
            };
            this.Controls.Add(lblAlgorithm);

            // ComboBox pentru alegerea algoritmului
            cmbHashAlgorithm = new ComboBox
            {
                Location = new Point(20, 110),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbHashAlgorithm.Items.AddRange(new object[] { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" });
            cmbHashAlgorithm.SelectedIndex = 2; // SHA256 default
            this.Controls.Add(cmbHashAlgorithm);

            // Button Compute Hash
            btnCompute = new Button
            {
                Text = "Calculează Hash",
                Location = new Point(240, 108),
                Size = new Size(130, 30)
            };
            btnCompute.Click += BtnCompute_Click;
            this.Controls.Add(btnCompute);

            // Label pentru hash calculat
            var lblComputedHash = new Label
            {
                Text = "Hash calculat:",
                Location = new Point(20, 155),
                AutoSize = true
            };
            this.Controls.Add(lblComputedHash);

            // TextBox pentru hash calculat
            txtComputedHash = new TextBox
            {
                Location = new Point(20, 180),
                Size = new Size(550, 25),
                ReadOnly = true,
                Font = new Font("Courier New", 9F)
            };
            this.Controls.Add(txtComputedHash);

            // Label pentru hash așteptat
            var lblExpectedHash = new Label
            {
                Text = "Hash așteptat (opțional):",
                Location = new Point(20, 220),
                AutoSize = true
            };
            this.Controls.Add(lblExpectedHash);

            // TextBox pentru hash așteptat
            txtExpectedHash = new TextBox
            {
                Location = new Point(20, 245),
                Size = new Size(550, 25),
                Font = new Font("Courier New", 9F)
            };
            this.Controls.Add(txtExpectedHash);

            // Button Verify
            btnVerify = new Button
            {
                Text = "Verifică Potrivire",
                Location = new Point(20, 285),
                Size = new Size(150, 35)
            };
            btnVerify.Click += BtnVerify_Click;
            this.Controls.Add(btnVerify);

            // Label pentru rezultat
            lblResult = new Label
            {
                Text = "",
                Location = new Point(20, 330),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(lblResult);

            // Button Exit
            btnExit = new Button
            {
                Text = "Ieșire",
                Location = new Point(480, 380),
                Size = new Size(90, 30)
            };
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
        }

        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selectează fișierul pentru verificare",
                Filter = "Toate fișierele (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath!.Text = openFileDialog.FileName;
                lblResult!.Text = "";
                txtComputedHash!.Text = "";
            }
        }

        private void BtnCompute_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilePath!.Text))
            {
                MessageBox.Show("Vă rugăm să selectați un fișier mai întâi!", "Atenție", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Fișierul specificat nu există!", "Eroare", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string algorithm = cmbHashAlgorithm!.SelectedItem?.ToString() ?? "SHA256";
                string hash = ComputeFileHash(txtFilePath.Text, algorithm);
                txtComputedHash!.Text = hash;
                lblResult!.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la calcularea hash-ului: {ex.Message}", "Eroare", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ComputeFileHash(string filePath, string algorithm)
        {
            using HashAlgorithm hashAlgorithm = algorithm switch
            {
                "MD5" => MD5.Create(),
                "SHA1" => SHA1.Create(),
                "SHA384" => SHA384.Create(),
                "SHA512" => SHA512.Create(),
                _ => SHA256.Create()
            };

            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hashBytes = hashAlgorithm.ComputeHash(fileStream);
            
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            
            return sb.ToString();
        }

        private void BtnVerify_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtExpectedHash!.Text))
            {
                MessageBox.Show("Vă rugăm să introduceți hash-ul așteptat pentru verificare!", "Atenție", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtComputedHash!.Text))
            {
                MessageBox.Show("Vă rugăm să calculați mai întâi hash-ul fișierului!", "Atenție", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string computed = txtComputedHash.Text.Trim().ToLower();
            string expected = txtExpectedHash.Text.Trim().ToLower();

            if (computed == expected)
            {
                lblResult!.Text = "✓ Hash-urile coincid! Fișierul este integru.";
                lblResult.ForeColor = Color.Green;
                MessageBox.Show("Hash-urile coincid! Fișierul este integru și nu a fost modificat.", 
                    "Verificare Reușită", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblResult!.Text = "✗ Hash-urile NU coincid! Fișierul poate fi corupt sau modificat.";
                lblResult.ForeColor = Color.Red;
                MessageBox.Show("Hash-urile NU coincid! Fișierul poate fi corupt sau modificat.", 
                    "Verificare Eșuată", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
