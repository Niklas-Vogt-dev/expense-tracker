# Expense Tracker

A desktop expense tracking application built with **C# and .NET 8 using WPF and SQLite**.

The application allows users to manage income and expenses, track their current balance, analyze monthly spending, and organize transactions by category.

## Features

* Create, edit and delete transactions
* Track income and expenses
* Automatic balance calculation
* Monthly expense overview
* Navigate between months
* Expense breakdown by category
* Percentage-based category visualization
* Search transactions by category or description
* Persistent user settings
* Optional delete confirmation
* Automatic demo data on first launch
* SQLite database for persistent data storage
* Dark-themed WPF user interface

## Technologies

* **C#**
* **.NET 8**
* **WPF**
* **SQLite**
* **Visual Studio**

## Project Structure

```text
expense-tracker
├── Data
│   ├── AppSettings.cs
│   ├── Database.cs
│   └── SettingsManager.cs
├── Helpers
│   └── AmountParser.cs
├── Models
│   ├── Transaction.cs
│   └── ExpenseCategory.cs
├── Windows
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── TransactionWindow.xaml
│   ├── TransactionWindow.xaml.cs
│   ├── DeleteConfirmationWindow.xaml
│   ├── DeleteConfirmationWindow.xaml.cs
│   ├── WelcomeWindow.xaml
│   ├── WelcomeWindow.xaml.cs
│   ├── SettingsWindow.xaml
│   └── SettingsWindow.xaml.cs
```

## Getting Started

### Requirements

* Windows
* .NET 8 SDK
* Visual Studio 2022 or later

### Installation

Clone the repository:

```bash
git clone https://github.com/Niklas-Vogt-dev/expense-tracker.git
```

Open the solution in Visual Studio and build the project.

On the first launch, the application automatically creates the SQLite database and loads demo data.

## Usage

The main dashboard provides an overview of the current account balance and monthly expenses.

Transactions can be added, edited, searched and deleted. Monthly navigation allows users to review spending for previous months, while the category overview shows how expenses are distributed.

## Purpose

This project was created to gain practical experience with **C#, .NET, WPF and SQLite** while building a complete desktop application with a clean user interface and persistent data storage.
