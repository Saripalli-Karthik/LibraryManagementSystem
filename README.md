# Library Management System API

A complete Library Management System built with **.NET 8**, **Entity Framework Core**, **SQL Server**, and **JWT-based authentication**. The system supports **Admin**, **Librarian**, and **Student** roles with role-based authorization and fine-grained book borrowing, returning, and fine tracking logic.

---

## 🚀 Features

* 🔐 JWT Authentication with ASP.NET Core Identity
* 📚 Book CRUD operations (Admin/Librarian only)
* 🙋 User registration & login with role selection
* 📖 Borrow/Return Books (Students or Admin/Librarian)
* 💰 Fine system for late returns
* ✅ XML Comments integrated with Swagger
* 🌐 Swagger UI with JWT token support
* 🧪 Testable services & controller refactoring ready for unit testing

---

## 🧰 Technologies Used

* ASP.NET Core 8 Web API
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Bearer Authentication
* Swagger / Swashbuckle

---

## 🛠️ Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* (Optional) [Postman](https://www.postman.com/) for testing

### Running the API

1. Clone the repository:

```bash
git clone https://github.com/Saripalli-Karthik/LibraryManagementAPI.git
cd LibraryManagementAPI
```

2. Update your connection string in `appsettings.json` if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

3. Run migrations & seed the database:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run
```

5. Visit Swagger at: `https://localhost:7002/swagger`

---

## 👥 Roles

* **Admin**: Full access
* **Librarian**: Manage books and help with borrows/returns
* **Student**: Borrow/Return books, view and pay fines

---

## 📦 API Endpoints Overview

### 🔐 Authentication

* `POST /api/auth/register`
* `POST /api/auth/login`

### 📚 Book

* `GET /api/book`
* `GET /api/book/{id}`
* `POST /api/book` *(Admin/Librarian)*
* `PUT /api/book/{id}` *(Admin)*
* `DELETE /api/book/{id}` *(Admin)*

### 🧾 Borrow

* `POST /api/borrow`
* `GET /api/borrow/my` *(Student)*
* `POST /api/borrow/return`

### 💵 Fines

* `GET /api/fine`
* `POST /api/fine/pay`

---

## ✅ Seeding

On application startup, the following roles and users are seeded:

* Roles: `Admin`, `Librarian`, `Student`
* Default Admin user:

  * Email: `admin@lib.com`
  * Password: `Admin@123`

---

## 🪪 License

This project uses the **MIT License**. Feel free to fork, contribute, or use it in commercial or personal projects.

---

## 📬 Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you'd like to change.

---

## 📧 Contact

Created by Karthik Saripalli - feel free to reach out via GitHub!
