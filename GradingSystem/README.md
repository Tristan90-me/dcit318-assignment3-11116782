# 📝 Grading System with File I/O - DCIT 318 Assignment 3 (Question 4)

This C# console application demonstrates **File I/O**, **Custom Exceptions**, and **Data Validation** by processing student records from an input file, computing grades, and writing the results to an output file. It also includes **interactive features** for managing students after the initial processing.

---

## ✅ Features

### 🔹 Core Requirements
- Reads student data from an **input file** (`students.txt`).
- Validates data and handles errors with **custom exceptions**:
  - `InvalidScoreFormatException`
  - `MissingFieldException`
- Computes grades using the following scale:
  - **80–100 → A**
  - **70–79 → B**
  - **60–69 → C**
  - **50–59 → D**
  - **Below 50 → F**
- Generates a **formatted report file** (`report.txt`).

### 🔹 Additional Enhancements
- **Interactive Menu** for user operations:
  - View all students and grades.
  - Add new student dynamically with validation.
  - Save updated report file.
  - Exit gracefully.
- **Validation** for:
  - Missing fields.
  - Invalid score format or range.
- **Exception Handling** for:
  - `FileNotFoundException`
  - Custom exceptions
  - Generic errors.
- **Colored Console Output** for better UX.

---
## 💻 How to Run

1. Clone the repository from GitHub or download the source files.
2. Open the project in **Visual Studio** or **Visual Studio Code** with the C# extension installed.
3. Build and run the application.

```bash
cd GradingSystem

dotnet run
