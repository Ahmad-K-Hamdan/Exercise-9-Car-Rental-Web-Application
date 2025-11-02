# 🚗 Hamdan's Car Rental System

A complete **Car Rental Management System** built using **ASP.NET Core MVC** with **Entity Framework Core** and **Identity**.  
The application supports **role-based access control**, allowing customers to browse and rent cars while admins can manage cars, users, and reservations through a clean, responsive dashboard.

---

## ✨ Features

### 👥 Roles & Authentication
- **Customers**
  - View available cars
  - Make and manage reservations
  - View booking history
- **Admins**
  - Manage cars (add, hide/unhide, view status)
  - Manage users (promote to admin, view reservations)
  - Manage all reservations (search, filter, monitor overdue)

---

### 🚘 Cars Management
- Add new cars with details and images  
- Hide or unhide cars from the public view  
- Automatic availability updates when cars are rented or returned  

---

### 📅 Reservations
- Customers can rent cars by selecting start and return dates  
- Automatic duration and price calculation  
- Admins can view and filter reservations (active, overdue, completed)  
- AJAX-based search and filtering for smooth UX  

---

### 🧩 Tech Stack
- **Backend:** ASP.NET Core 8.0 (MVC)
- **Database:** Microsoft SQL Server + Entity Framework Core
- **Authentication:** ASP.NET Core Identity (Admin / Customer roles)
- **Frontend:** Razor Views, Bootstrap 5, jQuery (AJAX)
- **Architecture:** Repository pattern + Exception middleware

---

### ⚙️ Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/<your-username>/Hamdan-Car-Rental.git
   cd Hamdan-Car-Rental
   ```

2. **Update the database connection string**  
   In `appsettings.json`, set your SQL Server connection under `"DefaultConnection"`.

3. **Run Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. The system automatically creates two roles on startup:
   - `Admin`
   - `Customer`

   You can assign the **Admin** role manually in the database or promote a user through the admin panel.

---

### 👨‍💻 Author
**Ahmad Hamdan**  
Fifth-year Computer Engineering Student — Birzeit University  

---

### 📜 License
This project is open-source and available under the **MIT License**.
