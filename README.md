# 📚 Library Management System (LMS) (Under development)

## 👋 About This Project

I am building a **Library Management System (LMS)** as a **desktop application** using:

* **.NET (C#)** for the application layer
* **SQL Server** for the database

### 🧩 What Is This System?

The Library Management System is designed to **digitally manage the daily operations of a library**. It replaces manual processes (paper registers, spreadsheets) with a **centralized, structured, and reliable system**.

The system is responsible for managing:

* 📚 Books and their physical copies
* ✍️ Authors, genres, and publishers
* 👥 Library members and employees
* 🔄 Book borrowing and returning workflows
* ⏱️ Due dates, penalties, and payments
* 🔐 User access and role-based control

### 🎯 Problems This System Solves

This system allows the library to:

* Track **book availability** in real time
* Know **who borrowed what and when**
* Enforce borrowing rules and return deadlines
* Calculate **late return penalties** correctly
* Maintain a clear **transaction history** (borrows, returns, payments)
* Reduce errors caused by manual record keeping

### 🧠 Project Philosophy

This project represents my **system design and implementation journey**. I am starting from requirements and gradually building the system **feature by feature**, documenting:

* Design decisions
* Database interactions
* Business rules
* ADO.NET data access logic

This repository will grow **feature by feature**, not all at once, reflecting real-world software development practices.

---

## 🎯 Project Goals

By completing this project, I aim to:

* Build a **real-world desktop application**
* Apply **system design principles** in practice
* Design a **normalized SQL Server database**
* Implement clean **.NET architecture**
* Connect UI, business logic, and database correctly

---

## 🧠 Problem Statement

The Library Management System should allow a library to:

* Manage books and authors
* Manage library members
* Issue and return books
* Track overdue books and fines
* Generate reports
* Control access using user roles

---

## 🗂️ Project Journey & Phases

This project is built in **clear phases**, similar to real system design workflows.

---

## 🧩 Phase 1 — Requirements Analysis

### Functional Requirements

* Add, update, delete, and search books
* Add, update, delete, and search members
* Issue books to members
* Return books
* Track issued and available copies
* Calculate overdue fines
* User authentication (Admin / Librarian)

### Non-Functional Requirements

* Desktop-based application
* Separation of layers and concerns
* Easy-to-use UI
* Fast database operations
* Data consistency and integrity

---

## 🏗️ Phase 2 — System Architecture

I will design the system using a layered architecture:

* **Presentation Layer** (Windows Forms)
* **Business Logic Layer**
* **Data Access Layer** (ADO.NET)
* **SQL Server Database**

Each layer will have a clear responsibility.

---

## 🗃️ Phase 3 — Database Design

The database for this project is **already designed** and will be used as the **single source of truth** for the system.

It is a **relational SQL Server database** with proper normalization, primary keys, foreign keys, and many-to-many relationships.

### 🔹 Core Concepts Reflected in the Schema

* Separation between **People**, **Members**, and **Employees**
* Clear modeling of **Books**, **Authors**, **Genres**, and **Publishers**
* Many-to-many relationships using junction tables:

  * `BookAuthor`
  * `AuthorGenre`
  * `BookGenre`
* Tracking physical copies of books using `BookCopy`
* Issuing logic handled through `Borrow` and `BorrowDetail`
* Payment and penalty handling using `Payment` and `Penalty`

This schema will remain **stable** during development. My focus is on:

* Fully understanding each table
* Respecting constraints and relationships
* Writing correct and efficient SQL queries

---

## 🖥️ Phase 4 — UI Planning

I will design UI screens such as:

* Login Screen
* Dashboard
* Book Management
* Member Management
* Issue / Return Books
* Reports

Each screen will be implemented step by step.

---

## 🧠 Phase 5 — Data Access Implementation (ADO.NET)

I will use **ADO.NET** as the data access technology.

* Implement CRUD operations using SQL commands
* Use **parameterized queries** to prevent SQL injection
* Explicitly manage connections, commands, readers, and transactions

---

## 🧪 Phase 6 — Business Logic

I will implement:

* Validation rules
* Book availability checks
* Issue / return workflows
* Overdue detection
* Fine calculation logic

---

## 🖱️ Phase 7 — UI Implementation

I will:

* Connect UI to business logic
* Display data using grids and forms
* Implement search and filters
* Handle user interactions and errors

---

## 📊 Phase 8 — Reporting

Planned reports include:

* Issued books report
* Overdue books report
* Inventory report
* Member activity report

Optional exports:

* PDF
* Excel

---

## 🧪 Phase 9 — Testing

I will test:

* All CRUD operations
* Issue and return scenarios
* Edge cases
* Database integrity
* Role-based access

---

## 🚀 Phase 10 — Deployment

I will prepare:

* SQL Server database scripts
* Application installer (MSI / ClickOnce)
* Setup and usage documentation

---

## 📌 What I Will Learn From This Project

* System design for desktop applications
* SQL Server database modeling
* .NET desktop development
* Clean architecture principles
* Real-world CRUD and transactions
* Error handling and validation

---

## 🏁 Expected Outcome

After completing this project, I expect to:

* Have a complete **Library Management System**
* Understand system design beyond theory
* Be confident building desktop applications with .NET and SQL Server
* Showcase a real, structured project on GitHub 🚀

---

## 📅 Progress Tracking

This section will be updated as I move forward:

* [x] Requirements Analysis
* [x] Database Design
* [x] Data Access Layer
* [ ] UI Design
* [ ] Business Logic
* [ ] UI Implementation
* [ ] Reporting
* [ ] Testing
* [ ] Deployment
