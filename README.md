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
│   ├── 3D Laboratory Environment with Apppratus/     # Môi trường phòng thí nghiệm 3D
│   ├── CurvedUI/                                     # UI dạng cong tối ưu cho VR
│   ├── HandGrabInteractableDataCollection/           # Dữ liệu grab & hand tracking
│   ├── Image/                                        # Ảnh, texture
│   ├── Oculus/                                       # Meta/Oculus SDK chính thức
│   ├── Plugins/                                      # Plugin bên thứ 3
│   ├── ProBuilder Data/                              # Dữ liệu từ ProBuilder
│   ├── Resources/                                    # File load runtime
│   ├── Samples/                                      # Tài nguyên mẫu từ package
│   ├── Scenes/                                       # Scene chính của ứng dụng
│   ├── ScenesLab/                                    # Scene lab thử nghiệm
│   ├── Script/                                       # Mã nguồn C#                           
│   ├── TextMesh Pro/                                 # Văn bản UI
│   ├── TutorialInfo/                                 # Thư mục auto từ Unity
│   ├── XR/                                           # Hệ thống XR của Unity
│   ├── XRI/                                          # XR Interaction Toolkit
│   ├── dungcu/                                       # Dụng cụ thí nghiệm TLC
│   └── msVFX_Free Smoke Effects Pack/                # Hiệu ứng khói – VFX
│
├── ProjectSettings/                                  # Cấu hình Unity project
├── Packages/                                         # Danh sách package
└── README.md                                         # File mô tả dự án


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
