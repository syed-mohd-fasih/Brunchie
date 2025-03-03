# **Brunchie - Campus Food Ordering Web App**

## **Overview**
**Brunchie** is a web application designed to streamline food ordering for students in educational institutions. It connects students with on-campus vendors, allowing them to browse menus, place orders, and avoid long wait times during rush hours.

This project is built using **ASP.NET Core** for the backend and **React.js with Tailwind CSS** including other UI frameworks for the frontend, ensuring a scalable and efficient user experience.

---

## **Features**

### **1. User Authentication**  
- Students and vendors can sign up and log in.  
- Role-based access:  
  - **Students** get a personalized food feed.  
  - **Vendors** manage menus and receive orders.  

### **2. Student Dashboard**  
- **Personalized Menu Feed**: Displays available vendors and menu items based on the student’s campus.  
- **Order Placement**: Students can select menu items and place orders.  
- **Order History**: View past orders and track statuses.  

### **3. Vendor Dashboard**  
- **Menu Management**: Vendors can add, edit, and delete menu items.  
- **Order Management**: View incoming student orders and update their status (e.g., "Preparing", "Ready for Pickup").  
- **Profile Settings**: Add business details and payment instructions.  

### **4. No Payment Gateway (Yet)**  
- Instead of direct online payments, vendors can provide payment options like bank transfer details or accept cash.  

### **5. Responsive Design**  
- Optimized for both desktop and mobile use.  
- Built with **React.js + Tailwind CSS** for a fast and modern UI.  

### **6. Scalable Backend**  
- Powered by **ASP.NET Core**, using **Entity Framework Core** and **SQLite (for now, with future scalability to PostgreSQL or Azure SQL)**.  
- REST API architecture for smooth frontend-backend communication.  

---

## **Tech Stack**  

### **Frontend**  
- **React.js** – Fast and efficient UI framework.  
- **Tailwind CSS** – Utility-first CSS framework for styling.  
- **Material UI (MUI)** (optional) – For additional UI components.  

### **Backend**  
- **ASP.NET Core** – Robust and scalable backend framework.  
- **Entity Framework Core** – ORM for database management.  
- **SQLite** – Lightweight database (can be scaled up as needed).  

---

## **License**  
This project is open-source and available under the **MIT License**.  
