
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LibraryBookManager
{
    public partial class Form1 : Form
    {
        private readonly string dbPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Library.db"
        );

        private TextBox txtTitle = null!;
        private TextBox txtAuthor = null!;
        private TextBox txtCategory = null!;
        private TextBox txtSearch = null!;

        private CheckBox chkAvailable = null!;

        private DataGridView dgvBooks = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnSearch = null!;
        private Button btnClear = null!;

        private int selectedBookID = 0;

        private string ConnectionString =>
            "Data Source=" + dbPath + ";Version=3;";

        public Form1()
        {
            InitializeComponent();

            BuildInterface();

            InitializeDatabase();

            LoadBooks();
        }

        // 1. DATABASE INITIALIZATION

        private void InitializeDatabase()
        {
            using (SQLiteConnection con =
                new SQLiteConnection(ConnectionString))
            {
                con.Open();

                string query = @"
                    CREATE TABLE IF NOT EXISTS Books
                    (
                        BookID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Author TEXT NOT NULL,
                        Category TEXT,
                        Available INTEGER
                    )";

                using (SQLiteCommand cmd =
                    new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 2. CREATE FORM DESIGN

        private void BuildInterface()
        {
            this.Text = "Library Book Manager";
            this.Size = new Size(1000, 650);
            this.MinimumSize = new Size(800, 550);
            this.StartPosition =
                FormStartPosition.CenterScreen;

            Label heading = new Label();

            heading.Text = "LIBRARY BOOK MANAGER";
            heading.Font =
                new Font("Arial", 20, FontStyle.Bold);

            heading.Location = new Point(300, 15);
            heading.AutoSize = true;

            this.Controls.Add(heading);

            // BOOK TITLE

            Label lblTitle = new Label();
            lblTitle.Text = "Book Title:";
            lblTitle.Location = new Point(30, 75);
            lblTitle.AutoSize = true;

            txtTitle = new TextBox();
            txtTitle.Location = new Point(130, 72);
            txtTitle.Width = 200;

            // AUTHOR

            Label lblAuthor = new Label();
            lblAuthor.Text = "Author:";
            lblAuthor.Location = new Point(370, 75);
            lblAuthor.AutoSize = true;

            txtAuthor = new TextBox();
            txtAuthor.Location = new Point(450, 72);
            txtAuthor.Width = 200;

            // CATEGORY

            Label lblCategory = new Label();
            lblCategory.Text = "Category:";
            lblCategory.Location = new Point(30, 120);
            lblCategory.AutoSize = true;

            txtCategory = new TextBox();
            txtCategory.Location = new Point(130, 117);
            txtCategory.Width = 200;

            // AVAILABLE CHECKBOX

            chkAvailable = new CheckBox();
            chkAvailable.Text = "Available";
            chkAvailable.Location = new Point(370, 117);
            chkAvailable.Checked = true;
            chkAvailable.AutoSize = true;

            // ADD BUTTON

            btnAdd = new Button();
            btnAdd.Text = "Add Book";
            btnAdd.Location = new Point(30, 170);
            btnAdd.Size = new Size(110, 35);
            btnAdd.Click += BtnAdd_Click;

            // UPDATE BUTTON

            btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new Point(150, 170);
            btnUpdate.Size = new Size(110, 35);
            btnUpdate.Click += BtnUpdate_Click;

            // DELETE BUTTON

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(270, 170);
            btnDelete.Size = new Size(110, 35);
            btnDelete.Click += BtnDelete_Click;

            // CLEAR BUTTON

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(390, 170);
            btnClear.Size = new Size(110, 35);
            btnClear.Click += BtnClear_Click;

            // SEARCH TEXTBOX

            txtSearch = new TextBox();
            txtSearch.Location = new Point(30, 235);
            txtSearch.Width = 300;

            // SEARCH BUTTON

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(340, 232);
            btnSearch.Size = new Size(100, 30);
            btnSearch.Click += BtnSearch_Click;

            // DATAGRIDVIEW

            dgvBooks = new DataGridView();

            dgvBooks.Location = new Point(30, 285);
            dgvBooks.Size = new Size(910, 290);

            dgvBooks.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvBooks.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvBooks.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvBooks.MultiSelect = false;
            dgvBooks.ReadOnly = true;
            dgvBooks.AllowUserToAddRows = false;

            dgvBooks.CellClick += DgvBooks_CellClick;

            // ADD CONTROLS TO FORM

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);

            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAuthor);

            this.Controls.Add(lblCategory);
            this.Controls.Add(txtCategory);

            this.Controls.Add(chkAvailable);

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnClear);

            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);

            this.Controls.Add(dgvBooks);
        }

        // 3. LOAD ALL BOOKS

        private void LoadBooks()
        {
            try
            {
                using (SQLiteConnection con =
                    new SQLiteConnection(ConnectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Books";

                    using (SQLiteDataAdapter adapter =
                        new SQLiteDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();

                        adapter.Fill(dt);

                        dgvBooks.DataSource = dt;
                    }
                }

                dgvBooks.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Load Error: " + ex.Message
                );
            }
        }

        // 4. ADD BOOK

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "" ||
                txtAuthor.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Please enter Book Title and Author."
                );
                return;
            }

            try
            {
                using (SQLiteConnection con =
                    new SQLiteConnection(ConnectionString))
                {
                    con.Open();

                    string query = @"
                        INSERT INTO Books
                        (Title, Author, Category, Available)
                        VALUES
                        (@Title, @Author, @Category, @Available)";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Title", txtTitle.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Author", txtAuthor.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Category", txtCategory.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Available",
                            chkAvailable.Checked ? 1 : 0);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Book Added Successfully!");

                LoadBooks();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Add Error: " + ex.Message
                );
            }
        }

        // 5. UPDATE BOOK

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (selectedBookID == 0)
            {
                MessageBox.Show("Please select a book first.");
                return;
            }

            if (txtTitle.Text.Trim() == "" ||
                txtAuthor.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Title and Author are required."
                );
                return;
            }

            try
            {
                using (SQLiteConnection con =
                    new SQLiteConnection(ConnectionString))
                {
                    con.Open();

                    string query = @"
                        UPDATE Books SET
                        Title=@Title,
                        Author=@Author,
                        Category=@Category,
                        Available=@Available
                        WHERE BookID=@ID";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Title", txtTitle.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Author", txtAuthor.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Category", txtCategory.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Available",
                            chkAvailable.Checked ? 1 : 0);

                        cmd.Parameters.AddWithValue(
                            "@ID", selectedBookID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Book Updated Successfully!");

                LoadBooks();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Update Error: " + ex.Message
                );
            }
        }

        // 6. DELETE BOOK

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedBookID == 0)
            {
                MessageBox.Show("Please select a book first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this book?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection con =
                        new SQLiteConnection(ConnectionString))
                    {
                        con.Open();

                        string query =
                            "DELETE FROM Books WHERE BookID=@ID";

                        using (SQLiteCommand cmd =
                            new SQLiteCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue(
                                "@ID", selectedBookID);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show(
                        "Book Deleted Successfully!"
                    );

                    LoadBooks();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Delete Error: " + ex.Message
                    );
                }
            }
        }

        // 7. SEARCH BOOK

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection con =
                    new SQLiteConnection(ConnectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT * FROM Books
                        WHERE Title LIKE @Search
                        OR Author LIKE @Search
                        OR Category LIKE @Search";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Search",
                            "%" + txtSearch.Text.Trim() + "%"
                        );

                        using (SQLiteDataAdapter adapter =
                            new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            dgvBooks.DataSource = dt;
                        }
                    }
                }

                dgvBooks.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Search Error: " + ex.Message
                );
            }
        }

        // 8. SELECT BOOK FROM DATAGRIDVIEW

        private void DgvBooks_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dgvBooks.Rows[e.RowIndex];

                selectedBookID = Convert.ToInt32(
                    row.Cells["BookID"].Value
                );

                txtTitle.Text =
                    row.Cells["Title"].Value?.ToString() ?? "";

                txtAuthor.Text =
                    row.Cells["Author"].Value?.ToString() ?? "";

                txtCategory.Text =
                    row.Cells["Category"].Value?.ToString() ?? "";

                chkAvailable.Checked =
                    Convert.ToInt32(
                        row.Cells["Available"].Value
                    ) == 1;
            }
        }

        // 9. CLEAR BUTTON

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearFields();
            LoadBooks();
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtCategory.Clear();
            txtSearch.Clear();

            chkAvailable.Checked = true;

            selectedBookID = 0;

            dgvBooks.ClearSelection();
        }
    }
}
