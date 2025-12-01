# 🧪 **Hệ thống mô phỏng thí nghiệm Sắc ký lớp mỏng (TLC) bằng công nghệ VR**

**Ứng dụng chạy trên Meta Quest -- phát triển bằng Unity & Meta XR SDK**

## 📌 Giới thiệu

Hệ thống mô phỏng thực hành thí nghiệm Sắc ký lớp mỏng (Thin-Layer
Chromatography -- TLC) trong môi trường thực tế ảo (VR), giúp sinh viên
có thể thực hành đầy đủ quy trình, tương tác trực tiếp trong phòng thí
nghiệm ảo và lưu dữ liệu học tập qua Firebase.

## 📂 Cấu trúc thư mục dự án

    Project/
    ├── Assets/
    │   ├── Models/
    │   ├── Scenes/
    │   ├── Scripts/
    │   │   ├── TLCSteps/
    │   │   ├── Interaction/
    │   │   ├── UI/
    │   │   └── Firebase/
    │   ├── Prefabs/
    │   ├── XR/
    │   └── Materials/
    ├── ProjectSettings/
    ├── Packages/
    └── README.md

## 🧬 Chức năng chính

-   Mô phỏng phòng thí nghiệm 3D
-   Quy trình TLC theo từng bước
-   Tương tác VR: controller + hand tracking
-   Ghi kết quả lên Firebase

## ⚙️ Yêu cầu phần mềm

-   Unity LTS (2021/2022/2023)
-   Meta XR SDK
-   Android Build Support
-   Firebase SDK for Unity

## 🎮 Cách chạy dự án

1.  Mở project trong Unity\
2.  Cài XR Plugin Management + Meta XR SDK\
3.  Chuyển sang Android\
4.  Build & Run vào Meta Quest

## ☁️ Cấu hình Firebase

File cần thiết:

    Assets/StreamingAssets/google-services.json

## 📊 Dữ liệu lưu lên Firebase

-   completedSteps\
-   timeSpent\
-   errorCount\
-   timestamp

## 📝 Tác giả

Phạm Minh Quân -- Đại học Kinh tế Quốc dân

## 📄 Giấy phép

Sử dụng cho mục đích khóa luận tốt nghiệp.
