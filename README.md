# Library Book Manager

## Project Description

Library Book Manager is a simple database-connected desktop application developed using C# and Windows Forms (.NET). It helps users manage library book records easily.

The application allows users to add new books, update existing book information, delete records, and search for books. All book records are displayed in a DataGridView for easy viewing and management.

The project uses a Books table to store book information in a database.

## Features

- Add new book records.
- Update existing book details.
- Delete book records.
- Search books by title, author, or other available fields.
- Display all book records in a DataGridView.
- Store book information in a database.
- Manage book availability status.
- Perform CRUD (Create, Read, Update, Delete) operations.

## Technologies Used

- C#
- .NET
- Windows Forms (WinForms)
- Visual Studio
- SQLite Database (if implemented)

## Database Structure

### Table: Books

| Column Name | Data Type | Description |
|---|---|---|
| BookID | INTEGER | Unique ID of each book |
| Title | TEXT | Name of the book |
| Author | TEXT | Book author's name |
| Category | TEXT | Book category |
| Available | BOOLEAN | Book availability status |

## Windows Forms Controls Used

| Control | Purpose |
|---|---|
| TextBox | Enter book title, author, and search text |
| ComboBox | Select book category |
| CheckBox | Set book availability |
| Button | Add, Update, Delete, and Search records |
| DataGridView | Display book records |
| Label | Display field names and information |
| MessageBox | Display confirmation and error messages |

## Main Functionalities

### 1. Add Book
Enter book details and save a new book record to the database.

### 2. Update Book
Select an existing book record and modify its information.

### 3. Delete Book
Delete the selected book record from the database.

### 4. Search Book
Search for books using available information such as title or author.

### 5. Display Records
Show all saved book records in a DataGridView.


## Author

**Vishnu Kumar**

**output screen**
<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/6ba081fb-b222-496f-b133-4b6a79ec5059" />
