# SE104_ShopManagement #
Ứng dụng quản lí shop bán hàng với giao diện được xây dựng trên framework WPF.

## Tính năng ##
-	Đăng ký, đăng nhập và đăng xuất.
-	Gửi và nhận: văn bản,  hình ảnh, video, sticker, file... trong thời gian thực.
-	Cài đặt, xem danh sách sticker.
-	Quản lý lịch sử cuộc trò chuyện.
-	Tìm kiếm, kết bạn, tạo cuộc trò chuyện.
-	Chức năng chuyển theme, chuyển ngôn ngữ.
-	Quản lý thông báo, lời mời kết bạn.
-	Quản lý file media, tập tin.
-	Download Manager.
-	Chỉnh sửa thông tin cá nhân.
-	Đổi màu cuộc trò chuyện.

## Yêu cầu cài đặt ##
- Client:
    - .NET Framework 4.7.2
- Server:
    - .NET Framework 4.7.2
    - MongoDB Server ([Install Guide][mongodb_install])

## Hướng dẫn sử dụng ##
- Bước 1: Cài đặt đầy đủ phần mềm yêu cầu
- Bước 2: Chạy Server
- Bước 3: Mở ứng dụng và tận hưởng

## Team ##
- Nghi Lâm Minh Khôi - 20520593
- Châu Đức Hiệp - 20520499
- Nguyễn Huy Trí Dũng - 20520459
- Nguyễn Khánh Huyền - 20520558

## Công cụ ##
- DotNetty: Hỗ trợ xây dựng socket server
- Log4Net: Ghi log, xuất log và tracking
- CNetwork: Đơn giản hóa triển khai Network/API bằng các interface dựng sẵn
- MongoDB: nền tảng cơ sở dữ liệu NoSQL

## TODO ##
- Hỗ trợ cơ sở dữ liệu SQL (đã có thiết kế)
- Thêm tính năng lọc tin nhắn
- Hỗ trợ gửi tệp âm thanh
- Tính năng gọi điện, face-time
- Tối ưu giao tiếp Server-Client
- Xây dựng mô hình Microservice

## Known Issuses ##
- Client không thể tải hình ảnh: mở server dưới quyền Administrator

[//]: # (LINKS)
[mongodb_install]: https://docs.mongodb.com/manual/administration/install-community/