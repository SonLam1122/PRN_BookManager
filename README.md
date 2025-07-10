# Trình quản lý sách

Đây là một ứng dụng **Windows Presentation Foundation (WPF)** dành cho máy tính để bàn, được xây dựng bằng **.NET 8** để quản lý một hiệu sách. Ứng dụng này tuân theo kiến trúc **Model-View-ViewModel (MVVM)** và sử dụng **Entity Framework Core** để tương tác với cơ sở dữ liệu **SQL Server**.

---

## 🚀 Tính năng

* **Quản lý sách:**
    * Thêm, cập nhật và xóa sách.
    * Xem danh sách chi tiết của tất cả các cuốn sách, bao gồm cả tên tác giả và danh mục.
* **Quản lý tác giả:**
    * Thực hiện các thao tác **CRUD** (Tạo, Đọc, Cập nhật, Xóa) cho các tác giả.
* **Quản lý danh mục:**
    * Thực hiện các thao tác **CRUD** cho các danh mục sách.
* **Tìm kiếm và lọc:**
    * Tìm kiếm sách theo tiêu đề, **ISBN**, hoặc mô tả.
    * Lọc sách theo tác giả hoặc danh mục.

---

## 🛠️ Công nghệ sử dụng

* **.NET 8**
* **WPF (Windows Presentation Foundation)**
* **C#**
* **Entity Framework Core 9.0.6:** Được sử dụng làm **ORM** (Object-Relational Mapper) để tương tác với cơ sở dữ liệu.
* **Microsoft SQL Server:** Được sử dụng làm hệ quản trị cơ sở dữ liệu.
* **Kiến trúc MVVM (Model-View-ViewModel):** Để tách biệt giao diện người dùng khỏi logic nghiệp vụ.

---

## ⚙️ Cài đặt và thiết lập

Để chạy dự án này trên máy cục bộ của bạn, hãy làm theo các bước sau:

**1. Điều kiện tiên quyết:**

* **Visual Studio 2022 trở lên:** Với .NET desktop development workload được cài đặt.
* **.NET 8 SDK**
* **Microsoft SQL Server**

**2. Sao chép kho lưu trữ:**

git clone [https://github.com/sonlam1122/prn_bookmanager.git](https://github.com/sonlam1122/prn_bookmanager.git)
cd prn_bookmanager/PRN_BookManager-1b3ade31e8fcfd59be68e4b2d4f1d8261d7887d6


**3. Thiết lập cơ sở dữ liệu:**

  * Mở SQL Server Management Studio (SSMS).
  * Chạy tập lệnh SQL có trong `Database/database.sql` để tạo cơ sở dữ liệu `BookStoreManagement` và các bảng cần thiết.

**4. Cấu hình chuỗi kết nối:**

  * Mở tệp `appsettings.json`.
  * Sửa đổi `DefaultConnection` trong phần `ConnectionStrings` để trỏ đến phiên bản SQL Server của bạn.



{
    "ConnectionStrings": {
        "DefaultConnection": "Server=TEN_MAY_CHU_CUA_BAN;Database=BookStoreManagement;User Id=ten_nguoi_dung_cua_ban;Password=mat_khau_cua_ban;TrustServerCertificate=true;Encrypt=false;"
    }
}


**5. Chạy ứng dụng:**

  * Mở tệp `BookManager.sln` bằng Visual Studio.
  * Đặt `BookManager` làm dự án khởi động.
  * Nhấn `F5` hoặc nút `Start` để xây dựng và chạy ứng dụng.

-----

## 📖 Cách sử dụng

  * **Cửa sổ chính:** Hiển thị danh sách tất cả các sách trong một lưới dữ liệu.
  * **Thêm sách:** Nhấp vào nút "Add" để thêm một mục sách mới vào lưới.
  * **Cập nhật sách:** Sửa đổi thông tin của một cuốn sách trực tiếp trong lưới và nhấp vào nút "Update" để lưu các thay đổi.
  * **Xóa sách:** Chọn một cuốn sách và nhấp vào nút "Delete" để xóa nó.
  * **Tìm kiếm và lọc:** Sử dụng hộp văn bản tìm kiếm và các hộp tổ hợp lọc để tìm các sách cụ thể.
  * **Quản lý tác giả/danh mục:** Nhấp vào các nút "Manage Authors" hoặc "Manage Categories" để mở các cửa sổ chuyên dụng để quản lý các mục này.

-----

## 📂 Cấu trúc mã

Dự án được cấu trúc bằng cách sử dụng các thư mục riêng biệt cho từng thành phần của kiến trúc MVVM:

  * **`/Data`**: Chứa lớp `BookStoreContext` chịu trách nhiệm cho các tương tác với cơ sở dữ liệu bằng cách sử dụng Entity Framework Core.
  * **`/Models`**: Chứa các lớp mô hình (`Books`, `Authors`, `Categories`) đại diện cho các thực thể của ứng dụng.
  * **`/Services`**: Chứa lớp `BookService`, đóng gói logic nghiệp vụ của ứng dụng, chẳng hạn như truy xuất và thao tác dữ liệu.
  * **`/ViewModels`**: Chứa các lớp ViewModel (`MainViewModel`) đóng vai trò là cầu nối giữa các Views và Models.
  * **`/Views`**: Chứa các tệp XAML và code-behind cho các cửa sổ của ứng dụng (`MainWindow`, `AuthorManagementWindow`, `CategoryManagementWindow`).
  * **`/Converters`**: Chứa các bộ chuyển đổi giá trị, chẳng hạn như `BooleanToVisibilityConverter`, được sử dụng để liên kết dữ liệu trong XAML.
  * **`App.xaml.cs`**: Tệp code-behind cho tệp App.xaml. Nó chứa logic khởi động cho ứng dụng, bao gồm cả việc khởi tạo cơ sở dữ liệu.
  * **`appsettings.json`**: Tệp cấu hình chứa chuỗi kết nối cơ sở dữ liệu.

-----

## 🤝 Đóng góp

Chúng tôi hoan nghênh các đóng góp\! Nếu bạn muốn đóng góp cho dự án này, vui lòng fork kho lưu trữ và gửi một pull request.

1.  Fork dự án
2.  Tạo nhánh tính năng của bạn (`git checkout -b feature/AmazingFeature`)
3.  Commit các thay đổi của bạn (`git commit -m 'Add some AmazingFeature'`)
4.  Đẩy lên nhánh (`git push origin feature/AmazingFeature`)
5.  Mở một Pull Request

