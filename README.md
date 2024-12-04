
# eShopSolution

**eShopSolution** là một nền tảng thương mại điện tử được xây dựng với các công nghệ hiện đại, tập trung vào khả năng quản lý sản phẩm, người dùng và đơn hàng một cách hiệu quả.

---

## Mục Lục  
- [Tổng Quan](#tổng-quan)  
- [Công Nghệ Sử Dụng](#công-nghệ-sử-dụng)  
- [Tính Năng](#tính-năng)  
- [Hướng Dẫn Cài Đặt](#hướng-dẫn-cài-đặt)  
- [Cách Sử Dụng](#cách-sử-dụng)  
- [Đóng Góp](#đóng-góp)  
- [Giấy Phép](#giấy-phép)  

---

## Tổng Quan  

**eShopSolution** cung cấp giải pháp thương mại điện tử đầy đủ chức năng, phù hợp cho cả người dùng cuối và quản trị viên. Dự án bao gồm các tính năng như quản lý sản phẩm, xác thực người dùng, xử lý đơn hàng và giao diện người dùng linh hoạt.

---

## Công Nghệ Sử Dụng  

### Backend:  
- **ASP.NET Core**: Xây dựng Web API hiệu năng cao và bảo mật.  
- **Entity Framework Core**: Quản lý thao tác cơ sở dữ liệu.  
- **SQL Server**: Cơ sở dữ liệu chính để lưu trữ và quản lý dữ liệu.  
- **AutoMapper**: Hỗ trợ ánh xạ giữa các đối tượng.  
- **Fluent Validation**: Xác thực dữ liệu đầu vào.  
- **JWT (JSON Web Token)**: Xác thực và phân quyền người dùng.  

### Frontend:  
- **Razor Pages**: Tạo giao diện người dùng động và trực quan.  
- **Bootstrap**: Đảm bảo giao diện đẹp và tương thích trên nhiều thiết bị.  

### Công Cụ Khác:  
- **Swagger**: Tài liệu hóa và kiểm thử API.  
- **SendGrid**: Gửi email thông báo.  
- **Cloudinary**: Quản lý và lưu trữ hình ảnh.  

---

## Tính Năng  

### Dành Cho Người Dùng Cuối:  
- Duyệt sản phẩm với tính năng tìm kiếm và phân trang.  
- Thêm sản phẩm vào giỏ hàng và đặt hàng.  
- Xem lịch sử và trạng thái đơn hàng.  
- Quản lý thông tin cá nhân và bảo mật tài khoản.  

### Dành Cho Quản Trị Viên:  
- Quản lý danh mục sản phẩm (thêm, sửa, xóa).  
- Quản lý danh mục, thương hiệu, và hình ảnh sản phẩm.  
- Theo dõi và xử lý đơn hàng của khách hàng.  
- Phân tích dữ liệu doanh số và tạo báo cáo.  

---

## Hướng Dẫn Cài Đặt  

### Yêu Cầu:  
1. .NET SDK (phiên bản 6.0 hoặc cao hơn).  
2. SQL Server để quản lý cơ sở dữ liệu.  

### Các Bước Thực Hiện:  

1. Clone repository về máy:  
   ```bash  
   git clone https://github.com/duckkoder/eShopSolution.git  
   ```  

2. Điều hướng đến thư mục dự án và khôi phục các dependency:  
   ```bash  
   cd eShopSolution  
   dotnet restore  
   ```  

3. Cập nhật chuỗi kết nối cơ sở dữ liệu trong `appsettings.json`.  

4. Áp dụng các migration vào cơ sở dữ liệu:  
   ```bash  
   dotnet ef database update  
   ```  

5. Chạy ứng dụng:  
   ```bash  
   dotnet run  
   ```  

---

## Cách Sử Dụng  

1. **Giao Diện Quản Trị Viên**:  
   - Đăng nhập bằng tài khoản quản trị viên để quản lý sản phẩm, danh mục, và đơn hàng.  

2. **Giao Diện Người Dùng**:  
   - Tìm kiếm và mua sắm sản phẩm với trải nghiệm người dùng tối ưu.  

3. **API**:  
   - Sử dụng Swagger để kiểm tra và tích hợp API với các hệ thống bên ngoài.  

---

## Đóng Góp  

Chào mừng mọi đóng góp! Vui lòng làm theo các bước sau:  
1. Fork repository.  
2. Tạo nhánh mới để thêm tính năng hoặc sửa lỗi.  
3. Commit thay đổi và gửi pull request.  

---

## Giấy Phép  

Dự án được cấp phép theo giấy phép MIT. Vui lòng xem file `LICENSE` để biết thêm thông tin.  

---
