eShopSolution
eShopSolution là một nền tảng thương mại điện tử được xây dựng với các công nghệ hiện đại, tập trung vào khả năng quản lý sản phẩm, người dùng và đơn hàng một cách hiệu quả.

Mục Lục
Tổng Quan
Công Nghệ Sử Dụng
Tính Năng
Hướng Dẫn Cài Đặt
Cách Sử Dụng
Đóng Góp
Giấy Phép
Tổng Quan
eShopSolution cung cấp giải pháp thương mại điện tử đầy đủ chức năng, phù hợp cho cả người dùng cuối và quản trị viên. Dự án bao gồm các tính năng như quản lý sản phẩm, xác thực người dùng, xử lý đơn hàng và giao diện người dùng linh hoạt.

Công Nghệ Sử Dụng
Backend:
ASP.NET Core: Xây dựng Web API hiệu năng cao và bảo mật.
Entity Framework Core: Quản lý thao tác cơ sở dữ liệu.
SQL Server: Cơ sở dữ liệu chính để lưu trữ và quản lý dữ liệu.
AutoMapper: Hỗ trợ ánh xạ giữa các đối tượng.
Fluent Validation: Xác thực dữ liệu đầu vào.
JWT (JSON Web Token): Xác thực và phân quyền người dùng.
Frontend:
Razor Pages: Tạo giao diện người dùng động và trực quan.
Bootstrap: Đảm bảo giao diện đẹp và tương thích trên nhiều thiết bị.
Công Cụ Khác:
Swagger: Tài liệu hóa và kiểm thử API.
SendGrid: Gửi email thông báo.
Cloudinary: Quản lý và lưu trữ hình ảnh.
Tính Năng
Dành Cho Người Dùng Cuối:
Duyệt sản phẩm với tính năng tìm kiếm và phân trang.
Thêm sản phẩm vào giỏ hàng và đặt hàng.
Xem lịch sử và trạng thái đơn hàng.
Quản lý thông tin cá nhân và bảo mật tài khoản.
Dành Cho Quản Trị Viên:
Quản lý danh mục sản phẩm (thêm, sửa, xóa).
Quản lý danh mục, thương hiệu, và hình ảnh sản phẩm.
Theo dõi và xử lý đơn hàng của khách hàng.
Phân tích dữ liệu doanh số và tạo báo cáo.
Hướng Dẫn Cài Đặt
Yêu Cầu:
.NET SDK (phiên bản 6.0 hoặc cao hơn).
SQL Server để quản lý cơ sở dữ liệu.
Các Bước Thực Hiện:
Clone repository về máy:

bash
Sao chép mã
git clone https://github.com/duckkoder/eShopSolution.git  
Điều hướng đến thư mục dự án và khôi phục các dependency:

bash
Sao chép mã
cd eShopSolution  
dotnet restore  
Cập nhật chuỗi kết nối cơ sở dữ liệu trong appsettings.json.

Áp dụng các migration vào cơ sở dữ liệu:

bash
Sao chép mã
dotnet ef database update  
Chạy ứng dụng:

bash
Sao chép mã
dotnet run  
Cách Sử Dụng
Giao Diện Quản Trị Viên:

Đăng nhập bằng tài khoản quản trị viên để quản lý sản phẩm, danh mục, và đơn hàng.
Giao Diện Người Dùng:

Tìm kiếm và mua sắm sản phẩm với trải nghiệm người dùng tối ưu.
API:

Sử dụng Swagger để kiểm tra và tích hợp API với các hệ thống bên ngoài.
Đóng Góp
Chào mừng mọi đóng góp! Vui lòng làm theo các bước sau:

Fork repository.
Tạo nhánh mới để thêm tính năng hoặc sửa lỗi.
Commit thay đổi và gửi pull request.
Giấy Phép
Dự án được cấp phép theo giấy phép MIT. Vui lòng xem file LICENSE để biết thêm thông tin.

Nếu có thêm thông tin hoặc tính năng nào cần bổ sung, bạn có thể chỉnh sửa file này để phù hợp hơn với dự án! 😊






